// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-19 07:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="BaseApiController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Infrastructure.Web
{
    /// <inheritdoc />
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "OwnJwtAuth")]
    [Produces("application/json")]
    public abstract class BaseApiController : ResultBaseApiController
    {
    }
}