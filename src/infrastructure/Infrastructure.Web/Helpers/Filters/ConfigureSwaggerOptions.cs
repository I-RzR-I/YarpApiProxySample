// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 08:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="ConfigureSwaggerOptions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using Infrastructure.Common;
using Infrastructure.Common.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace Infrastructure.Web.Helpers.Filters
{
    /// <summary>
    ///     Configures the Swagger generation options.
    /// </summary>
    /// <remarks>
    ///     This allows API version to define a Swagger document per API version after the
    ///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
    /// </remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            var info = SystemApplication.GetAppInformation();
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, info));
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, SystemInformation info)
        {
            var docInfo = new OpenApiInfo
            {
                Title = $"{info.AppName} API",
                Version = description.ApiVersion.ToString(),
                Description = $"{info.AppName} version: {info.AppVersion}",
                TermsOfService = info.TermsOfService.IsNull() ? null : new Uri(info.TermsOfService),
                Contact = new OpenApiContact { Url = info.Contact.Url.IsNullOrEmpty() ? null : new Uri(info.Contact.Url), Email = info.Contact.Email, Name = info.Contact.Name }
            };

            if (description.IsDeprecated) docInfo.Description += " This API version has been deprecated.";

            return docInfo;
        }
    }
}