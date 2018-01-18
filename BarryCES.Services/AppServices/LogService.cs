using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
    /// <summary>
    /// 日志契约实现
    /// </summary>
    public class LogService : ILogService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public LogService(IDbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <summary>
        /// 获取登录日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public PagedResult<LoginLogDto> SearchLoginLogs(LogFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<LoginLogEntity>();
                var query = dbSet.AsQueryable();
                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.LoginName.Contains(filters.keywords));

                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new LoginLogDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        LoginName = item.LoginName,
                        IP = item.IP,
                        Mac = item.Mac,
                        CreateDateTime = item.CreateDateTime
                    }).Paging(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取访问日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public PagedResult<VisitDto> SearchVisitLogs(LogFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PageViewEntity>();
                var query = dbSet.AsQueryable();
                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.LoginName.Contains(filters.keywords));

                return query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new VisitDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        LoginName = item.LoginName,
                        IP = item.IP,
                        Url = item.Url,
                        VisitDate = item.CreateDateTime
                    }).Paging(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取登录日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public async Task<PagedResult<LoginLogDto>> SearchLoginLogsAsync(LogFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<LoginLogEntity>();
                var query = dbSet.AsQueryable();
                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.LoginName.Contains(filters.keywords));

                return await query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new LoginLogDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        LoginName = item.LoginName,
                        IP = item.IP,
                        Mac = item.Mac,
                        CreateDateTime = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取访问日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public async Task<PagedResult<VisitDto>> SearchVisitLogsAsync(LogFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PageViewEntity>();
                var query = dbSet.AsQueryable();
                if (filters.keywords.IsNotBlank())
                    query = query.Where(item => item.LoginName.Contains(filters.keywords));

                return await query.OrderBy(item => item.CreateDateTime)
                    .Select(item => new VisitDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        LoginName = item.LoginName,
                        IP = item.IP,
                        Url = item.Url,
                        VisitDate = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取最近七天的访问统计数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<VisitDataDto>> GetLastestVisitDataAsync()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                const string fomart = "yyyy-MM-dd";
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day).AddDays(-7);
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PageViewEntity>();
                var query = await dbSet.Where(item => item.CreateDateTime >= date).ToListAsync();

                var result = (from item in query
                    group item by
                        new DateTime(item.CreateDateTime.Year, item.CreateDateTime.Month, item.CreateDateTime.Day)
                    into g
                    select new VisitDataDto
                    {
                        Date = g.Key.ToString("yyyy-MM-dd"),
                        Number = g.Count()
                    }).ToList();
                var datas = new List<VisitDataDto>();
                for (var i = 0; i < 7; i++)
                {
                    var currentDate = date.AddDays(i).ToString(fomart);
                    var data = result.FirstOrDefault(item => item.Date == currentDate);
                    if (data != null)
                        datas.Add(data);
                    datas.Add(new VisitDataDto { Date = currentDate, Number = 0 });
                }
                return datas;
            }
        }

        /// <summary>
        /// 获取最近七天的访问统计数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VisitDataDto> GetLastestVisitData()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                const string fomart = "yyyy-MM-dd";
                var now = DateTime.Now;
                var date = new DateTime(now.Year,now.Month,now.Day).AddDays(-7);
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PageViewEntity>();
                var query = dbSet.Where(item => item.CreateDateTime >= date).ToList();

                var result = (from item in query
                    group item by
                        new DateTime(item.CreateDateTime.Year, item.CreateDateTime.Month, item.CreateDateTime.Day)
                    into g
                    select new VisitDataDto
                    {
                        Date = g.Key.ToString(fomart),
                        Number = g.Count()
                    }).ToList();

                for (var i = 0; i < 7; i++)
                {
                    var currentDate = date.AddDays(i).ToString(fomart);
                    var data = result.FirstOrDefault(item => item.Date == currentDate);
                    if (data != null)
                        yield return data;
                    else
                        yield return new VisitDataDto {Date = currentDate, Number = 0};
                }
            }
        }
    }
}
