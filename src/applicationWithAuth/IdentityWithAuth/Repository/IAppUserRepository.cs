// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-23 23:37
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-24 00:06
// ***********************************************************************
//  <copyright file="IAppUserRepository.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Threading.Tasks;
using AggregatedGenericResultMessage.Abstractions;
using IdentityWithAuth.Models;
using IdentityWithAuth.Models.DataStore;

#endregion

namespace IdentityWithAuth.Repository
{
    /// <summary>
    ///     Application repository
    /// </summary>
    public interface IAppUserRepository
    {
        /// <summary>
        ///     Get user list
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<IEnumerable<AppUser>>> GetUsersAsync();

        /// <summary>
        ///     Get user
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<PersonModel>> GetUserByNameAsync(string userName);
    }
}