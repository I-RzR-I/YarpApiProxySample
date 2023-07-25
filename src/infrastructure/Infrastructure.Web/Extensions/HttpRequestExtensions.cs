// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-19 23:09
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="HttpRequestExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

#endregion

namespace Infrastructure.Web.Extensions
{
    /// <summary>
    ///     HTTP request extension
    /// </summary>
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        /// <summary>
        ///     Check if is ajax request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }

        /// <summary>
        ///     Is api request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsApiRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var path = request.Path;
            return path.ToString().ToLowerInvariant().StartsWith("/api");
        }

        /// <summary>
        ///     Get token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static (string scheme, string token) GetToken(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var accessToken = request.Headers[HeaderNames.Authorization].ToString();
            var arr = accessToken.Split(" ");
            return (arr[0], arr[1]);
        }
    }
}