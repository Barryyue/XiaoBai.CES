using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Filters;
using BarryCES.Web.Filters;
using BarryCES.Web.Models;

namespace BarryCES.Web.Controllers
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Logins()
        {
            return View();
        }

        /// <summary>
        /// 访问记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Visits()
        {
            return View();
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult LoginsList(LogFilters filters)
        {
            var result = _logService.SearchLoginLogs(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 访问记录
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult VisitsList(LogFilters filters)
        {
            var result = _logService.SearchVisitLogs(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 图表
        /// </summary>
        /// <returns></returns>
        public ActionResult Charts()
        {
            return View();
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ChartsDatas()
        {
            var result = new JsonResultModel<IEnumerable<VisitDataDto>>
            {
                flag = true,
                data = _logService.GetLastestVisitData()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}