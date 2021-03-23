using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform.Filter
{
    internal sealed class LogRequestFilter : ActionFilterAttribute
    {
        private readonly ILoggerProvider logger;

        public LogRequestFilter()
        {
            logger = new NLogger("LogRequestFilter");
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var formData = JsonConvert.SerializeObject(filterContext.ActionParameters);

            this.logger.Info(string.Format("OnActionExcuting：{0} ", formData));
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = string.Empty;
            if (filterContext.Result is JsonResult)
            {
                result = JsonConvert.SerializeObject((filterContext.Result as JsonResult).Data);
            }

            if (filterContext.Exception != null)
            {
                result = filterContext.Exception.Message;
                this.logger.Debug(string.Format("OnActionExcuted Exception StackTrace：{0} ", filterContext.Exception.StackTrace));
            }

            this.logger.Info(string.Format("OnActionExcuted：{0} ", result));
        }
    }
}