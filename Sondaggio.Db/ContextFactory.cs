using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionario.Db
{
	public class ContextFactory
	{
		public TContext GetContext<TContext>(bool readOnly = false, bool lazyLoad = false) where TContext : DbContext, new()
		{
			var context = new TContext();
			context.Configuration.AutoDetectChangesEnabled = readOnly;
			context.Configuration.ProxyCreationEnabled = lazyLoad;
			context.Configuration.LazyLoadingEnabled = lazyLoad;

			return context;
		}
	}
}
