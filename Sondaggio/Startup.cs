using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Sondaggio.Startup))]

namespace Sondaggio
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			//Default
			//ConfigureAuth(app);
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(name: "DefaultApi",routeTemplate: "api/{controller}/{id}",defaults: new { id = RouteParameter.Optional });
			app.UseWebApi(config);
		}
	}
}
