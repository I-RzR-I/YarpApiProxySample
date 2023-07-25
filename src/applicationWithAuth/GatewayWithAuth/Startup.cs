// ***********************************************************************
//  Assembly         : YarpApiProxySample.Gateway
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

using System.Reflection;
using GatewayWithAuth.Extensions;
using Infrastructure.Common;
using Infrastructure.Web.Helpers;
using Infrastructure.Web.Helpers.Filters;
using Infrastructure.Web.ServiceCollection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

namespace GatewayWithAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SystemApplication.SetExecutingAssembly(Assembly.GetAssembly(GetType()));
            SystemApplication.SetAllowedInternalRequestApis(configuration.GetValue<string>("ConnectedApis").Split(','));
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsModule(Configuration);

            services.AddHttpContextAccessor();
            
            services.RegisterAuthorizationEndpoint();
            
            services.AddControllers(options =>
                {
                    options.Filters.Add<ApiValidationActionFilterAttribute>();
                    options.Filters.Add<JsonApiExceptionFilterAttribute>();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                }).AddXmlSerializerFormatters();

            services.AddSwaggerModule(Configuration);

            services.AddReverseProxy()
                .LoadFromConfig(Configuration.GetSection("ReverseProxy"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorsModule();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway v1");
                    c.RegisterSwaggerEndpointsByApiUrl(Configuration, app);
                    c.DisplayRequestDuration();
                    c.ConfigObject.DocExpansion = DocExpansion.None;
                    c.ConfigObject.DisplayRequestDuration = true;
                    c.ConfigObject.Filter = string.Empty;
                });
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
                endpoints.MapReverseProxy();
            });
        }
    }
}