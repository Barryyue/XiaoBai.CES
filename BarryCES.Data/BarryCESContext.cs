/*******************************************************************************
* Copyright (C) BarryCES.Com
* 
* Author: Barry.yue
* Create Date: 2015/8/21
* Description: Automated building by service@BarryCES.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BarryCES.Data.Config;

namespace BarryCES.Data
{
    /// <summary>
    /// BarryCESContext
    /// </summary>
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class BarryCESContext : DbContext
    {
        /// <summary>
        /// BarryCESContext
        /// </summary>
        public BarryCESContext() : base("Dscoop")
        {
            //SQL语句拦截器
            //System.Data.Entity.Infrastructure.Interception.DbInterception.Add(new SqlCommandInterceptor());
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串名称</param>
        public BarryCESContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除一对多的级联删除关系
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //移除表名复数形式
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            //配置实体和数据表的关系
            modelBuilder.Configurations.AddFromAssembly(typeof(UserConfig).Assembly);
        }
    }
}
