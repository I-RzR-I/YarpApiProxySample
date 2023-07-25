// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-21 08:44
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:27
// ***********************************************************************
//  <copyright file="EnumSchemaFilter.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Infrastructure.Web.Helpers.Filters
{
    /// <summary>
    ///     Enum schema filter
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;
            model.Type = "string";
            model.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(n => model.Enum.Add(new OpenApiString(n.FirstCharToLower())));
        }
    }
}