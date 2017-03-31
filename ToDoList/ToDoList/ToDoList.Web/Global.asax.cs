using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ToDoList.EntityFramework;

namespace ToDoList.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			Services.Async = AsyncService.Default;

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			var folderPath = Server.MapPath("~/App_Data");
			AppDomain.CurrentDomain.SetData("DataDirectory", folderPath);

			return;
		}
	}
}