using System.Web.Mvc;

namespace mebel.Areas.mebelAdmin
{
    public class mebelAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "mebelAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "mebelAdmin_default",
                "mebelAdmin/{controller}/{action}/{id}",
                new {Controller="Home",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}