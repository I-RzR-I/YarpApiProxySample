// ***********************************************************************
//  Assembly         : YarpApiProxySample.Infrastructure.Web
//  Author           : RzR
//  Created On       : 2023-07-19 23:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-07-21 23:26
// ***********************************************************************
//  <copyright file="ModelStateDictionaryExtensions.cs" company="">
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
using AggregatedGenericResultMessage.Abstractions.Models;
using AggregatedGenericResultMessage.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

#endregion

namespace Infrastructure.Web.Extensions
{
    /// <summary>
    ///     Model state extensions
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        ///     Append errors to Model State
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ModelStateDictionary AppendResultModelErrors(this ModelStateDictionary modelState, ICollection<IMessageModel> errors)
        {
            foreach (var error in errors) modelState.AddModelError(error.Key, error.Message);
            return modelState;
        }

        /// <summary>
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<IMessageModel> ToResultModelErrors(this ModelStateDictionary modelState)
        {
            foreach (var stateError in modelState.Values)
            foreach (var error in stateError.Errors)
                yield return new MessageModel(string.Empty, error.ErrorMessage);
        }

        /// <summary>
        ///     Attach model state
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static Result<T> AttachModelState<T>(this Result<T> self, ModelStateDictionary modelState)
        {
            self ??= new Result<T>();
            self.Messages = modelState.ToResultModelErrors().ToList();
            return self;
        }
    }
}