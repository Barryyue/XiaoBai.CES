namespace BarryCES.Models.Filters
{
    /// <summary>
    /// 项目过滤器
    /// </summary>
    public class ProjectFilter : BaseFilter
    {
    }

    /// <summary>
    /// 项目列表过滤器
    /// </summary>
    public class ProjectItemFilter : BaseFilter
    {
        /// <summary>
        /// 所属项目ID
        /// </summary>
        public string Id { get; set; }
    }
}
