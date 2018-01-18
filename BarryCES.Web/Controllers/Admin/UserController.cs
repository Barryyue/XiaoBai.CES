using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Filters;
using BarryCES.Web.Filters;
using BarryCES.Web.Models;

namespace BarryCES.Web.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserController(IUserService userSvc, IMapper mapper, IRoleService roleSvc)
        {
            _userService = userSvc;
            _mapper = mapper;
            _roleService = roleSvc;
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
        /// 用户角色授权
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ActionResult Authen(string id)
        {
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View(new UserAddDto());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var dto = _userService.Find(id);
            var model = _mapper.Map<UserDto, UserUpdateDto>(dto);
            return View(model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(UserAddDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Add(dto);
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
        public ActionResult Edit(UserUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Update(dto);
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
                result.flag = _userService.Delete(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetListWithPager(UserFilters filters)
        {
            var result = _userService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我的角色
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetMyRoles(RoleFilters filters)
        {
            var result = _roleService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我尚未拥有的角色
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public JsonResult GetNotMyRoles(RoleFilters filters)
        {
            filters.ExcludeMyRoles = true;
            var result = _roleService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/giveRight/{id}/{userId}")]
        public JsonResult GiveRight(string id, string userId)
        {
            var result = new JsonResultModel<bool>
            {
                flag = _userService.Give(userId, id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户角色取消
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/cancelRight/{id}/{userId}")]
        public JsonResult CancelRight(string id, string userId)
        {
            var result = new JsonResultModel<bool>
            {
                flag = _userService.Cancel(userId, id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPwd(string id)
        {
            return View(new ResetPasswordDto {UserId = id});
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPwd(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                _userService.ResetPwd(model.UserId, model.Password);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}