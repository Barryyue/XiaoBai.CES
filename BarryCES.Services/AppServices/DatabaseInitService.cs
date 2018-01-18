using BarryCES.Data;
using BarryCES.Data.Entity;
using BarryCES.Interfaces;
using Mehdime.Entity;

namespace BarryCES.Services.AppServices
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    public class DatabaseInitService : IDatabaseInitService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public DatabaseInitService(IDbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            InitData.Init();
        }

        /// <summary>
        /// 初始化路径码
        /// </summary>
        public bool InitPathCode()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<BarryCESContext>();
                var dbSet = db.Set<PathCodeEntity>();

                var list = InitData.GetPathCodes();

                db.Database.ExecuteSqlCommand("DELETE FROM PathCodes");
                dbSet.AddRange(list);

                return scope.SaveChanges() > 0;
            }
        }
    }
}
