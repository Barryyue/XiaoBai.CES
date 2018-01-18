using System.Web.Mvc;
using BarryCES.Interfaces;
using BarryCES.Web.Models;

namespace BarryCES.Web.Controllers
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public class SystemController : Controller
    {
        private readonly IDatabaseInitService _databaseInitService;

        public SystemController(IDatabaseInitService databaseInitService)
        {
            _databaseInitService = databaseInitService;
        }

        /// <summary>
        /// 系统管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 重置路径码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReloadPathCode()
        {
            var result = new JsonResultModel<bool>
            {
                flag = _databaseInitService.InitPathCode()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}