using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Models.Enum;
using BarryCES.Models.Filters;

namespace BarryCES.Models
{
    public class DocDto:BaseDto
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        [DisplayName("文章标题"),Required,MinLength(2)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName("文章内容"), Required]
        public string Content { get; set; }

        /// <summary>
        /// 所属类型
        /// </summary>
        [DisplayName("所属类型"), Required]
        public string TypeId { get; set; }

        public string ParentTypeId { get; set; }

        public string ParentTypeName { get; set; }

        [DisplayName("所属类型")]
        public string TypeName { get; set; }

        /// <summary>
        /// 列表图像
        /// </summary>
        [DisplayName("列表图片")]
        public string Avatar { get; set; }
    }

    public class DocSearchDto: BaseFilter
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 所属类型
        /// </summary>
        public string TypeId { get; set; }
    }
}
