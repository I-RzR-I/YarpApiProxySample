// ***********************************************************************
//  Assembly         : YarpApiProxySample.Domain.Common
//  Author           : RzR
//  Created On       : 2023-07-21 23:18
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="RefreshToken.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

#endregion

namespace Domain.Common.Identity
{
    /// <summary>
    ///     Refresh token model
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        ///     Refresh token id
        /// </summary>
        public Guid RefreshTokenId { get; set; }

        /// <summary>
        ///     Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Jwt id
        /// </summary>
        public string JwtId { get; set; }

        /// <summary>
        ///     Request id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        ///     Created date as UTC
        /// </summary>
        public DateTime CreationDateUtc { get; set; }

        /// <summary>
        ///     Expiration date as UTC
        /// </summary>
        public DateTime ExpiryDateUtc { get; set; }
        
        /// <summary>
        ///     Is token used
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        ///     Is token invalidated
        /// </summary>
        public bool Invalidated { get; set; }

        /// <summary>
        ///     Token user id
        /// </summary>
        public string UserId { get; set; }
    }
}