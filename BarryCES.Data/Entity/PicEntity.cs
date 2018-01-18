using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarryCES.Data.Entity
{
    public class PicEntity:BaseEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 跳转url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 所属模块id
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 是否是轮播图
        /// </summary>
        public bool IsCarousel { get; set; } 
    }
}
