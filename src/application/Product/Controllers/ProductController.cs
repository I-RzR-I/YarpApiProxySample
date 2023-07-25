// ***********************************************************************
//  Assembly         : YarpApiProxySample.Product
//  Author           : RzR
//  Created On       : 2023-07-19 00:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-19 09:15
// ***********************************************************************
//  <copyright file="ProductController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using Bogus;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Product.Models;

#endregion

namespace Product.Controllers
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IEnumerable<ProductModel> Get()
            => Enumerable.Range(1, 100)
                .Select(num => new ProductModel { Id = num, Name = new Faker().Commerce.ProductName() })
                .ToArray();
    }
}