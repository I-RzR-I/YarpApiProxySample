// ***********************************************************************
//  Assembly         : YarpApiProxySample.Product
//  Author           : RzR
//  Created On       : 2023-07-21 08:00
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
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
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Models;
using Bogus;
using Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductWithAuth.Models;

#endregion

namespace ProductWithAuth.Controllers
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<ProductModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var data = Enumerable.Range(1, 100)
                .Select(num => new ProductModel { Id = num, Name = new Faker().Commerce.ProductName() })
                .ToArray();

            return JsonResult(await Task.FromResult(Result<IEnumerable<ProductModel>>.Success(data)));
        }
    }
}