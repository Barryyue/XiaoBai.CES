﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BarryCES.Infrastructure.Extentions
{
    /// <summary>
    /// DbContext扩展
    /// </summary>
    public static class DbContextExtention
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="db">数据库对象实例</param>
        /// <param name="sql">需要执行的sql语句</param>
        /// <param name="parameters">自定义参数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public static PagedResult<T> SqlPagerQuery<T>(this Database db, string sql, SqlParameter[] parameters, int pageIndex = 1, int pageSize = 15, string orderBy = "Id")
        {
            var basePageSql = @"SET NOCOUNT ON;
                                SELECT @Total = COUNT(1) FROM({#SQL#}) AS T
                                SELECT r.* FROM(SELECT ROW_NUMBER() OVER(ORDER BY {#OrderBy#}) as RowId, t.* FROM ({#SQL#}) AS t) r
                                WHERE r.RowId >= @StartID AND r.RowId <= @EndID";

            sql = basePageSql.Replace("{#SQL#}", sql).Replace("{#OrderBy#}", orderBy);

            var queryParam = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Total", Direction = ParameterDirection.Output,Size = 4},
                new SqlParameter {ParameterName = "@StartID", Value = (pageIndex - 1)*pageSize + 1,Size = 4},
                new SqlParameter {ParameterName = "@EndID", Value = pageIndex* pageSize,Size = 4}
            };
            if (parameters.AnyOne())
                queryParam.AddRange(parameters);

            var query = db.SqlQuery<T>(sql, queryParam.ToArray());

            return new PagedResult<T>
            {
                rows = query.ToList(),
                records = Convert.ToInt32(queryParam[0].Value),
                page = pageIndex,
                pagesize = pageSize
            };
        }
    }
}
