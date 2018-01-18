using System.Collections.Generic;
using BarryCES.Infrastructure;
using BarryCES.Models;
using BarryCES.Models.Filters;

namespace BarryCES.Interfaces
{
    /// <summary>
    /// 项目服务接口
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="projects">项目信息</param>
        /// <returns></returns>
        bool Add(IList<ProjectAddDto> projects);

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        PagedResult<ProjectDto> Search(ProjectFilter filters);

        /// <summary>
        /// 获取项目列表信息
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        PagedResult<ProjectItemDto> SearchItems(ProjectItemFilter filter);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">项目ID集合</param>
        /// <returns></returns>
        bool Delete(IList<string> ids);

        /// <summary>
        /// 获取部件列表信息
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        PagedResult<PartDto> SearchParts(BaseFilter filter);
    }
}
