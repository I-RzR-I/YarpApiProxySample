// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-23 23:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-24 00:06
// ***********************************************************************
//  <copyright file="AppUserRepository.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using DomainCommonExtensions.CommonExtensions;
using IdentityWithAuth.Data;
using IdentityWithAuth.Models;
using IdentityWithAuth.Models.DataStore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace IdentityWithAuth.Repository
{
    /// <inheritdoc cref="IAppUserRepository" />
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AppUserRepository(ApplicationDbContext db, UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;

            //AddUsersAsync();
        }

        /// <inheritdoc />
        public async Task<IResult<IEnumerable<AppUser>>> GetUsersAsync()
        {
            var users = await _db.Set<AppUser>().ToListAsync();

            return Result<IEnumerable<AppUser>>.Success(users);
        }

        /// <inheritdoc />
        public async Task<IResult<PersonModel>> GetUserByNameAsync(string userName)
        {
            var user = await _db.Set<AppUser>()
                .Select(x => new PersonModel()
                {
                    UserName = x.UserName,
                    UserId = x.Id
                })
                .FirstOrDefaultAsync(x => x.UserName == userName);

            return user.IsNull()
                ? Result<PersonModel>.NotFound($"User not found {userName}")
                : Result<PersonModel>.Success(user);
        }

        /// <summary>
        ///     Add users in store
        /// </summary>
        /// <remarks></remarks>
        private void AddUsersAsync()
        {
            for (var i = 0; i < 10; i++)
            {
                var user = new AppUser
                {
                    UserName = $"UserTemp{i}",
                    Id = Guid.NewGuid().ToString(),
                    Email = $"User{i}@temp.com",
                    Password = "Us3r1@73m7",
                    Name = $"User Temp{i}"
                };
                _ = _userManager.CreateAsync(user, user.Password).GetAwaiter().GetResult();
            }
        }
    }
}