using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebHookAdpter.Core;

namespace UnityApi.Server
{
    public class ValidFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {

            var modelState = actionContext.ModelState;

            //if (!modelState.IsValid)
            //{
            //    List<string> errors = new List<string>();
            //    foreach (var key in modelState.Keys)
            //    {
            //        var state = modelState[key];
            //        if (state.Errors.Any())
            //        {
            //            errors.Add(state.Errors.First().ErrorMessage);
            //        }
            //    }

            //    StatusResult result = new StatusResult(errors);
            //    actionContext.Result = new HttpResponseMessage( System.Net.HttpStatusCode.OK)
            //    {
            //        Content = new StringContent(result.ToJson())
            //    };
            //}
        }


        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            //var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            //var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //if (actionExecutedContext.Exception != null)
            //{
            //    var result = new StatusResult<object>(actionExecutedContext.Exception.Message);
            //    actionExecutedContext.Result = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            //    {
            //        Content = ConvertToAesJsonContent(result)
            //    };
            //}
            //else
            //{
            //    var Content = actionExecutedContext.Result;
            //    var status = Content.ReadAsAsync<StatusResult<object>>().Result;
            //    var aesContent = status.ToAesJsonContent(ConfigManager.Provider.ApiServer.EncryptKey, ConfigManager.Provider.ApiServer.From);
            //    actionExecutedContext.Response.Content = aesContent;
            //}
        }


        HttpContent ConvertToAesJsonContent(StatusResult<object> input)
        {
            return null;
        }

    }
}
