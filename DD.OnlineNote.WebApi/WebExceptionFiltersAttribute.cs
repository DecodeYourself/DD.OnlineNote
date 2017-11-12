using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DD.OnlineNote.WebApi
{
    public class WebExceptionFiltersAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                Logger.Log.Instance.Error("Ошибка: {0}", context.Exception.Message);
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            if (context.Exception is Exception)
            {
                Logger.Log.Instance.Error("Какая то дичь: {0}", context.Exception.Message);
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}