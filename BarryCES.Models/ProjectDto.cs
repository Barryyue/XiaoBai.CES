using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarryCES.Models
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }
    }

    /// <summary>
    /// 项目列表
    /// </summary>
    public class ProjectItemDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
    }

    public class ProjectAddDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [Display(Name = "项目名称"), Required, MinLength(1), MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 项目列表
        /// </summary>
        public IList<ProjectItemAddDto> ProjectItems { get; set; } 
    }

    /// <summary>
    /// 项目列表
    /// </summary>
    public class ProjectItemAddDto
    {

        /// <summary>
        /// 项目列表名称
        /// </summary>
        [Display(Name = "项目列表名称"), Required, MinLength(1), MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 项目列表价格
        /// </summary>
        [Display(Name = "项目列表价格"), Required]
        public decimal Price { get; set; }
    }
}
