using System.Collections.Generic;
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
using BarryCES.Models.Enum;
using BarryCES.Models.Filters;
using Mehdime.Entity;

namespace BarryCES.Services.AppServices
{
    /// <summary>
    /// 用户实现类
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public UserService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Add(UserAddDto user)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<UserAddDto, UserEntity>(user);
                entity.Id = BaseIdGenerator.Instance.GetId();
                if (entity.UserRoles.AnyOne())
                {
                    entity.UserRoles.ForEach(r =>
                    {
                        r.UserId = entity.Id;
                        r.Id = BaseIdGenerator.Instance.GetId();
                    });
                }
                entity.Password = entity.Password.ToMd5();
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                dbSet.Add(entity);

                return scope.SaveChanges() > 0 ? entity.Id : string.Empty;
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public bool Update(UserUpdateDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var entity = dbSet.Find(dto.Id);
                entity.LoginName = dto.LoginName;
                entity.RealName = dto.RealName;
                entity.Email = dto.Email;
                if (dto.Password.IsNotBlank())
                    entity.Password = dto.Password.ToMd5();
                //_mapper.Map(dto, entity);
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public UserDto Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var entity = dbSet.Find(id);
                var dto = _mapper.Map<UserEntity, UserDto>(entity);
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
                var dbSet = db.Set<UserEntity>();
                var entities = dbSet.Where(item => ids.Contains(item.Id));
                entities.ForEach(item => item.IsDeleted = true);
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns></returns>
        public UserLoginDto Login(LoginDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var reslt = new UserLoginDto();
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var logDbSet = db.Set<LoginLogEntity>();
                var entity = dbSet.FirstOrDefault(item => item.LoginName == dto.LoginName.Trim());
                var loginLog = new LoginLogEntity
                {
                    Id = BaseIdGenerator.Instance.GetId(),
                    LoginName = dto.LoginName,
                    IP = dto.LoginIP
                };
                if (entity == null)
                {
                    reslt.Message = "账号不存在";
                    reslt.Result = LoginResult.AccountNotExists;
                    loginLog.UserId = "0";
                }
                else
                {
                    if (entity.Password == dto.Password.ToMd5())
                    {
                        reslt.LoginSuccess = true;
                        reslt.Message = "登陆成功";
                        reslt.Result = LoginResult.Success;
                        reslt.User = _mapper.Map<UserEntity, UserDto>(entity);
                    }
                    else
                    {
                        reslt.Message = "登陆密码错误";
                        reslt.Result = LoginResult.WrongPassword;
                    }
                    loginLog.UserId = entity.Id;
                }
                loginLog.Mac = reslt.Message;
                logDbSet.Add(loginLog);
                scope.SaveChanges();
                return reslt;
            }
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public bool Give(string userId, string roleId)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserRoleEntity>();
                if (dbSet.Any(item => item.UserId == userId && item.RoleId == roleId))
                    return true;
                dbSet.Add(new UserRoleEntity
                {
                    Id = BaseIdGenerator.Instance.GetId(),
                    UserId = userId,
                    RoleId = roleId
                });
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 用户角色取消
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public bool Cancel(string userId, string roleId)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserRoleEntity>();
                var userRole = dbSet.FirstOrDefault(item => item.UserId == userId && item.RoleId == roleId);
                dbSet.Remove(userRole);
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ResetPwd(string userId, string password)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var user = dbSet.FirstOrDefault(item => item.Id == userId);
                if (user == null) return false;

                user.Password = password.ToMd5();
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public PagedResult<UserDto> Search(UserFilters filters)
        {
            if (filters == null)
                return new PagedResult<UserDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);

                if (filters.keywords.IsNotBlank())
                    query =
                        query.Where(item => item.LoginName.Contains(filters.keywords) ||
                                            item.RealName.Contains(filters.keywords));

                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new UserDto
                    {
                        Id = item.Id,
                        LoginName = item.LoginName,
                        RealName = item.RealName,
                        Email = item.Email,
                        CreateDateTime = item.CreateDateTime
                    }).Paging(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 是否拥有此权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public bool HasRight(string userId, string url)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var menus = db.Set<MenuEntity>();
                var dbSetUserRoles = db.Set<UserRoleEntity>();
                var dbSetRoleMenus = db.Set<RoleMenuEntity>();
                var query = menus.Where(item => !item.IsDeleted);
                var roleIds = dbSetUserRoles.Where(item => item.UserId == userId)
                    .Select(item => item.RoleId).ToList();
                var menuIds = dbSetRoleMenus.Where(item => roleIds.Contains(item.RoleId))
                    .Select(item => item.MenuId)
                    .ToList();
                return query.Any(item => menuIds.Contains(item.Id) && url.StartsWith(item.Url));

            }
        }

        /// <summary>
        /// 记录访问记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool Visit(VisitDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PageViewEntity>();
                var entity = _mapper.Map<VisitDto, PageViewEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                dbSet.Add(entity);
                return scope.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> AddAsync(UserAddDto user)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<UserAddDto, UserEntity>(user);
                entity.Id = BaseIdGenerator.Instance.GetId();
                if (entity.UserRoles.AnyOne())
                {
                    entity.UserRoles.ForEach(r =>
                    {
                        r.UserId = entity.Id;
                        r.Id = BaseIdGenerator.Instance.GetId();
                    });
                }
                entity.Password = entity.Password.ToMd5();
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                dbSet.Add(entity);

                return await scope.SaveChangesAsync() > 0 ? entity.Id : string.Empty;
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UserUpdateDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var entity = dbSet.Find(dto.Id);
                entity.LoginName = dto.LoginName;
                entity.RealName = dto.RealName;
                entity.Email = dto.Email;
                if (dto.Password.IsNotBlank())
                    entity.Password = dto.Password.ToMd5();
                //_mapper.Map(dto, entity);
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<UserDto> FindAsync(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var entity = await dbSet.FindAsync(id);
                var dto = _mapper.Map<UserEntity, UserDto>(entity);
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
                var dbSet = db.Set<UserEntity>();
                var entities = dbSet.Where(item => ids.Contains(item.Id));
                entities.ForEach(item => item.IsDeleted = true);
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns></returns>
        public async Task<UserLoginDto> LoginAsync(LoginDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var reslt = new UserLoginDto();
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var logDbSet = db.Set<LoginLogEntity>();
                var entity = await dbSet.FirstOrDefaultAsync(item => item.LoginName == dto.LoginName.Trim());
                var loginLog = new LoginLogEntity
                {
                    Id = BaseIdGenerator.Instance.GetId(),
                    LoginName = dto.LoginName,
                    IP = dto.LoginIP
                };
                if (entity == null)
                {
                    reslt.Message = "账号不存在";
                    reslt.Result = LoginResult.AccountNotExists;
                    loginLog.UserId = "0";
                }
                else
                {
                    if (entity.Password == dto.Password.ToMd5())
                    {
                        reslt.LoginSuccess = true;
                        reslt.Message = "登陆成功";
                        reslt.Result = LoginResult.Success;
                        reslt.User = _mapper.Map<UserEntity, UserDto>(entity);
                    }
                    else
                    {
                        reslt.Message = "登陆密码错误";
                        reslt.Result = LoginResult.WrongPassword;
                    }
                    loginLog.UserId = entity.Id;
                }
                loginLog.Mac = reslt.Message;
                logDbSet.Add(loginLog);
                await scope.SaveChangesAsync();
                return reslt;
            }
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> GiveAsync(string userId, string roleId)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserRoleEntity>();
                if (await dbSet.AnyAsync(item => item.UserId == userId && item.RoleId == roleId))
                    return true;
                dbSet.Add(new UserRoleEntity
                {
                    Id = BaseIdGenerator.Instance.GetId(),
                    UserId = userId,
                    RoleId = roleId
                });
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 用户角色取消
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> CancelAsync(string userId, string roleId)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserRoleEntity>();
                var userRole = await dbSet.FirstOrDefaultAsync(item => item.UserId == userId && item.RoleId == roleId);
                dbSet.Remove(userRole);
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> ResetPwdAsync(string userId, string password)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var user = dbSet.FirstOrDefault(item => item.Id == userId);
                if (user == null) return false;

                user.Password = password.ToMd5();
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<UserDto>> SearchAsync(UserFilters filters)
        {
            if (filters == null)
                return new PagedResult<UserDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<UserEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);

                if (filters.keywords.IsNotBlank())
                    query =
                        query.Where(item => item.LoginName.Contains(filters.keywords) ||
                                            item.RealName.Contains(filters.keywords));

                return await query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new UserDto
                    {
                        Id = item.Id,
                        LoginName = item.LoginName,
                        RealName = item.RealName,
                        Email = item.Email,
                        CreateDateTime = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 是否拥有此权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public async Task<bool> HasRightAsync(string userId, string url)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<MenuEntity>();
                var dbSetUserRoles = db.Set<UserRoleEntity>();
                var dbSetRoleMenus = db.Set<RoleMenuEntity>();
                var query = dbSet.Where(item => !item.IsDeleted);
                var roleIds = await dbSetUserRoles.Where(item => item.UserId == userId)
                    .Select(item => item.RoleId).ToListAsync();
                var menuIds = await dbSetRoleMenus.Where(item => roleIds.Contains(item.RoleId))
                    .Select(item => item.MenuId)
                    .ToListAsync();
                return await query.AnyAsync(item => menuIds.Contains(item.Id) && url.StartsWith(item.Url));
                
            }
        }

        /// <summary>
        /// 记录访问记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> VisitAsync(VisitDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PageViewEntity>();
                var entity = _mapper.Map<VisitDto, PageViewEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                dbSet.Add(entity);
                return await scope.SaveChangesAsync() > 0;
            }
        }
    }
}
