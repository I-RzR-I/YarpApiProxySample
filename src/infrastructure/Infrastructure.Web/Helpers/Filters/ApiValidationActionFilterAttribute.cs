// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-19 23:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="ApiValidationActionFilterAttribute.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Net;
using AggregatedGenericResultMessage;
using Infrastructure.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Infrastructure.Web.Helpers.Filters
{
    /// <summary>
    ///     Validate model state and return errors if an error occurred
    /// </summary>
    public class ApiValidationActionFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid && (context.HttpContext.Request.IsAjaxRequest() 
                                                || context.HttpContext.Request.IsApiRequest()))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new Result().AttachModelState(context.ModelState).Messages);
            }

            base.OnActionExecuting(context);
        }
    }
}