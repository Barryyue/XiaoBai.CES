using BarryCES.Models.Enum;

namespace BarryCES.Models.Filters
{
    /// <summary>
    /// 菜单搜索过滤器
    /// </summary>
    public class MenuFilters : BaseFilter
    {
        /// <summary>
        /// 排除的类型
        /// </summary>
        public MenuType? ExcludeType { get; set; }
    }

    
}
