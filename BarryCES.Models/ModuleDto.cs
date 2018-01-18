using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Models.Enum;

namespace BarryCES.Models
{
    /// <summary>
    /// 板块Dto
    /// </summary>
    public class ModuleDto:BaseDto
    {
        /// <summary>
        /// 上级板块
        /// </summary>
        [DisplayName("上级板块")]
        public string ParentId { get; set; }

        /// <summary>
        /// 上级板块
        /// </summary>
        [DisplayName("上级板块")]
        public string ParentName { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        [DisplayName("板块名称"), Required, MinLength(2), MaxLength(20)]
        public string ModuleName { get; set; }

       
        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        [DisplayName("排序数字")]
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
