using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarryCES.Data.Entity
{
    /// <summary>
    /// 内容板块
    /// </summary>
    public class ModuleEntity:BaseEntity
    {

        /// <summary>
        /// 上级节点Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 板块名
        /// </summary>
        public string ModuleName { get; set; }
        
        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 节点等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public int IsPublic { get; set; }

    }
}
