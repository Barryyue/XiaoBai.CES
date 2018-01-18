using System.Web.Mvc;
using log4net;

namespace BarryCES.Web
{
    /// <summary>
    /// MVC异常处理
    /// </summary>
    public class SiteExceptionAttribute : HandleErrorAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger("SystemError");

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            Log.Error("SiteExceptionHandle", filterContext.Exception);

            ProcessException(filterContext);

            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.ExceptionHandled = true;
        }

        /// <summary>
        /// 处理UCenter api异常
        /// </summary>
        /// <param name="exceptionContext"></param>
        private void ProcessException(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception;
            var isAjaxRequest = exceptionContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            var content = exception.Message;
            SetResult(exceptionContext, content, isAjaxRequest);
        }

        /// <summary>
        /// 设置错误消息返回数据
        /// </summary>
        /// <param name="exceptionContext"></param>
        /// <param name="message"></param>
        /// <param name="isAjaxRequest"></param>
        private void SetResult(ExceptionContext exceptionContext, string message, bool isAjaxRequest)
        {
            if (isAjaxRequest)
            {
                exceptionContext.Result = new JsonResult
                {
                    Data = new { flag = false, msg = message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var data = new ViewDataDictionary
                {
                    ["Message"] = message
                };
                exceptionContext.Result = new ViewResult
                {
                    ViewData = data,
                    ViewName = "~/Views/Home/Tips.cshtml"
                };
            }
        }
    }
}