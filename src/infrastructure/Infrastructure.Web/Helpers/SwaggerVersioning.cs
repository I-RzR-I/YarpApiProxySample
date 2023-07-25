// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 08:43
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="SwaggerVersioning.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
// ReSharper disable IdentifierTypo

#endregion

namespace Infrastructure.Web.Helpers
{
    /// <summary>
    ///     Swagger version
    /// </summary>
    public static class SwaggerVersioning
    {
        /// <summary>
        ///     Provide a custom strategy for selecting actions.
        /// </summary>
        /// <returns>A lambda that returns true/false based on document name and ApiDescription</returns>
        public static bool DocInclusionPredicate(string version, ApiDescription apiDescription)
        {
            var values = apiDescription.RelativePath
                .Split('/')
                .Select(v => v.Replace("v{version}", version))
                .ToList();

            apiDescription.RelativePath = string.Join("/", values);

            var versionParameter = apiDescription.ParameterDescriptions
                .SingleOrDefault(p => p.Name == "version");

            if (versionParameter != null)
            {
                apiDescription.ParameterDescriptions.Remove(versionParameter);
            }
            else
            {
                if (values.Count < 2)
                    return true;
                var regex = new Regex(@"v\d+");
                var match = regex.Match(values[1]);
                if (!match.Success)
                    return true;
                values[1] = version;
                apiDescription.RelativePath = string.Join("/", values);
            }

            return true;
        }
    }
}