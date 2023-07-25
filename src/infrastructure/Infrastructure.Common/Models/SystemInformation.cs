// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Common
//  Author           : RzR
//  Created On       : 2023-07-21 08:32
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="SystemInformation.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Infrastructure.Common.Models
{
    /// <summary>
    ///     App system information model
    /// </summary>
    public class SystemInformation
    {
        /// <summary>
        ///     Application version
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        ///     Application name
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        ///     company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        ///     Copyright
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        ///     Product name
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        ///     Trade mark
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        ///     Term of service
        /// </summary>
        public string TermsOfService { get; set; }

        /// <summary>
        ///     Company contact
        /// </summary>
        public SystemContact Contact { get; set; }
    }

    /// <summary>
    ///     Company contact
    /// </summary>
    public class SystemContact
    {
        /// <summary>
        ///     Contact email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Contact URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Contact name
        /// </summary>
        public string Name { get; set; }
    }
}