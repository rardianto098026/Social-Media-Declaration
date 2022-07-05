using System.Web;
using System.Web.Mvc;

namespace Annual_Compliance_Declaration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
