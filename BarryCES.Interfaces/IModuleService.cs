using BarryCES.Infrastructure;
using BarryCES.Models;
using BarryCES.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarryCES.Interfaces
{
    public interface IModuleService
    {
        /// <summary>
        /// 添加板块
        /// </summary>
        /// <param name="dto">板块模型</param>
        /// <returns></returns>
        string Add(ModuleDto dto);

        /// <summary>
        /// 更新板块
        /// </summary>
        /// <param name="dto">板块模型</param>
        /// <returns></returns>
        bool Update(ModuleDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        ModuleDto Find(string id);

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
        PagedResult<ModuleDto> Search(MenuFilters filters);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        PagedResult<ModuleDto> AdvanceSearch(AdvanceFilter filters);


        /// <summary>
        /// 获取板块树
        /// </summary>
        /// <returns></returns>
        List<ModuleDto> GetList();


        /// <summary>
        /// 添加板块
        /// </summary>
        /// <param name="dto">板块模型</param>
        /// <returns></returns>
        Task<string> AddAsync(ModuleDto dto);

        /// <summary>
        /// 更新板块
        /// </summary>
        /// <param name="dto">板块模型</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(ModuleDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<ModuleDto> FindAsync(string id);

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
        Task<PagedResult<ModuleDto>> SearchAsync(MenuFilters filters);
        
        /// <summary>
        /// 获取板块树
        /// </summary>
        /// <returns></returns>
        Task<List<TreeDto>> GetTreesAsync();
        
    }
}
