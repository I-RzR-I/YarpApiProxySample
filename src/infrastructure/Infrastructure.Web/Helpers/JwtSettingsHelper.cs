// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-24 20:46
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-24 20:46
// ***********************************************************************
//  <copyright file="JwtSettingsHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using Domain.Common.Dto;
using DomainCommonExtensions.DataTypeExtensions;
using Newtonsoft.Json;

namespace Infrastructure.Web.Helpers
{
    /// <summary>
    ///     JWT settings helper
    /// </summary>
    public static class JwtSettingsHelper
    {
        /// <summary>
        ///     Get JWT settings
        /// </summary>
        /// <returns></returns>
        public  static IResult<JWTSettings> GetJwtSettings(string identityUrl)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create($"{identityUrl}/api/v1/connect/getTokenSettings");
                request.Method = "GET";
                request.Headers.Add("IsAllowedSafeRequest", "true");
                using var response = (HttpWebResponse)request.GetResponse();
                var dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                var tokenResult = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var obj = JsonConvert.DeserializeObject<JWTSettings>(tokenResult);

                    return Result<JWTSettings>.Success(obj);
                }
                else
                {
                    return Result<JWTSettings>.Failure("No JWT settings found");
                }
            }
            catch (Exception e)
            {
                return Result<JWTSettings>
                    .Failure("Internal error on get JWT settings")
                    .AddError(e);
            }
        }
    }
}