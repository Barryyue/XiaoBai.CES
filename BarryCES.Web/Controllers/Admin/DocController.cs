using System.Collections.Generic;
using System.Web.Mvc;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Enum;
using BarryCES.Models.Filters;
using BarryCES.Web.Filters;
using BarryCES.Web.Models;
using Newtonsoft.Json;
using System;

namespace BarryCES.Web.Controllers.Admin
{
    /// <summary>
    /// 文章内容管理
    /// </summary>
    public class DocController : Controller
    {

        private readonly IDocService _docService;

        public DocController(IDocService docService)
        {
            _docService = docService;
        }

        // GET: Doc
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View(new DocDto());
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="advanceFilter">高级查询</param>
        /// <returns></returns>
        [HttpPost, IgnoreRightFilter]
        public JsonResult GetListWithPager(DocSearchDto model)
        {

            var result = _docService.Search(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}