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
using System.Linq;
using System.Text;
using BarryCES.Infrastructure;

namespace BarryCES.Web.Controllers
{
    /// <summary>
    /// 板块配置
    /// </summary>
    public class ModuleSetController : Controller
    {

        private readonly IModuleService _moduleService;

        public ModuleSetController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            var model = _moduleService.Find(id);
            return View(model);
        }

        public ActionResult Add()
        {
            return View(new ModuleDto());
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetListWithKeywords(MenuFilters filters)
        {
            filters.page = 1;
            filters.rows = 10;
            var result = _moduleService.Search(filters);
            return Json(new { value = result.rows }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ModuleDto dto)
        {
            if (ModelState.IsValid)
            {
                dto.EditDateTime = DateTime.Now;
                dto.EditUserId = User.Identity.GetLoginUserId();
                var result = _moduleService.Update(dto);
                if (result)
                    return RedirectToAction("Index");
            }
            return View(dto);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="advanceFilter">高级查询</param>
        /// <returns></returns>
        [HttpPost, IgnoreRightFilter]
        public ActionResult GetListWithPager()
        {
            var data = _moduleService.GetList();
            foreach (ModuleDto item in data)
            {
                if (item.ParentId!=null)
                {
                    item.ParentName = data.Where(c => c.Id == item.ParentId).FirstOrDefault().ModuleName;
                }
                else
                {
                    item.ParentName = item.ModuleName;
                }
            }
            return Json(data);
        }

        
    }


    
}