using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Enum;
using BarryCES.Web.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace BarryCES.Web.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [IgnoreRightFilter]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;

        public HomeController(IUserService userSvr,IMenuService menuService)
        {
            _userService = userSvr;
            _menuService = menuService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var myMenus = _menuService.GetMyMenus(User.Identity.GetLoginUserId());
            return View(myMenus);
        }

        /// <summary>
        /// 欢迎页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            var model = new LoginDto
            {
                ReturnUrl = Request.QueryString["ReturnUrl"],
                LoginName = "Dscoop",
                Password = "qwaszx.."
            };
            return View(model);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginDto model)
        {
            if (!ModelState.IsValid) return View(model);
            model.LoginIP = HttpContext.Request.UserHostAddress;
            var loginDto = _userService.Login(model);
            if (loginDto.LoginSuccess)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var claims = new List<Claim>
                {
                    new Claim("LoginUserId", loginDto.User.Id.ToString()),
                    new Claim(ClaimTypes.Name, model.LoginName)
                };
                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var pro = new AuthenticationProperties()
                {
                    IsPersistent = true
                };
                authenticationManager.SignIn(pro, identity);
                if (model.ReturnUrl.IsBlank())
                    return RedirectToAction("Index");
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError(loginDto.Result == LoginResult.AccountNotExists ? "LoginName" : "Password",
                loginDto.Message);
            return View(model);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}