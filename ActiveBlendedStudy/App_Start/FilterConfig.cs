using ActiveBlendedStudy.Filters;
using System.Web;
using System.Web.Mvc;

namespace ActiveBlendedStudy
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
