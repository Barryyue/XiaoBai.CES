namespace BarryCES.Data.Entity
{
    public class ProjectItemEntity : BaseEntity
    {
        /// <summary>
        /// 所属项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目列表名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目列表价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public virtual ProjectEntity Project { get; set; }
    }
}
