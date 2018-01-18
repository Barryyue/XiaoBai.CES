using System.Security.Claims;
using System.Security.Principal;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace BarryCES.Web
{
    public partial class Startup
    {
        // 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login")
            });
        }
    }

    /// <summary>
    /// IIdentity扩展
    /// </summary>
    public static class IdentityExtention
    {
        /// <summary>
        /// 获取登录的用户ID
        /// </summary>
        /// <param name="identity">IIdentity</param>
        /// <returns></returns>
        public static string GetLoginUserId(this IIdentity identity)
        {
            if (identity != null)
            {
                var claim = (identity as ClaimsIdentity).FindFirst("LoginUserId");
                if (claim != null)
                    return claim.Value;
            }
            return string.Empty;
        }
        
    }
}