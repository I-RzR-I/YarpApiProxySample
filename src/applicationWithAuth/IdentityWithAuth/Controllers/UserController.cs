// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-21 08:00
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
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
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Models;
using IdentityWithAuth.Models;
using IdentityWithAuth.Repository;
using Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace IdentityWithAuth.Controllers
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        private readonly IAppUserRepository _userRepository;

        public UserController(IAppUserRepository userRepository)
            => _userRepository = userRepository;

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<PersonModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var data = (await _userRepository.GetUsersAsync()).Response
                .Select(num => new PersonModel { UserId = num.Id, UserName = num.UserName })
                .ToArray();

            return JsonResult(Result<IEnumerable<PersonModel>>.Success(data));
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(PersonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<MessageModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByName([FromQuery] string userName)
        {
            var user = (await _userRepository.GetUserByNameAsync(userName));

            return JsonResult(user);
        }
    }
}