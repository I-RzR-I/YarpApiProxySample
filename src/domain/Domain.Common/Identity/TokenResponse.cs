// ***********************************************************************
//  Assembly         : YarpApiProxySample.Domain.Common
//  Author           : RzR
//  Created On       : 2023-07-21 14:21
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="TokenResponse.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#pragma warning disable IDE1006 // Naming Styles

namespace Domain.Common.Identity
{
    /// <summary>
    ///     Auth token response
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        ///     Token identifier
        /// </summary>
        public string id_token { get; set; }

        /// <summary>
        ///     User access token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        ///     Token expires time
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        ///     User refresh token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        ///     Token type
        /// </summary>
        public string token_type { get; set; } = "Bearer";

        /// <summary>
        ///     Token scope
        /// </summary>
        public string scope { get; set; }
    }
}