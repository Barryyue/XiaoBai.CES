using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BarryCES.Data;
using BarryCES.Data.Entity;
using BarryCES.Infrastructure;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Infrastructure.Utilities;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Enum;
using BarryCES.Models.Filters;
using Mehdime.Entity;

namespace BarryCES.Services.AppServices
{
    /// <summary>
    /// 菜单契约服务
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public MenuService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public string Add(MenuDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<MenuDto, MenuEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                entity.CreateDateTime = DateTime.Now;
                entity.EditDateTime = DateTime.Now;
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var pathCodeDbSet = db.Set<PathCodeEntity>();

                var existsCode = dbSet.Where(item => item.ParentId == dto.ParentId)
                    .Select(item => item.Code).ToList();
                var pathCode = pathCodeDbSet.FirstOrDefault(item => !existsCode.Contains(item.Code));
                entity.Code = pathCode.Code.Trim();
                if (entity.ParentId.IsNotBlank())
                {
                    var parent = dbSet.Find(entity.ParentId);
                    entity.PathCode = string.Concat(parent.PathCode.Trim(), entity.Code.Trim());
                    entity.Type = parent.Type == 1 ? (byte)MenuType.Menu : (byte)MenuType.Button;
                }
                else
                {
                    entity.PathCode = entity.Code.Trim();
                    entity.Type = (byte)MenuType.Module;
                }
                dbSet.Add(entity);

                return scope.SaveChanges() > 0 ? entity.Id : string.Empty;
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public bool Update(MenuDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();

                var entity = dbSet.Find(dto.Id);
                entity.Name = dto.Name;
                entity.Url = dto.Url;
                entity.Order = dto.Order;
                entity.EditDateTime = dto.EditDateTime;
                entity.EditUserId = dto.EditUserId;

                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public MenuDto Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var entity = dbSet.Find(id);
                var dto = _mapper.Map<MenuEntity, MenuDto>(entity);
                if (dto.ParentId.IsNotBlank())
                {
                    var parent = dbSet.Find(dto.ParentId);
                    dto.ParentName = parent.Name;
                }
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
                var dbSet = db.Set<MenuEntity>();
                var entities = dbSet.Where(item => ids.Contains(item.Id)).ToList();
                foreach (var menuEntity in entities)
                {
                    menuEntity.IsDeleted = true;
                }
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public PagedResult<MenuDto> Search(MenuFilters filters)
        {
            if (filters == null)
                return new PagedResult<MenuDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);

                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.Name.Contains(filters.keywords));
                if (filters.ExcludeType.HasValue)
                    query = query.Where(item => item.Type != (byte)filters.ExcludeType.Value);

                return query.OrderBy(item => item.Order)
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType)item.Type
                    }).Paging(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public PagedResult<MenuDto> AdvanceSearch(AdvanceFilter filters)
        {
            if (filters == null)
                return new PagedResult<MenuDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM Menu WHERE IsDeleted=0 ");
                sql.Append(filters.GetCondition());
                var queryParams = filters.GetSqlParameters();

                return db.Database.SqlPagerQuery<MenuDto>(sql.ToString(), queryParams, filters.page, filters.rows,"[order]");
            }
        }

        /// <summary>
        /// 获取用户拥有的权限菜单（不包含按钮）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public List<MenuDto> GetMyMenus(string userId)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var dbSetUserRoles = db.Set<UserRoleEntity>();
                var dbSetRoleMenus = db.Set<RoleMenuEntity>();
                var query = dbSet.Where(item => !item.IsDeleted && item.Type != (byte)MenuType.Button);
                var roleIds = dbSetUserRoles.Where(item => item.UserId == userId)
                    .Select(item => item.RoleId).ToList();
                var menuIds = dbSetRoleMenus.Where(item => roleIds.Contains(item.RoleId))
                    .Select(item => item.MenuId)
                    .ToList();
                return query.Where(item => menuIds.Contains(item.Id))
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType)item.Type
                    }).OrderBy(c=>c.Order).ToList();
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public List<TreeDto> GetTrees()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var list = dbSet.Where(m => !m.IsDeleted).ToList();
                var result = _mapper.Map<List<MenuEntity>, List<TreeDto>>(list);
                result.ForEach(t => t.open = true);
                return result;
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenusByRoleId(string roleId)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var roleMenus = db.Set<RoleMenuEntity>();
                var list = dbSet.Where(m => !m.IsDeleted)
                .Join(roleMenus, m => m.Id, rm => rm.MenuId, (menu, roleMenu) => new { menu, roleMenu })
                .Where(item => item.roleMenu.RoleId == roleId)
                .Select(item => item.menu).ToList();
                return _mapper.Map<List<MenuEntity>, List<MenuDto>>(list);
            }
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public async Task<string> AddAsync(MenuDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<MenuDto, MenuEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var pathCodeDbSet = db.Set<PathCodeEntity>();

                var existsCode = await dbSet.Where(item => item.ParentId == dto.ParentId)
                    .Select(item => item.Code).ToListAsync();
                var pathCode = await pathCodeDbSet.FirstOrDefaultAsync(item => !existsCode.Contains(item.Code));
                entity.Code = pathCode.Code.Trim();
                if (entity.ParentId.IsNotBlank())
                {
                    var parent = await dbSet.FindAsync(entity.ParentId);
                    entity.PathCode = string.Concat(parent.PathCode.Trim(), entity.Code.Trim());
                    entity.Type = parent.Type == 1 ? (byte) MenuType.Menu : (byte) MenuType.Button;
                }
                else
                {
                    entity.PathCode = entity.Code.Trim();
                    entity.Type = (byte) MenuType.Module;
                }
                dbSet.Add(entity);

                return await scope.SaveChangesAsync() > 0 ? entity.Id : string.Empty;
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(MenuDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();

                var entity = dbSet.Find(dto.Id);
                entity.Name = dto.Name;
                entity.Url = dto.Url;
                entity.Order = dto.Order;

                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<MenuDto> FindAsync(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var entity = await dbSet.FindAsync(id);
                var dto = _mapper.Map<MenuEntity, MenuDto>(entity);
                if (dto.ParentId.IsNotBlank())
                {
                    var parent = await dbSet.FindAsync(dto.ParentId);
                    dto.ParentName = parent.Name;
                }
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
                var dbSet = db.Set<MenuEntity>();
                var entities = await dbSet.Where(item => ids.Contains(item.Id)).ToListAsync();
                foreach (var menuEntity in entities)
                {
                    menuEntity.IsDeleted = true;
                }
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<MenuDto>> SearchAsync(MenuFilters filters)
        {
            if (filters == null)
                return new PagedResult<MenuDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);

                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.Name.Contains(filters.keywords));
                if(filters.ExcludeType.HasValue)
                    query = query.Where(item => item.Type != (byte)filters.ExcludeType.Value);

                return await query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType)item.Type
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取用户拥有的权限菜单（不包含按钮）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<List<MenuDto>> GetMyMenusAsync(string userId)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var dbSetUserRoles = db.Set<UserRoleEntity>();
                var dbSetRoleMenus = db.Set<RoleMenuEntity>();
                var query = dbSet.Where(item => !item.IsDeleted && item.Type != (byte) MenuType.Button);
                var roleIds = await dbSetUserRoles.Where(item => item.UserId == userId)
                    .Select(item => item.RoleId).ToListAsync();
                var menuIds = await dbSetRoleMenus.Where(item => roleIds.Contains(item.RoleId))
                    .Select(item => item.MenuId)
                    .ToListAsync();
                return await query.Where(item => menuIds.Contains(item.Id))
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType)item.Type
                    }).ToListAsync();
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeDto>> GetTreesAsync()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var list = await dbSet.Where(m => !m.IsDeleted).ToListAsync();
                var result = _mapper.Map<List<MenuEntity>, List<TreeDto>>(list);
                result.ForEach(t => t.open = true);
                return result;
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuDto>> GetMenusByRoleIdAsync(string roleId)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var roleMenus = db.Set<RoleMenuEntity>();
                var list = await dbSet.Where(m => !m.IsDeleted)
                .Join(roleMenus, m => m.Id, rm => rm.MenuId, (menu, roleMenu) => new { menu, roleMenu })
                .Where(item => item.roleMenu.RoleId == roleId)
                .Select(item => item.menu).ToListAsync();
                return _mapper.Map<List<MenuEntity>, List<MenuDto>>(list);
            }
        }
    }
}
