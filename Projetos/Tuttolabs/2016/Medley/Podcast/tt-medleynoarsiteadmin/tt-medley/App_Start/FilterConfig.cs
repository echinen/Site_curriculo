using System.Web;
using System.Web.Mvc;
using tt_medley.Functions;

namespace tt_medley
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomAuthorizeAttribute());
        }
    }
}
