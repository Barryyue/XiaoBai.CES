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
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Models.Filters;
using Mehdime.Entity;

namespace BarryCES.Services.AppServices
{
    public class DocService : IDocService
    {

        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public DocService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }
        public string Add(DocDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddAsync(DocDto dto)
        {
            throw new NotImplementedException();
        }

        public PagedResult<DocDto> AdvanceSearch(AdvanceFilter filters)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public DocDto Find(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DocDto> FindAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<DocDto> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<List<TreeDto>> GetTreesAsync()
        {
            throw new NotImplementedException();
        }

        public PagedResult<DocDto> Search(DocSearchDto model)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<DocEntity>();
                var moduleDbSet = db.Set<ModuleEntity>();

                var query = dbSet.Where(c => c.Title.Contains(model.Title)
                  && c.Content.Contains(model.Content)
                  && c.TypeId == model.TypeId && !c.IsDeleted);

                var list = query.OrderByDescending(c=>c.EditDateTime).ThenByDescending(c=>c.CreateDateTime).Skip((model.page - 1) * model.rows).Take(model.rows).ToList();

                if (!list.Any())
                {
                    return new PagedResult<DocDto> { records=0,rows=null};
                }
                var typeIds = list.Select(c => c.TypeId);
                var modules = moduleDbSet.Where(c => typeIds.Contains(c.Id)).ToList();

                foreach (var item in modules)
                {
                    if (item.Level>1)
                    {
                        item.ModuleName = moduleDbSet.Where(c => c.Id == item.ParentId).FirstOrDefault().ModuleName + "," + item.ModuleName;
                    }
                }

                return new PagedResult<DocDto>
                {
                    records = query.Count(),
                    rows = list.Select(c => new DocDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content.Substring(0, 20),
                        TypeId = c.TypeId,
                        TypeName = (modules.FirstOrDefault(s => s.Id == c.TypeId).ModuleName).Split(',')[1],
                        ParentTypeId= modules.FirstOrDefault(s => s.Id == c.TypeId).ParentId,
                        ParentTypeName= (modules.FirstOrDefault(s => s.Id == c.TypeId).ModuleName).Split(',')[0],
                        Avatar = c.Avatar,
                        CreateDateTime = c.CreateDateTime,
                        CreateUserId = c.CreateUserId
                    }).ToList()
                };
            }
        }

        public Task<PagedResult<DocDto>> SearchAsync(MenuFilters filters)
        {
            throw new NotImplementedException();
        }

        public bool Update(DocDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DocDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
