using BarryCES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BarryCES.Web.Filters
{
    public class LoginAuthorize : AuthorizeAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpContext.Current.Response.Write("OnActionExecuting:正要准备执行Action的时候但还未执行时执行<br />");    
            var isIgnored = filterContext.ActionDescriptor.IsDefined(typeof(IgnoreRightFilter), true) ||
                 filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(IgnoreRightFilter), true);
            if (!isIgnored)
            {
                var userService = DependencyResolver.Current.GetService<IUserService>();
                var context = filterContext.HttpContext;
                var identity = context.User.Identity;
                var routeData = filterContext.RouteData.Values;
                var controller = routeData["controller"];
                var action = routeData["action"];
                var url = string.Format("/{0}/{1}", controller, action);
                var hasRight = userService.HasRight(identity.GetLoginUserId(), url);

                if (hasRight) return;
                if (context.Request.IsAjaxRequest())
                {
                    var data = new
                    {
                        flag = false,
                        code = (int)HttpStatusCode.Unauthorized,
                        msg = "您没有权限！"
                    };
                    filterContext.Result = new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var view = new ViewResult
                    {
                        ViewName = "~/Views/Shared/NoRight.cshtml",
                    };
                    filterContext.Result = view;
                }
            }
        }
    }
}