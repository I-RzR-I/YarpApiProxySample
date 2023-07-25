// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-24 16:49
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-24 18:28
// ***********************************************************************
//  <copyright file="OwnJwtAuthHandler.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using Infrastructure.Common;
using Infrastructure.Web.Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

#endregion
#pragma warning disable CS1998
// ReSharper disable ClassNeverInstantiated.Global

namespace Infrastructure.Web.Handlers
{
    /// <summary>
    ///     Own custom JWT token validation
    /// </summary>
    public class OwnJwtAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger<OwnJwtAuthHandler> _logger;
        private readonly IConfiguration _configuration;

        /// <inheritdoc />
        public OwnJwtAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, ILogger<OwnJwtAuthHandler> logger1, IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _logger = logger1;
            _configuration = configuration;
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Path.ToString().Equals("/"))
            {
                var endpoint = Context.GetEndpoint();
                var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
                var hasAllowAnonymous = actionDescriptor != null && actionDescriptor.EndpointMetadata
                    .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)
                               || em.GetType() == typeof(OnlyApiAuthorizeAttribute));
                if (hasAllowAnonymous.Equals(true) || Context.Request.Headers["Authorization"].IsNull())
                    return AuthenticateResult
                        .Success(GetEmptySuccessAuth());

                var token = Context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (Context.IsNull() || token.IsNullOrEmpty())
                    return AuthenticateResult.Fail(string.Empty);

                SystemApplication.SetJwtSettings(Helpers.JwtSettingsHelper.
                    GetJwtSettings(_configuration.GetValue<string>("IdentityUrl")).Response);

                var validation = ValidateToken(Context, token);

                if (!validation.IsSuccess)
                    return AuthenticateResult.Fail(string.Empty);

                //TODO Set user claims
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "userid"),
                    new Claim(ClaimTypes.Name, "username")
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult
                    .Success(GetEmptySuccessAuth());
            }
        }

        /// <summary>
        ///     Validate token
        /// </summary>
        /// <param name="context">Current HTTP context</param>
        /// <param name="token">Auth token</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private IResult ValidateToken(HttpContext context, string token)
        {
            try
            {
                var jwtSettings = SystemApplication.JwtSettings;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSettings.SecurityKey);

                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true
                    }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                if (jwtToken.ValidTo < DateTime.UtcNow)
                    return Result.Failure(HttpStatusCode.Unauthorized.ToString(), "Token expired!");

                //TODO Add all validation that is needed

                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error in token validation");

                return Result.Failure("Token validation error");
            }
        }

        private AuthenticationTicket GetEmptySuccessAuth()
            => new AuthenticationTicket(new ClaimsPrincipal(
                    new ClaimsIdentity(new[] { new Claim(string.Empty, string.Empty) }, Scheme.Name)),
                Scheme.Name);
    }
}