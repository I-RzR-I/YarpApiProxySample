// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 16:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="CorsSetupServiceCollection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Infrastructure.Web.ServiceCollection
{
    /// <summary>
    ///     CORS setup
    /// </summary>
    public static class CorsSetupServiceCollection
    {
        /// <summary>
        ///     Add CORS module
        /// </summary>
        /// <param name="services">App service collection</param>
        /// <param name="configuration">App configuration</param>
        /// <remarks></remarks>
        public static void AddCorsModule(this IServiceCollection services, IConfiguration configuration)
                    => services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                /*.AllowCredentials()   
                                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                                    .WithOrigins(corsSettings.AllowedOrigins)*/
                                .Build();
                        });
                    });

        /// <summary>
        ///     Use CORS module
        /// </summary>
        /// <param name="app">App builder</param>
        /// <remarks></remarks>
        public static void UseCorsModule(this IApplicationBuilder app)
                    => app.UseCors();
    }
}