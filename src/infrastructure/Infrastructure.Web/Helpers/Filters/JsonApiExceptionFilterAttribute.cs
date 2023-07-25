// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-19 23:15
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="JsonApiExceptionFilterAttribute.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AggregatedGenericResultMessage.Abstractions.Models;
using AggregatedGenericResultMessage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Infrastructure.Web.Helpers.Filters
{
    /// <summary>
    ///     JSON API exception filter
    /// </summary>
    public class JsonApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        ///     On api exception occurred
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JsonApiExceptionFilterAttribute>>();
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var exception = context.Exception;

            var errors = !string.IsNullOrEmpty(exception.Message)
                ? new List<IMessageModel> { new MessageModel("API_UnhandledException", exception.Message) }
                : new List<IMessageModel> { new MessageModel("API_UnhandledException", "An unhandled exception has occurred.") };

            var serializeObject = JsonSerializer.Serialize(errors);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
            await context.HttpContext.Response.WriteAsync(serializeObject);
        }
    }
}