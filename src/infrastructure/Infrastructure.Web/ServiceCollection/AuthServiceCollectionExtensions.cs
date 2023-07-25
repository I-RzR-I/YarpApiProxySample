// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 16:46
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="AuthServiceCollectionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Infrastructure.Web.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Infrastructure.Web.ServiceCollection
{
    /// <summary>
    ///     Auth service collection
    /// </summary>
    public static class AuthServiceCollectionExtensions
    {
        /// <summary>
        ///     Register auth endpoint
        /// </summary>
        /// <param name="services">App service collection</param>
        /// <returns></returns>
        public static void RegisterAuthorizationEndpoint(this IServiceCollection services)
        {
            services.AddSingleton<TokenValidationParameters>();

            services.AddAuthentication("OwnJwtAuth")
                .AddScheme<AuthenticationSchemeOptions, OwnJwtAuthHandler>("OwnJwtAuth", null);
            services.AddAuthorization();
        }
    }
}