using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Models.Enum;
using Newtonsoft.Json;

namespace BarryCES.Models.Filters
{
    /// <summary>
    /// 高级查询
    /// </summary>
    public class AdvanceFilter : BaseFilter
    {
        /// <summary>
        /// 连接符号
        /// </summary>
        public GroupOperator GroupOperator { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        public IList<RuleFilter> Rules { get; set; }
    }

    /// <summary>
    /// 规则过滤器
    /// </summary>
    public class RuleFilter
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 连接符号
        /// </summary>
        [JsonProperty("OperatorName")]
        public string Operater{ get; set; }

        /// <summary>
        /// 查询值
        /// </summary>
        public string Data { get; set; }
    }



    /// <summary>
    /// 扩展
    /// </summary>
    public static class FilterExtention
    {
        /// <summary>
        /// 组装成Sql条件
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <returns></returns>
        public static string GetCondition(this AdvanceFilter filters)
        {
            if (!filters.Rules.AnyOne())
                return string.Empty;

            var sql = new StringBuilder();
            sql.Append(" AND (");
            for (var i = 0; i < filters.Rules.Count; i++)
            {
                var rule = filters.Rules[i];
                sql.AppendFormat(" {0} {1} ",
                    i == 0 ? string.Empty : filters.GroupOperator.ToString(),
                    rule.ToSqlCondition());
            }
            sql.Append(")");

            return sql.ToString();
        }

        /// <summary>
        /// 获取组装后的sql参数
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <returns></returns>
        public static SqlParameter[] GetSqlParameters(this AdvanceFilter filters)
        {
            if (!filters.Rules.AnyOne())
                return null;

            var queryParams = new List<SqlParameter>();
            var length = filters.Rules.Count;
            for (var i = 0; i < length; i++)
            {
                var rule = filters.Rules[i];
                queryParams.Add(new SqlParameter
                {
                    ParameterName = "@" + rule.FieldName,
                    Direction = ParameterDirection.Input,
                    Value = rule.Data,
                    Size = 200
                });
            }
            return queryParams.ToArray();
        }

        /// <summary>
        /// 组装成sql条件(参数化)
        /// </summary>
        /// <param name="rule">查询条件</param>
        /// <returns></returns>
        public static string ToSqlCondition(this RuleFilter rule)
        {
            if (rule == null || rule.FieldName.IsBlank() || rule.Data.IsBlank())
                return string.Empty;
            const string at = "@";
            switch (rule.Operater)
            {
                case "ne":
                    return string.Format("{0} != {1}{0}", rule.FieldName, at);
                case "bw":
                    return string.Format("{0} >= {1}{0}", rule.FieldName, at);
                case "bn":
                    return string.Format("{0} < {1}{0}", rule.FieldName, at);
                case "ew":
                    return string.Format("{0} <= {1}{0}", rule.FieldName, at);
                case "en":
                    return string.Format("{0} > {1}{0} ", rule.FieldName, at);
                case "cn":
                    rule.Data = string.Format("%{0}%", rule.Data);
                    return string.Format("{0} like {1}{0} ", rule.FieldName, at);
                case "nc":
                    rule.Data = string.Format("%{0}%", rule.Data);
                    return string.Format("{0} not like {1}{0} ", rule.FieldName, at);
                case "nu":
                    return string.Format("{0} not in ({1}{0}) ", rule.FieldName, at);
                case "in":
                    return string.Format("{0} in ({1}{0}) ", rule.FieldName, at);
                case "ni":
                    return string.Format("{0} not in ({1}{0}) ", rule.FieldName, at);
                default:
                    return string.Format("{0} = {1}{0} ", rule.FieldName, at);
            }
        }
    }
}
