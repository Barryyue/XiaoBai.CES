﻿
using System.Collections.Generic;
using System.Threading.Tasks;
using BarryCES.Infrastructure;
using BarryCES.Models;
using BarryCES.Models.Filters;

namespace BarryCES.Interfaces
{
    /// <summary>
    /// 菜单契约
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        string Add(MenuDto dto);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        bool Update(MenuDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        MenuDto Find(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        bool Delete(IEnumerable<string> ids);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        PagedResult<MenuDto> Search(MenuFilters filters);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        PagedResult<MenuDto> AdvanceSearch(AdvanceFilter filters);

        /// <summary>
        /// 获取用户拥有的权限菜单（不包含按钮）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        List<MenuDto> GetMyMenus(string userId);

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        List<TreeDto> GetTrees();

        /// <summary>
        /// 通过角色ID获取拥有的菜单权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        List<MenuDto> GetMenusByRoleId(string roleId);

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        Task<string> AddAsync(MenuDto dto);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(MenuDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<MenuDto> FindAsync(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<string> ids);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<PagedResult<MenuDto>> SearchAsync(MenuFilters filters);

        /// <summary>
        /// 获取用户拥有的权限菜单（不包含按钮）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<List<MenuDto>> GetMyMenusAsync(string userId);

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<TreeDto>> GetTreesAsync();

        /// <summary>
        /// 通过角色ID获取拥有的菜单权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        Task<List<MenuDto>> GetMenusByRoleIdAsync(string roleId);
    }
}
