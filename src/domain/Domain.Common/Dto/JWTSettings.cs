// ***********************************************************************
//  Assembly         : YarpApiProxySample.Domain.Common
//  Author           : RzR
//  Created On       : 2023-07-24 18:40
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-24 18:40
// ***********************************************************************
//  <copyright file="JwtSettingsDto.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace Domain.Common.Dto
{
    /// <summary>
    ///     JWT settings DTO
    /// </summary>
    public class JWTSettings
    {
        /// <summary>
        ///     Security key
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        ///     Token issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        ///     Token audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        ///     Token duration in minutes
        /// </summary>
        public int DurationInMinutes { get; set; }

        /// <summary>
        ///     Refresh token duration in days
        /// </summary>
        public int RefreshTokenDurationInDays { get; set; }
    }
}