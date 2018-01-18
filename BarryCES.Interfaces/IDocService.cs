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
    public interface IDocService
    {
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto">文章模型</param>
        /// <returns></returns>
        string Add(DocDto dto);

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="dto">文章模型</param>
        /// <returns></returns>
        bool Update(DocDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        DocDto Find(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        bool Delete(IEnumerable<string> ids);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="model">查询过滤参数</param>
        /// <returns></returns>
        PagedResult<DocDto> Search(DocSearchDto model);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        PagedResult<DocDto> AdvanceSearch(AdvanceFilter filters);


        /// <summary>
        /// 获取文章树
        /// </summary>
        /// <returns></returns>
        List<DocDto> GetList();


        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto">文章模型</param>
        /// <returns></returns>
        Task<string> AddAsync(DocDto dto);

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="dto">文章模型</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(DocDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<DocDto> FindAsync(string id);

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
        Task<PagedResult<DocDto>> SearchAsync(MenuFilters filters);
        
        /// <summary>
        /// 获取文章树
        /// </summary>
        /// <returns></returns>
        Task<List<TreeDto>> GetTreesAsync();
        
    }
}
