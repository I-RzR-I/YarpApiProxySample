// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Identity
//  Author           : RzR
//  Created On       : 2023-07-21 22:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="ITokenService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Threading;
using System.Threading.Tasks;
using AggregatedGenericResultMessage.Abstractions;
using Domain.Common.Dto;
using Domain.Common.Identity;

#endregion

namespace Infrastructure.Identity.Abstractions.Services
{
    /// <summary>
    ///     Jwt token service
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        ///     Generate user auth token
        /// </summary>
        /// <param name="requestData">Required. </param>
        /// <param name="cancellationToken">Optional. The default value is default.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<TokenResponse>> GenerateJwtTokenAsync(
            GenerateJwtTokenDto requestData,
            CancellationToken cancellationToken = default);
    }
}