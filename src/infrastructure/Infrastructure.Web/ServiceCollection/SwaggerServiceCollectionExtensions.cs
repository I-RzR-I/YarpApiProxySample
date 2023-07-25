// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 16:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="SwaggerServiceCollectionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AggregatedGenericResultMessage.Extensions.Common;
using Infrastructure.Web.Helpers;
using Infrastructure.Web.Helpers.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

namespace Infrastructure.Web.ServiceCollection
{
    /// <summary>
    ///     Swagger service collection
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        ///     Add swagger
        /// </summary>
        /// <param name="services">App service collection</param>
        /// <param name="configuration">App configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            var authorityRawUrl = configuration.GetValue<string>("IdentityUrl");
            var tokenUrl = authorityRawUrl.IsNullOrEmpty()
                ? new Uri($"{authorityRawUrl}/api/v1/connect/auth", UriKind.Relative)
                : new Uri($"{authorityRawUrl}/api/v1/connect/auth");

            services.AddSwaggerGen(options =>
            {
                options.DocInclusionPredicate(SwaggerVersioning.DocInclusionPredicate);

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityDefinition("Password",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = null,
                                TokenUrl = tokenUrl,
                                Scopes = new Dictionary<string, string> { { "openid", "openid" }, { "profile", "profile" }, { "offline_access", "offline_access" }, { "email", "email" } }
                            }
                        },
                        In = ParameterLocation.Header,
                        Scheme = "Bearer"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        Array.Empty<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Password"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();

                options.CustomSchemaIds(x => x.FullName);

                options.SchemaFilter<EnumSchemaFilter>();
                options.DescribeAllParametersInCamelCase();

                options.OperationFilter<SwaggerAuthorizeCheckOperationFilter>();

                var filePath = Path.Combine(AppContext.BaseDirectory, "API.xml");
                options.IncludeXmlComments(filePath);
                options.CustomSchemaIds(type => type.ToString());
            });

            services.RegisterSwaggerVersion();

            return services;
        }

        /// <summary>
        ///     Register swagger version
        /// </summary>
        /// <param name="services">App service collection</param>
        /// <remarks></remarks>
        public static void RegisterSwaggerVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersion.Default;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        /// <summary>
        ///     User swagger module
        /// </summary>
        /// <param name="app">App builder</param>
        /// <param name="apiName">API name</param>
        /// <remarks></remarks>
        public static void UseSwaggerModule(this IApplicationBuilder app, string apiName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
                c.DisplayRequestDuration();
                c.ConfigObject.DocExpansion = DocExpansion.None;
                c.ConfigObject.DisplayRequestDuration = true;
                c.ConfigObject.Filter = string.Empty;
            });
        }
    }
}