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
    public class ModuleService : IModuleService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public ModuleService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        public string Add(ModuleDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }

        public PagedResult<ModuleDto> AdvanceSearch(AdvanceFilter filters)
        {
            if (filters == null)
                return new PagedResult<ModuleDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var sql = new StringBuilder();
                sql.Append("SELECT * FROM Module WHERE IsDeleted=0 ");
                sql.Append(filters.GetCondition());
                var queryParams = filters.GetSqlParameters();

                return db.Database.SqlPagerQuery<ModuleDto>(sql.ToString(), queryParams, filters.page, filters.rows, "[order]");
            }
        }

        public bool Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public ModuleDto Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<ModuleEntity>();
                var entity = dbSet.Find(id);
                var dto = _mapper.Map<ModuleEntity, ModuleDto>(entity);
                if (dto.ParentId.IsNotBlank())
                {
                    var parent = dbSet.Find(dto.ParentId);
                    dto.ParentName = parent.ModuleName;
                }
                return dto;
            }
        }

        public Task<ModuleDto> FindAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<ModuleDto> GetList()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<ModuleEntity>();
                var data = dbSet.Where(m => !m.IsDeleted).ToList();
                var result = _mapper.Map<List<ModuleEntity>, List<ModuleDto>>(data);
                return result;
            }
        }

        public Task<List<TreeDto>> GetTreesAsync()
        {
            throw new NotImplementedException();
        }

        public PagedResult<ModuleDto> Search(MenuFilters filters)
        {
            if (filters == null)
                return new PagedResult<ModuleDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<ModuleEntity>();
                var query = dbSet.Where(item => !item.IsDeleted && item.Level == 1);

                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.ModuleName.Contains(filters.keywords));

                return query.OrderBy(item => item.Order)
                     .Select(item => new ModuleDto
                     {
                         Id = item.Id,
                         ParentId = item.ParentId,
                         ModuleName = item.ModuleName,
                         Order = item.Order,
                         Level = item.Level
                     }).Paging(filters.page, filters.rows);
            }
        }

        public Task<PagedResult<ModuleDto>> SearchAsync(MenuFilters filters)
        {
            throw new NotImplementedException();
        }

        public bool Update(ModuleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<ModuleEntity>();
                var entity = dbSet.Find(dto.Id);
                entity.ModuleName = dto.ModuleName;
                entity.Order = dto.Order;
                entity.EditDateTime = dto.EditDateTime;
                entity.EditUserId = dto.EditUserId;

                return scope.SaveChanges() > 0;
            }
        }

        public Task<bool> UpdateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
