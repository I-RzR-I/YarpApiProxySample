// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 08:49
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="SwaggerAuthorizeCheckOperationFilter.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using AggregatedGenericResultMessage.Extensions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace Infrastructure.Web.Helpers.Filters
{
    /// <summary>
    ///     Swagger authorize check filter
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SwaggerAuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        ///     Apply filter
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = new List<AuthorizeAttribute>();
            var filterDescriptor = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var allowAnonymous = filterDescriptor.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is IAllowAnonymousFilter);
            if (context.MethodInfo.DeclaringType != null)
            {
                authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>()
                    .ToList();

                if (!authAttributes.Any())
                    return;
            }

            if (!allowAnonymous)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }

            operation.Responses.Add("404", new OpenApiResponse { Description = "Not found" });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Internal server error" });

            //Description
            var roles = authAttributes.Aggregate(string.Empty, (current, authAttribute) => current + authAttribute.Roles);
            roles = roles.IsNullOrEmpty() ? "Any" : roles;

            operation.Description = $"<h3>Roles:</h3> {roles}";
        }
    }
}