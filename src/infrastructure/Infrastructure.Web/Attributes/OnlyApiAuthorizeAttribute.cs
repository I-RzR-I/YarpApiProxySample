// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-24 18:51
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-25 10:09
// ***********************************************************************
//  <copyright file="OnlyApiAuthorizeAttribute.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace Infrastructure.Web.Attributes
{
    /// <summary>
    ///     Authorize request only for api
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OnlyApiAuthorizeAttribute : TypeFilterAttribute
    {
        /// <inheritdoc />
        public OnlyApiAuthorizeAttribute() : base(typeof(OnlyApiAuthorizeAttributeExecutor))
        {
        }

        /// <summary>
        ///     Allow only API request attribute
        /// </summary>
        private class OnlyApiAuthorizeAttributeExecutor : Attribute, IAsyncResourceFilter
        {
            /// <inheritdoc />
            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                var currentApi = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}";
                var originUrl = context.HttpContext.Request.Headers["Origin"].ToString();
                var referUrl = context.HttpContext.Request.Headers["Referer"].ToString();
                var allowedApis = SystemApplication.AllowedInternalRequestApis;
                var allowedHeader = context.HttpContext.Request.Headers["IsAllowedSafeRequest"].ToString();

                if ((!originUrl.IsNullOrEmpty() && allowedApis.AnyStartWith(originUrl))
                    || (!referUrl.IsNullOrEmpty() && allowedApis.AnyStartWith(referUrl))
                    || allowedHeader == "true"
                    || new List<string>() { "localhost", "127.0.0.1" }.Any(x => x == context.HttpContext.Request.Host.Host))
                {
                    await next();
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 401;
                    //context.HttpContext.Response.Redirect("/");
                }
            }
        }
    }
}