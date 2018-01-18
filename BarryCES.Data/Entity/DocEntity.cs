using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarryCES.Data.Entity
{
    /// <summary>
    /// 文章（图文、视频连接）
    /// </summary>
    public class DocEntity: BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 列表图像
        /// </summary>
        public string Avatar { get; set; }

    }
}
