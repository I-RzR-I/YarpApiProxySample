// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-23 21:51
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-23 21:51
// ***********************************************************************
//  <copyright file="AppUser.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.AspNetCore.Identity;

namespace IdentityWithAuth.Models.DataStore
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}