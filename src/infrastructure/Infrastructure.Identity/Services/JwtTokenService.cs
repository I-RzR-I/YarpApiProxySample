// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Identity
//  Author           : RzR
//  Created On       : 2023-07-21 22:49
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="JwtTokenService.cs" company="">
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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using Domain.Common.Dto;
using Domain.Common.Identity;
using Infrastructure.Identity.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

#endregion
#pragma warning disable CS1998

namespace Infrastructure.Identity.Services
{
    /// <inheritdoc cref="ITokenService" />
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Infrastructure.Identity.Services.JwtTokenService" /> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <remarks></remarks>
        public JwtTokenService(
            ILogger<JwtTokenService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<IResult<TokenResponse>> GenerateJwtTokenAsync(
            GenerateJwtTokenDto requestData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenId = Guid.NewGuid().ToString();
                var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, tokenId), 
                        new Claim(JwtRegisteredClaimNames.Iss, _configuration["JWTSettings:Issuer"]),
                        new Claim(JwtRegisteredClaimNames.Aud, _configuration["JWTSettings:Audience"]),
                        new Claim(JwtRegisteredClaimNames.Sub, requestData.UserId),
                        new Claim(JwtRegisteredClaimNames.UniqueName, requestData.UniqueName),
                        new Claim(JwtRegisteredClaimNames.Email, requestData.Email)
                    }
                    .Concat(requestData.CustomClaims?.Select(x => new Claim(x.claimType, x.claimValue)) ?? Enumerable.Empty<Claim>());

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecurityKey"]!));
                var signingCredentials =
                    new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["JWTSettings:Issuer"],
                    _configuration["JWTSettings:Audience"],
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWTSettings:DurationInMinutes"]!)),
                    signingCredentials);

                var token = tokenHandler.WriteToken(jwtSecurityToken);
                var refreshToken = GenerateRefreshToken(jwtSecurityToken, requestData.UserId, 
                    int.Parse(_configuration["JWTSettings:RefreshTokenDurationInDays"]!),
                    requestData.RequestId);

                return Result<TokenResponse>.Success(new TokenResponse
                {
                    access_token = token,
                    refresh_token = refreshToken.Token,
                    id_token = tokenId,
                    scope = requestData.UserTokenRequest.scope,
                    token_type = "Bearer"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error on generate auth token!");

                return Result<TokenResponse>.Failure("Internal error on generate auth token");
            }
        }

        /// <summary>
        ///     Generate refresh token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <param name="userId">User id</param>
        /// <param name="durationInDays">Token valid in days</param>
        /// <param name="requestId">Request id</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static RefreshToken GenerateRefreshToken(
            SecurityToken token, string userId, double durationInDays,
            string requestId)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[256];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                JwtId = token.Id,
                UserId = userId,
                CreationDateUtc = DateTime.UtcNow,
                ExpiryDateUtc = DateTime.UtcNow.AddDays(durationInDays),
                RequestId = requestId
            };
        }
    }
}