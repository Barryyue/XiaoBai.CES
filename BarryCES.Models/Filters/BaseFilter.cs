using System.Collections.Generic;
using System.Reflection;
using System;

namespace BarryCES.Models.Filters
{
    /// <summary>
    /// 基本过滤器
    /// </summary>
    public class BaseFilter
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页显示的数据量
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string keywords { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string sord { get; set; }

        public DateTime begin { get; set; }

        public DateTime end { get; set; }
    }
}
