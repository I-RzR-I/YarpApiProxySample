// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Common
//  Author           : RzR
//  Created On       : 2023-07-21 08:34
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="SystemApplication.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Common.Dto;
using Infrastructure.Common.Models;

#endregion

namespace Infrastructure.Common
{
    /// <summary>
    ///     System application parameters
    /// </summary>
    /// <remarks></remarks>
    public static class SystemApplication
    {
        private static SystemInformation _systemInformation;

        /// <summary>
        ///     Executing assembly
        /// </summary>
        public static Assembly ExecutingAssembly { get; private set; }

        /// <summary>
        ///     Allowed API call for internal services
        /// </summary>
        public static IEnumerable<string> AllowedInternalRequestApis { get; private set; }

        /// <summary>
        ///     Auth JWT settings
        /// </summary>
        public static JWTSettings JwtSettings { get; private set; }

        /// <summary>
        ///     Set current executing assembly
        /// </summary>
        /// <param name="assembly">Current assembly</param>
        public static void SetExecutingAssembly(Assembly assembly)
        {
            if (ExecutingAssembly != null)
                return;
            ExecutingAssembly = assembly;
        }

        /// <summary>
        ///     Set allowed apis for internal request
        /// </summary>
        /// <param name="apiList"></param>
        public static void SetAllowedInternalRequestApis(IEnumerable<string> apiList) 
            => AllowedInternalRequestApis = apiList;

        /// <summary>
        ///     Set allowed apis for internal request
        /// </summary>
        /// <param name="settings">JWT settings</param>
        public static void SetJwtSettings(JWTSettings settings) 
            => JwtSettings = settings;

        /// <summary>
        ///     Get app info
        /// </summary>
        /// <returns></returns>
        public static SystemInformation GetAppInformation()
        {
            if (_systemInformation != null)
                return _systemInformation;
            var assembly = Assembly.GetExecutingAssembly();
            var metadata = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToList();

            return _systemInformation = new SystemInformation
            {
                AppVersion = ExecutingAssembly?.GetName().Version.ToString(),
                AppName = ExecutingAssembly?.GetName().Name,
                TermsOfService = metadata.SingleOrDefault(x => x.Key.Equals("TermsOfService"))?.Value,
                Contact = new SystemContact
                {
                    Name = metadata.SingleOrDefault(x => x.Key.Equals("ContactName"))?.Value,
                    Url = metadata.SingleOrDefault(x => x.Key.Equals("ContactUrl"))?.Value,
                    Email = metadata.SingleOrDefault(x => x.Key.Equals("ContactEmail"))?.Value
                },
                Company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company,
                Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright,
                Trademark = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark,
                Product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product
            };
        }
    }
}