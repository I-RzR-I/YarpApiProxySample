// ***********************************************************************
//  Assembly         : YarpApiProxySample.Domain.Common
//  Author           : RzR
//  Created On       : 2023-07-21 14:23
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="TokenRequest.cs" company="">
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
    ///     Token request
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        ///     User name
        /// </summary>
        public string username { get; set; }

        /// <summary>
        ///     User password
        /// </summary>
        public string password { get; set; }

        /// <summary>
        ///     Auth client id
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        ///     Auth client secret
        /// </summary>
        public string client_secret { get; set; }

        /// <summary>
        ///     Auth grant type
        /// </summary>
        public string grant_type { get; set; }

        /// <summary>
        ///     Auth scope
        /// </summary>
        public string scope { get; set; }
    }
}