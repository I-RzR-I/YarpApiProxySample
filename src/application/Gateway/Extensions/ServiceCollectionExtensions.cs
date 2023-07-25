// ***********************************************************************
//  Assembly         : YarpApiProxySample.Gateway
//  Author           : RzR
//  Created On       : 2023-07-19 08:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-19 23:37
// ***********************************************************************
//  <copyright file="ServiceCollectionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Yarp.ReverseProxy.Configuration;

#endregion

namespace Gateway.Extensions
{
    /// <summary>
    ///     Service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register swagger endpoints
        /// </summary>
        /// <param name="service">Service collection</param>
        /// <param name="configuration">App configuration</param>
        /// <param name="appBuilder">App builder</param>
        /// <remarks></remarks>
        public static void RegisterSwaggerEndpoints(this SwaggerUIOptions service, IConfiguration configuration, IApplicationBuilder appBuilder)
        {
            var proxyConfig = appBuilder.ApplicationServices.GetRequiredService<IProxyConfigProvider>();
            foreach (var config in proxyConfig.GetConfig().Routes)
            {
                var name = configuration[$"ReverseProxy:Routes:{config.RouteId}:Name"];
                var version = configuration[$"ReverseProxy:Routes:{config.RouteId}:Version"];
                var endpointRoute = config.Match.Path?.Split('/')[1];

                service.SwaggerEndpoint($"/{endpointRoute}/swagger/{version}/swagger.json", name);
            }
        }

        /// <summary>
        ///     Register swagger endpoints by api url
        /// </summary>
        /// <param name="service">Service collection</param>
        /// <param name="configuration">App configuration</param>
        /// <param name="appBuilder">App builder</param>
        /// <remarks></remarks>
        public static void RegisterSwaggerEndpointsByApiUrl(
            this SwaggerUIOptions service, IConfiguration configuration,
            IApplicationBuilder appBuilder)
        {
            var proxyConfig = appBuilder.ApplicationServices
                .GetRequiredService<IProxyConfigProvider>()
                .GetConfig();

            foreach (var cluster in proxyConfig.Clusters)
            {
                var routes = proxyConfig.Routes
                    .Where(x => x.ClusterId == cluster.ClusterId).ToList();

                foreach (var destination in cluster.Destinations!)
                    foreach (var route in routes)
                    {
                        var name = configuration[$"ReverseProxy:Routes:{route.RouteId}:Name"];
                        var version = configuration[$"ReverseProxy:Routes:{route.RouteId}:Version"];
                        var destinationUrl = destination.Value.Address.EndsWith("/")
                            ? destination.Value.Address
                            : $"{destination.Value.Address}/";

                        service.SwaggerEndpoint($"{destinationUrl}swagger/{version}/swagger.json", name);
                    }
            }
        }

        /// <summary>
        ///     Register swagger servers
        /// </summary>
        /// <param name="swaggerGenOptions">Swagger option</param>
        /// <param name="serviceCollection">Service collection</param>
        /// <remarks></remarks>
        public static void RegisterSwaggerServers(this SwaggerGenOptions swaggerGenOptions, IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var proxyConfig = serviceProvider.GetRequiredService<IProxyConfigProvider>();
            foreach (var cluster in proxyConfig.GetConfig().Clusters)
                foreach (var destination in cluster.Destinations!)
                    swaggerGenOptions.AddServer(new OpenApiServer { Url = destination.Value.Address });
        }
    }
}