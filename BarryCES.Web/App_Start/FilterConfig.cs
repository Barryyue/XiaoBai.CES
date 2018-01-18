
using System.Web.Mvc;
using BarryCES.Web.Filters;

namespace BarryCES.Web
{
    /// <summary>
    /// FilterConfig
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SiteExceptionAttribute());
            filters.Add(new LoginAuthorize());
            filters.Add(new RightFilter());
            filters.Add(new VisitFilter());
        }
    }
}
