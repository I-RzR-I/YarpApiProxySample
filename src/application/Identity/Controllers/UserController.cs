// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-19 00:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-19 09:15
// ***********************************************************************
//  <copyright file="UserController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Models;
using Bogus;
using Identity.Models;
using Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Identity.Controllers
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<PersonModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<MessageModel>), StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            var data = Enumerable.Range(1, 100)
                .Select(num => new PersonModel { UserId = num, UserName = new Faker().Person.FullName })
                .ToArray();

            return JsonResult(Result<IEnumerable<PersonModel>>.Success(data));
        }
    }
}