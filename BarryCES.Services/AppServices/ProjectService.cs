using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BarryCES.Data;
using BarryCES.Data.Entity;
using BarryCES.Infrastructure;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Infrastructure.Utilities;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Filters;
using Mehdime.Entity;

namespace BarryCES.Services.AppServices
{
    /// <summary>
    /// 项目服务
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public ProjectService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="projects">项目信息</param>
        /// <returns></returns>
        public bool Add(IList<ProjectAddDto> projects)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entities = _mapper.Map<IList<ProjectAddDto>, IList<ProjectEntity>>(projects);
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<ProjectEntity>();

                foreach (var entity in entities)
                {
                    entity.Id = BaseIdGenerator.Instance.GetId();
                    if (entity.ProjectItems != null)
                    {
                        entity.ProjectItems.ForEach(item => item.Id = BaseIdGenerator.Instance.GetId());
                    }

                    dbSet.Add(entity);
                }

                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public PagedResult<ProjectDto> Search(ProjectFilter filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var query = db.Set<ProjectEntity>().Where(item => !item.IsDeleted);
                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.Name.Contains(filters.keywords));

                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new ProjectDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        CreateDateTime = item.CreateDateTime
                    }).Paging(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取项目列表信息
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public PagedResult<ProjectItemDto> SearchItems(ProjectItemFilter filter)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var query = db.Set<ProjectItemEntity>()
                    .Where(item => !item.IsDeleted &&
                                   item.ProjectId == filter.Id);
                
                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new ProjectItemDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price
                    }).Paging(filter.page, filter.rows);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">项目ID集合</param>
        /// <returns></returns>
        public bool Delete(IList<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var projectDbSet = db.Set<ProjectEntity>();

                var projects = projectDbSet.Where(item => ids.Contains(item.Id));

                projects.ForEach(p => p.IsDeleted = true);

                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 获取部件列表信息
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public PagedResult<PartDto> SearchParts(BaseFilter filter)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var query = db.Set<PartEntity>().Where(item => !item.IsDeleted);

                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new PartDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        CreateDateTime = item.CreateDateTime
                    }).Paging(filter.page, filter.rows);
            }
        }
    }
}
