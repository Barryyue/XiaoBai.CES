using System.Collections.Generic;

namespace BarryCES.Data.Entity
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectEntity : BaseEntity
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目列表信息
        /// </summary>
        public virtual IList<ProjectItemEntity> ProjectItems { get; set; } 
    }
}
