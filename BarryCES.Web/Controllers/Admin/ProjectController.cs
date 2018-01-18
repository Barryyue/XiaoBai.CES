using System.Collections.Generic;
using System.Web.Mvc;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Filters;
using BarryCES.Web.Filters;
using BarryCES.Web.Models;

namespace BarryCES.Web.Controllers
{
    /// <summary>
    /// 项目管理
    /// </summary>
    [IgnoreRightFilter]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="projectService"></param>
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult Delete(IList<string> ids)
        {
            var result = new JsonResultModel<bool>();
            if (ids.AnyOne())
            {
                result.flag = _projectService.Delete(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetListWithPager(ProjectFilter filter)
        {
            var projects = _projectService.Search(filter);
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索部件页面
        /// </summary>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetParts(BaseFilter filter)
        {
            var parts = _projectService.SearchParts(filter);
            return Json(parts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索项目列表
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetProjectItems(ProjectItemFilter filter)
        {
            var projects = _projectService.SearchItems(filter);
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <returns></returns>
        [IgnoreRightFilter]
        [HttpPost]
        public JsonResult SaveProject(List<ProjectAddDto> projects)
        {
            var success = _projectService.Add(projects);
            var result = new JsonResultModel<bool>(success, string.Empty, success);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}