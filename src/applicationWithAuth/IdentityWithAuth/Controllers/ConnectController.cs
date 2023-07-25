// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-21 13:54
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="ConnectController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Models;
using Domain.Common.Dto;
using Domain.Common.Identity;
using DomainCommonExtensions.CommonExtensions;
using IdentityWithAuth.Models.DataStore;
using Infrastructure.Common;
using Infrastructure.Identity.Abstractions.Services;
using Infrastructure.Web;
using Infrastructure.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace IdentityWithAuth.Controllers
{
    [ApiVersion("1.0")]
    public class ConnectController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ConnectController(ITokenService tokenService, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AuthAsync([FromForm] TokenRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(request.username);
            if (user.IsNull())
                return JsonResult(Result<TokenResponse>.NotFound("User not found"));

            var signInResult = await _signInManager.PasswordSignInAsync(user, request.password, false, false);

            return signInResult.Succeeded
                ? JsonResult(await _tokenService.GenerateJwtTokenAsync(
                    new GenerateJwtTokenDto()
                    {
                        UserId = user.Id,
                        UserTokenRequest = request,
                        RequestId = Guid.NewGuid().ToString(),
                        UniqueName = user.UserName,
                        Email = user.Email
                    }, cancellationToken))
                : JsonResult(Result<TokenResponse>.Failure("Auth fails"));
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [OnlyApiAuthorize]
        [ProducesResponseType(typeof(JWTSettings), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTokenSettingsAsync(CancellationToken cancellationToken = default)
            => await Task.FromResult(
                JsonResult(Result<JWTSettings>.Success(SystemApplication.JwtSettings)));
    }
}