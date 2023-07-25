// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-24 12:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-24 13:59
// ***********************************************************************
//  <copyright file="UserSeed.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading.Tasks;
using IdentityWithAuth.Models.DataStore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace IdentityWithAuth.Data.Seed
{
    public static class UserSeed
    {
        public static async Task SeedIdentityAsync(this IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            for (var i = 0; i < 5; i++)
            {
                var user = new AppUser
                {
                    UserName = $"UserTemp{i}",
                    Id = Guid.NewGuid().ToString(),
                    Email = $"User{i}@temp.com",
                    Password = "Us3r1@73m7",
                    Name = $"User Temp{i}"
                };
                userManager.CreateAsync(user, user.Password).GetAwaiter().GetResult();
            }

            await Task.CompletedTask;
        }
    }
}