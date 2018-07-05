using Autofac;

namespace Questionario.Db
{
	public class ModuloCore : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ContextFactory>().AsSelf().SingleInstance();
		}
	}
}
