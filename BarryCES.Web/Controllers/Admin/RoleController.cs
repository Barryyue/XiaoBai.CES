using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    /// 用户角色
    /// </summary>
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;

        public RoleController(IRoleService roleSvc,IMenuService menuService)
        {
            _roleService = roleSvc;
            _menuService = menuService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View(new RoleDto());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var model = _roleService.Find(id);
            return View(model);
        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <returns></returns>
        public ActionResult Authen()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthenMenuDatas()
        {
            var list = _menuService.GetTrees();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthenRoleDatas()
        {
            var list = _roleService.GetTrees();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色下的菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthenRoleMenus(string id)
        {
            var list = _menuService.GetMenusByRoleId(id);
            var menuIds = list?.Select(item => item.Id);
            return Json(menuIds, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色下的菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetRoleMenus(List<RoleMenuDto> datas)
        {
            var result = new JsonResultModel<bool>();
            if (datas.AnyOne())
            {
                result.flag = _roleService.SetRoleMenus(datas);
            }
            else
            {
                result.msg = "请选择需要授权的菜单";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清空该角色下的所有权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClearRoleMenus(string id)
        {
            var result = new JsonResultModel<bool>
            {
                flag = _roleService.ClearRoleMenus(id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(RoleDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Add(dto);
                if (result.IsNotBlank())
                    return RedirectToAction("Index");
            }
            return View(dto);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(RoleDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Update(dto);
                if (result)
                    return RedirectToAction("Index");
            }
            return View(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult Delete(IEnumerable<string> ids)
        {
            var result = new JsonResultModel<bool>();
            if (ids.AnyOne())
            {
                result.flag = _roleService.Delete(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetListWithPager(RoleFilters filters)
        {
            var result = _roleService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}