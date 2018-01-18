using System;
using System.Web.Mvc;
using BarryCES.Interfaces;
using BarryCES.Models;
using log4net;

namespace BarryCES.Web.Filters
{
    /// <summary>
    /// 访问记录
    /// </summary>
    public class VisitFilter : FilterAttribute, IActionFilter
    {
        private static readonly ILog Log = LogManager.GetLogger("SystemError");

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region 记录访问记录

            try
            {
                var userService = DependencyResolver.Current.GetService<IUserService>();
                var context = filterContext.HttpContext;
                var user = context.User;
                var isLogined = user != null && user.Identity != null && user.Identity.IsAuthenticated;
                var visit = new VisitDto
                {
                    IP = filterContext.HttpContext.Request.UserHostAddress,
                    LoginName = isLogined ? user.Identity.Name : string.Empty,
                    Url = context.Request.Url.PathAndQuery,
                    UserId = isLogined ? user.Identity.GetLoginUserId().ToString() : "0"
                };
                userService.Visit(visit);
            }
            catch(Exception ex)
            {
                Log.Error("SiteVisitFilterError", ex);
            }

            #endregion
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //todo
        }
    }
}