﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
    /// 角色契约实现
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public RoleService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto">角色模型</param>
        /// <returns></returns>
        public string Add(RoleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<RoleDto, RoleEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                var db = scope.DbContexts.Get<BarryCESContext>();

                var dbSet = db.Set<RoleEntity>();
                dbSet.Add(entity);

                return scope.SaveChanges() > 0 ? entity.Id : string.Empty;
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dto">角色模型</param>
        /// <returns></returns>
        public bool Update(RoleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var entity = dbSet.Find(dto.Id);
                _mapper.Map(dto, entity);
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public RoleDto Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var entity = dbSet.Find(id);
                var dto = _mapper.Map<RoleEntity, RoleDto>(entity);
                return dto;
            }
        }

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public bool Delete(IEnumerable<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var entities = dbSet.Where(item => ids.Contains(item.Id));
                entities.ForEach(item => item.IsDeleted = true);
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public PagedResult<RoleDto> Search(RoleFilters filters)
        {
            if (filters == null)
                return new PagedResult<RoleDto>(filters.page, filters.rows);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);

                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.Name.Contains(filters.keywords));

                if (filters.UserId.IsNotBlank())
                {
                    var userRoles = db.Set<UserRoleEntity>();
                    var myRoleIds = userRoles.Where(item => item.UserId == filters.UserId)
                                    .Select(item => item.RoleId)
                                    .ToList();
                    query = filters.ExcludeMyRoles
                        ? query.Where(item => !myRoleIds.Contains(item.Id))
                        : query.Where(item => myRoleIds.Contains(item.Id));
                }

                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new RoleDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description
                    }).Paging(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <returns></returns>
        public List<TreeDto> GetTrees()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var list = dbSet.Where(r => !r.IsDeleted).ToList();
                return _mapper.Map<List<RoleEntity>, List<TreeDto>>(list);
            }
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <returns></returns>
        public bool SetRoleMenus(List<RoleMenuDto> datas)
        {
            if (!datas.AnyOne()) return false;

            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleMenuEntity>();
                var roleId = datas.First().RoleId;
                var olds = dbSet.Where(item => item.RoleId == roleId).ToList();
                var oldIds = olds.Select(item => item.MenuId);
                var newIds = datas.Select(item => item.MenuId);
                var adds = datas.Where(item => !oldIds.Contains(item.MenuId)).ToList();
                var removes = olds.Where(item => !newIds.Contains(item.MenuId)).ToList();
                if (adds.AnyOne())
                {
                    var roleMenus = _mapper.Map<List<RoleMenuDto>, List<RoleMenuEntity>>(adds);
                    roleMenus.ForEach(item =>
                    {
                        item.Id = BaseIdGenerator.Instance.GetId();
                    });
                    dbSet.AddRange(roleMenus);
                }
                if (removes.AnyOne())
                {
                    dbSet.RemoveRange(removes);
                }

                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 清空该角色下的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public bool ClearRoleMenus(string roleId)
        {
            if (roleId.IsBlank()) return false;

            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleMenuEntity>();
                var list = dbSet.Where(item => item.RoleId == roleId);
                dbSet.RemoveRange(list);

                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto">角色模型</param>
        /// <returns></returns>
        public async Task<string> AddAsync(RoleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<RoleDto, RoleEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                var db = scope.DbContexts.Get<BarryCESContext>();

                var dbSet = db.Set<RoleEntity>();
                dbSet.Add(entity);

                return await scope.SaveChangesAsync() > 0 ? entity.Id : string.Empty;
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dto">角色模型</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(RoleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var entity = dbSet.Find(dto.Id);
                _mapper.Map(dto, entity);
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<RoleDto> FindAsync(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var entity = await dbSet.FindAsync(id);
                var dto = _mapper.Map<RoleEntity, RoleDto>(entity);
                return dto;
            }
        }

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var entities = dbSet.Where(item => ids.Contains(item.Id));
                entities.ForEach(item => item.IsDeleted = true);
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<RoleDto>> SearchAsync(RoleFilters filters)
        {
            if (filters == null)
                return new PagedResult<RoleDto>(filters.page, filters.rows);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);

                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.Name.Contains(filters.keywords));

                if (filters.UserId.IsNotBlank())
                {
                    var userRoles = db.Set<UserRoleEntity>();
                    var myRoleIds =
                            await
                                userRoles.Where(item => item.UserId == filters.UserId)
                                    .Select(item => item.RoleId)
                                    .ToListAsync();
                    query = filters.ExcludeMyRoles
                        ? query.Where(item => !myRoleIds.Contains(item.Id))
                        : query.Where(item => myRoleIds.Contains(item.Id));
                }

                return await query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new RoleDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeDto>> GetTreesAsync()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleEntity>();
                var list = await dbSet.Where(r => !r.IsDeleted).ToListAsync();
                return _mapper.Map<List<RoleEntity>, List<TreeDto>>(list);
            }
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SetRoleMenusAsync(List<RoleMenuDto> datas)
        {
            if (!datas.AnyOne()) return false;

            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleMenuEntity>();
                var roleId = datas.First().RoleId;
                var olds = await dbSet.Where(item => item.RoleId == roleId).ToListAsync();
                var oldIds = olds.Select(item => item.MenuId);
                var newIds = datas.Select(item => item.MenuId);
                var adds = datas.Where(item => !oldIds.Contains(item.MenuId)).ToList();
                var removes = olds.Where(item => !newIds.Contains(item.MenuId)).ToList();
                if (adds.AnyOne())
                {
                    var roleMenus = _mapper.Map<List<RoleMenuDto>, List<RoleMenuEntity>>(adds);
                    roleMenus.ForEach(item =>
                    {
                        item.Id = BaseIdGenerator.Instance.GetId();
                    });
                    dbSet.AddRange(roleMenus);
                }
                if (removes.AnyOne())
                {
                    dbSet.RemoveRange(removes);
                }

                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 清空该角色下的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> ClearRoleMenusAsync(string roleId)
        {
            if (roleId.IsBlank()) return false;

            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<RoleMenuEntity>();
                var list = dbSet.Where(item => item.RoleId == roleId);
                dbSet.RemoveRange(list);

                return await scope.SaveChangesAsync() > 0;
            }
        }
    }
}
