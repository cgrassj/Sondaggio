using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using Questionario.Db.Models;
using Questionario.Db;

[assembly: OwinStartup(typeof(Questionario.Web.Startup))]
namespace Questionario.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule(new ModuloCore());
			containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			var container = containerBuilder.Build();

			HttpConfiguration config = new HttpConfiguration();
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional } );
			
			var builder = new ODataConventionModelBuilder();
			builder.EntitySet<Sondaggio>("Sondaggi");
			builder.EntitySet<Domanda>("Domande");
			builder.EntitySet<Risposta>("Risposte");
			builder.EntitySet<Utente>("Utenti");
			config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
			config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

			// JSON formatter
			JsonMediaTypeFormatter formatter = config.Formatters.JsonFormatter;
			formatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
			formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			appBuilder.UseWebApi(config);
		}
	}
}