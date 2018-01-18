/*******************************************************************************
* Copyright (C) BarryCES.Com
* 
* Author: Barry.yue
* Create Date: 09/04/2015 11:47:14
* Description: Automated building by service@BarryCES.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using BarryCES.Data.Entity;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Infrastructure.Utilities;

namespace BarryCES.Data
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    internal sealed class DbConfiguration : DbMigrationsConfiguration<BarryCESContext>
    {
        private readonly DateTime _now = new DateTime(2015, 5, 1, 23, 59, 59);
        private readonly BaseIdGenerator _instance = BaseIdGenerator.Instance;

        public DbConfiguration()
        {
            AutomaticMigrationsEnabled = true;//启用自动迁移
            AutomaticMigrationDataLossAllowed = true;//是否允许接受数据丢失的情况，false=不允许，会抛异常；true=允许，有可能数据会丢失
        }

        protected override void Seed(BarryCESContext context)
        {
            if (!context.Set<SystemConfigEntity>().Any(item => item.IsDataInited))
            {
                #region 用户

                var admin = new UserEntity
                {
                    Id = _instance.GetId(),
                    LoginName = "Dscoop",
                    RealName = "超级管理员",
                    Password = "qwaszx..".ToMd5(),
                    Email = "service@dscoop.com",
                    IsSuperMan = true,
                    CreateDateTime = _now
                };
                var guest = new UserEntity
                {
                    Id = _instance.GetId(),
                    LoginName = "admin",
                    RealName = "游客",
                    Password = "qwaszx".ToMd5(),
                    Email = "service@dscoop.com",
                    CreateDateTime = _now
                };
                //用户
                var user = new List<UserEntity>
                {
                    admin,
                    guest
                };

                #endregion

                #region 菜单

                var backMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    Name = "后台管理",
                    Url = "#",
                    CreateDateTime = _now,
                    Order = 100,
                    Code = "AC",
                    PathCode = "AC",
                    Type = 1
                }; //1
                var moduleMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = backMgr.Id,
                    Name = "板块配置",
                    Url = "/ModuleSet/Index",
                    CreateDateTime = _now,
                    Order = 110,
                    Code = "AA",
                    PathCode = "ACAA",
                    Type = 2
                }; //2

                var docMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = backMgr.Id,
                    Name = "文章管理",
                    Url = "/Doc/Index",
                    CreateDateTime = _now,
                    Order = 120,
                    Code = "AB",
                    PathCode = "ACAB",
                    Type = 2
                }; //2

                var picMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = backMgr.Id,
                    Name = "图片管理",
                    Url = "/Pic/Index",
                    CreateDateTime = _now,
                    Order = 130,
                    Code = "AC",
                    PathCode = "ACAC",
                    Type = 2
                }; //2


                var system = new MenuEntity
                {
                    Id = _instance.GetId(),
                    Name = "系统设置",
                    Url = "#",
                    CreateDateTime = _now,
                    Order = 200,
                    Code = "AA",
                    PathCode = "AA",
                    Type = 1
                }; //1
                var menuMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "菜单管理",
                    Url = "/Menu/Index",
                    CreateDateTime = _now,
                    Order = 210,
                    Code = "AA",
                    PathCode = "AAAA",
                    Type = 2
                }; //2
                var roleMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "角色管理",
                    Url = "/Role/Index",
                    CreateDateTime = _now,
                    Order = 220,
                    Code = "AB",
                    PathCode = "AAAB",
                    Type = 2
                }; //3
                var userMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "用户管理",
                    Url = "/User/Index",
                    CreateDateTime = _now,
                    Order = 230,
                    Code = "AC",
                    PathCode = "AAAC",
                    Type = 2
                }; //4
                var userRoleMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userMgr.Id,
                    Name = "用户授权",
                    Url = "/User/Authen",
                    CreateDateTime = _now,
                    Order = 240,
                    Code = "AD",
                    PathCode = "AAAD",
                    Type = 2
                }; //5
                var roleMenuMgr = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "角色授权",
                    Url = "/Role/Authen",
                    CreateDateTime = _now,
                    Order = 250,
                    Code = "AE",
                    PathCode = "AAAE",
                    Type = 2
                }; //6
                var sysConfig = new MenuEntity
                {

                    Id = _instance.GetId(),
                    ParentId = system.Id,
                    Name = "系统配置",
                    Url = "/System/Index",
                    CreateDateTime = _now,
                    Order = 260,
                    Code = "AF",
                    PathCode = "AAAF",
                    Type = 2
                }; //7
                var sysConfigReloadPathCode = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = sysConfig.Id,
                    Name = "重置路径码",
                    Url = "/System/ReloadPathCode",
                    CreateDateTime = _now,
                    Order = 261,
                    Code = "AAAF",
                    PathCode = "AAAFAA",
                    Type = 3
                }; //8
                var log = new MenuEntity
                {
                    Id = _instance.GetId(),
                    Name = "日志查看",
                    Url = "#",
                    CreateDateTime = _now,
                    Order = 300,
                    Code = "AB",
                    PathCode = "AB",
                    Type = 1
                }; //9
                var logLogin = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = log.Id,
                    Name = "登录日志",
                    Url = "/Log/Logins",
                    CreateDateTime = _now,
                    Order = 310,
                    Code = "AA",
                    PathCode = "ABAA",
                    Type = 2
                }; //10
                var logView = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = log.Id,
                    Name = "访问日志",
                    Url = "/Log/Visits",
                    CreateDateTime = _now,
                    Order = 320,
                    Code = "AB",
                    PathCode = "ABAB",
                    Type = 2
                }; //11

                var logChart = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = log.Id,
                    Name = "图表统计",
                    Url = "/Log/Charts",
                    CreateDateTime = _now,
                    Order = 330,
                    Code = "AC",
                    PathCode = "ABAC",
                    Type = 2
                };
                var userRoleSet = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userRoleMgr.Id,
                    Name = "用户角色授权",
                    Url = "/User/GiveRight",
                    CreateDateTime = _now,
                    Order = 234,
                    Code = "AA",
                    PathCode = "AAADAA",
                    Type = 3
                };
                var userRoleCancel = new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = userRoleMgr.Id,
                    Name = "用户角色取消",
                    Url = "/User/CancelRight",
                    CreateDateTime = _now,
                    Order = 235,
                    Code = "AB",
                    PathCode = "AAADAB",
                    Type = 3
                };
                //菜单
                var menus = new List<MenuEntity>
                {
                    backMgr,
                    moduleMgr,
                    docMgr,
                    picMgr,
                    system,
                    menuMgr,
                    roleMgr,
                    userMgr,
                    userRoleMgr,
                    userRoleSet,
                    userRoleCancel,
                    roleMenuMgr,
                    sysConfig,
                    sysConfigReloadPathCode,
                    log,
                    logLogin,
                    logView,
                    logChart
                };
                var menuBtns = GetMenuButtons(menuMgr.Id, menuMgr.Order, "Menu", "菜单", "AAAA"); //14
                var rolwBtns = GetMenuButtons(roleMgr.Id, roleMgr.Order, "Role", "角色", "AAAB"); //17
                var userBtns = GetMenuButtons(userMgr.Id, userMgr.Order, "User", "用户", "AAAC"); //20

                menus.AddRange(menuBtns); //14
                menus.AddRange(rolwBtns); //17
                menus.AddRange(userBtns); //20

                var moduleBtns = GetMenuButtons(moduleMgr.Id, moduleMgr.Order, "ModuleSet", "板块", "AAAA"); //14
                var docBtns = GetMenuButtons(docMgr.Id, docMgr.Order, "Doc", "文章", "AAAB"); //17
                var picBtns = GetMenuButtons(picMgr.Id, picMgr.Order, "Pic", "图片", "AAAC"); //20

                menus.AddRange(moduleBtns); //14
                menus.AddRange(docBtns); //17
                menus.AddRange(picBtns); //20
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = roleMenuMgr.Id,
                    Order = 241,
                    Name = "授权",
                    Type = 3,
                    Url = "/Role/SetRoleMenus",
                    CreateDateTime = _now,
                    Code = "AA",
                    PathCode = "AAACAA"
                });
                menus.Add(new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = roleMenuMgr.Id,
                    Order = 242,
                    Name = "清空权限",
                    Type = 3,
                    Url = "/Role/ClearRoleMenus",
                    CreateDateTime = _now,
                    Code = "AB",
                    PathCode = "AAACAB"
                });

                #endregion

                #region 板块配置

                var module1 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "首页文章",
                    Level = 1,
                    IsPublic = 1,
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 100
                }; //1

                var module11 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ParentId=module1.Id,
                    ModuleName = "行业资讯",
                    Level = 2,
                    IsPublic = 1,
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 110
                }; //1

                var module2 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "数码业务扩展",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 200,
                    Level = 1,
                    IsPublic = 1
                }; //2

                var module21 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "行业洞察",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 210,
                    ParentId = module2.Id,
                    Level = 2,
                    IsPublic = 1
                }; //3

                var module22 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "视听资讯",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 220,
                    ParentId = module2.Id,
                    Level = 2,
                    IsPublic = 1
                }; //3

                var module3 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "技术交流学习",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 300,
                    Level = 1,
                    IsPublic = 1
                }; //2

                var module31 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "技术分享",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 310,
                    ParentId = module3.Id,
                    Level = 2,
                    IsPublic = 1
                }; //3

                var module32 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "视听教程",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 320,
                    ParentId = module3.Id,
                    Level = 2,
                    IsPublic = 1
                }; //3

                var module4 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "成功案例分享",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 400,
                    Level = 1,
                    IsPublic = 1
                }; //2

                var module41 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "成功案例",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 410,
                    ParentId = module4.Id,
                    Level = 2,
                    IsPublic = 1
                }; //3

                var module42 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "深入访谈",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 420,
                    ParentId = module4.Id,
                    Level = 2,
                    IsPublic = 1
                }; //3

                var module5 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "数码印刷讲堂",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 500,
                    Level = 1,
                    IsPublic = 1
                }; //2


                var module6 = new ModuleEntity
                {
                    Id = _instance.GetId(),
                    ModuleName = "Dscoop活动",
                    CreateDateTime = _now,
                    CreateUserId = admin.Id,
                    Order = 620,
                    Level = 1,
                    IsPublic = 1
                }; //3


                var modules = new List<ModuleEntity>
                {
                    module1,
                    module11,
                    module2,
                    module21,
                    module22,
                    module3,
                    module31,
                    module32,
                    module4,
                    module41,
                    module42,
                    module5,
                    module6
                };

                #endregion

                #region 角色

                var superAdminRole = new RoleEntity
                {
                    Id = _instance.GetId(),
                    Name = "超级管理员",
                    Description = "超级管理员"
                };
                var guestRole = new RoleEntity
                {
                    Id = _instance.GetId(),
                    Name = "guest",
                    Description = "游客"
                };
                var roles = new List<RoleEntity>
                {
                    superAdminRole,
                    guestRole
                };

                #endregion

                #region 用户角色关系

                var userRoles = new List<UserRoleEntity>
                {
                    new UserRoleEntity
                    {
                        Id = _instance.GetId(),
                        UserId = admin.Id,
                        RoleId = superAdminRole.Id,
                        CreateDateTime = _now
                    },
                    new UserRoleEntity
                    {
                        Id = _instance.GetId(),
                        UserId = guest.Id,
                        RoleId = guestRole.Id,
                        CreateDateTime = _now
                    }
                };

                #endregion

                #region 角色菜单权限关系

                var roleMenus = menus.Select(m => new RoleMenuEntity
                {
                    Id = _instance.GetId(),
                    RoleId = superAdminRole.Id,
                    MenuId = m.Id,
                    CreateDateTime = _now
                }).ToList();

                roleMenus.AddRange(menus.Where(item => item.Type != 3)
                    .Select(m => new RoleMenuEntity
                    {
                        Id = _instance.GetId(),
                        RoleId = guestRole.Id,
                        MenuId = m.Id,
                        CreateDateTime = _now
                    }));
                //超级管理员授权
                //游客授权

                #endregion

                #region 系统配置

                var systemConfig = new List<SystemConfigEntity>
                {
                    new SystemConfigEntity
                    {
                        Id = _instance.GetId(),
                        SystemName = "Dscoop",
                        IsDataInited = true,
                        DataInitedDate = _now,
                        CreateDateTime = _now,
                        IsDeleted = false
                    }
                };

                #endregion

                #region 路径码

                var pathCodes = InitData.GetPathCodes();

                #endregion

                #region 部件

                var parts = new List<PartEntity>
                {
                    new PartEntity { Id= _instance.GetId(),Name = "上"},
                    new PartEntity { Id= _instance.GetId(),Name = "中"},
                    new PartEntity { Id= _instance.GetId(),Name = "下"}
                };

                #endregion

                AddOrUpdate(context, m => m.ModuleName, modules.ToArray());
                AddOrUpdate(context, m => m.LoginName, user.ToArray());
                AddOrUpdate(context, m => new { m.ParentId, m.Name }, menus.ToArray());
                AddOrUpdate(context, m => m.Name, roles.ToArray());
                AddOrUpdate(context, m => new { m.UserId, m.RoleId }, userRoles.ToArray());
                AddOrUpdate(context, m => new { m.MenuId, m.RoleId }, roleMenus.ToArray());
                AddOrUpdate(context, m => m.SystemName, systemConfig.ToArray());
                AddOrUpdate(context, m => m.Code, pathCodes.ToArray());
                AddOrUpdate(context, m => m.Name, parts.ToArray());
            }
        }

        #region Private

        /// <summary>
        /// 获取菜单的基础按钮
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="controllerShowName">菜单显示名称</param>
        /// <param name="parentPathCode">父级路径码</param>
        /// <returns></returns>
        private IEnumerable<MenuEntity> GetMenuButtons(string parentId, int order, string controllerName, string controllerShowName, string parentPathCode)
        {
            return new List<MenuEntity>
            {
                new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = parentId,
                    Name = string.Concat("添加",controllerShowName),
                    Url = string.Format("/{0}/Add",controllerName),
                    CreateDateTime = _now,
                    Order = order+1,
                    Code = "AA",
                    PathCode = parentPathCode+"AA",
                    Type = 3
                },
                new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = parentId,
                    Name = string.Concat("修改",controllerShowName),
                    Url = string.Format("/{0}/Edit",controllerName),
                    CreateDateTime = _now,
                    Order = order+2,
                    Code = "AB",
                    PathCode = parentPathCode+"AB",
                    Type = 3
                },
                new MenuEntity
                {
                    Id = _instance.GetId(),
                    ParentId = parentId,
                    Name = string.Concat("删除",controllerShowName),
                    Url = string.Format("/{0}/Delete",controllerName),
                    CreateDateTime = _now,
                    Order = order+3,
                    Code = "AC",
                    PathCode = parentPathCode+"AC",
                    Type = 3
                }
            };
        }

        /// <summary>
        /// 添加更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="exp"></param>
        /// <param name="param"></param>
        void AddOrUpdate<T>(DbContext context, Expression<Func<T, object>> exp, T[] param) where T : class
        {
            var set = context.Set<T>();
            set.AddOrUpdate(exp, param);
        }

        #endregion
    }
}
