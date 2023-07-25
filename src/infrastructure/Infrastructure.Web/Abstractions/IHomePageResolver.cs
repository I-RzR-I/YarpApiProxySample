// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-25 00:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-25 00:47
// ***********************************************************************
//  <copyright file="IHomePageResolver.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

#endregion

namespace Infrastructure.Web.Abstractions
{
    /// <summary>
    ///     Home page resolver
    /// </summary>
    /// <remarks></remarks>
    public interface IHomePageResolver
    {
        /// <summary>
        ///     Resolve / request by writing in response body
        /// </summary>
        /// <param name="context"></param>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        Task ResolveAsync(HttpContext context, string applicationName);
    }
}