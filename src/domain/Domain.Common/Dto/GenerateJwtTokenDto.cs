// ***********************************************************************
//  Assembly         : YarpApiProxySample.Domain.Common
//  Author           : RzR
//  Created On       : 2023-07-21 23:20
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="GenerateJwtTokenDto.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using Domain.Common.Identity;
// ReSharper disable UnusedAutoPropertyAccessor.Global

#endregion

namespace Domain.Common.Dto
{
    /// <summary>
    ///     Generate new auth token request data
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GenerateJwtTokenDto
    {
        /// <summary>
        ///     User auth data
        /// </summary>
        public TokenRequest UserTokenRequest { get; set; }

        /// <summary>
        ///     User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     USer name
        /// </summary>
        public string UniqueName { get; set; }

        /// <summary>
        ///     User email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        ///     Additional user claims
        /// </summary>
        public IEnumerable<(string claimType, string claimValue)> CustomClaims { get; set; } = null;

        /// <summary>
        /// Auth request id
        /// </summary>
        public string RequestId { get; set; } = null;
    }
}