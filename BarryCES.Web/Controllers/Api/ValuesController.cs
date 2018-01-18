using BarryCES.Web.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace BarryCES.Web.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet, Route("values/login")]
        public IHttpActionResult Login()
        {

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;//.GetOwinContext().Authentication;
            var claims = new List<Claim>
                {
                    new Claim("LoginUserId", "adf"),
                    new Claim(ClaimTypes.Name, "abc")
                };
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            var pro = new AuthenticationProperties()
            {
                IsPersistent = true
            };
            authenticationManager.SignIn(pro, identity);
            return Json(new { });
        }
        [HttpPost, Route("values/getInfo")]
        public IHttpActionResult GetInfo()
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;//.GetOwinContext().Authentication;

            return Json(new
            {
                ss = authenticationManager.User.Identity.Name
            });
        }
        [HttpPost, Route("api/values/loginout")]
        public IHttpActionResult loginOut()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return Json(new { msg = "success" });
        }

        [Authorize, HttpGet, Route("api/values/getstr")]
        public string getStr()
        {
            return DateTime.Now.ToString();
        }

    }
}
