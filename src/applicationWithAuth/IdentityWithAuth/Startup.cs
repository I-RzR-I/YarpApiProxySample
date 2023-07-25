// ***********************************************************************
//  Assembly         : YarpApiProxySample.Identity
//  Author           : RzR
//  Created On       : 2023-07-21 08:00
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="Startup.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Common.Dto;
using IdentityWithAuth.Data;
using IdentityWithAuth.Models.DataStore;
using IdentityWithAuth.Repository;
using Infrastructure.Common;
using Infrastructure.Identity.Abstractions.Services;
using Infrastructure.Identity.Services;
using Infrastructure.Web.Helpers;
using Infrastructure.Web.Helpers.Filters;
using Infrastructure.Web.ServiceCollection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniqueServiceCollection;

#endregion

namespace IdentityWithAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SystemApplication.SetExecutingAssembly(Assembly.GetAssembly(GetType()));
            SystemApplication.SetAllowedInternalRequestApis(configuration.GetValue<string>("ConnectedApis").Split(','));
            SystemApplication.SetJwtSettings(Configuration.GetSection("JWTSettings").Get<JWTSettings>());
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsModule(Configuration);

            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseInMemoryDatabase("AppDb");
            });

            //  Auth token service
            services.AddScoped<ITokenService, JwtTokenService>();

            services.AddIdentity<AppUser, IdentityRole>(config =>
                {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //
            services.AddUnique<IAppUserRepository, AppUserRepository>(ServiceLifetime.Scoped);
            //
            services.RegisterAuthorizationEndpoint();

            services.AddControllers(options =>
                {
                    options.Filters.Add<ApiValidationActionFilterAttribute>();
                    options.Filters.Add<JsonApiExceptionFilterAttribute>();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                }).AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerModule(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorsModule();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerModule("Identity v1");
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/",
                    async context =>
                    {
                        await new TextHomePageResolver().ResolveAsync(context, SystemApplication.GetAppInformation().AppName);
                    });
            });
        }
    }
}