// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-25 00:44
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-25 00:47
// ***********************************************************************
//  <copyright file="TextHomePageResolver.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Threading.Tasks;
using Infrastructure.Web.Abstractions;
using Microsoft.AspNetCore.Http;

#endregion

namespace Infrastructure.Web.Helpers
{
    /// <inheritdoc />
    public class TextHomePageResolver : IHomePageResolver
    {
        /// <inheritdoc />
        public async Task ResolveAsync(HttpContext context, string applicationName) 
            => await context.Response.WriteAsync($"{applicationName} is up and running");
    }
}