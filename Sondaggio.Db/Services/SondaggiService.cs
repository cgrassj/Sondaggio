using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Questionario.Db.BusinessObjects;
using Questionario.Db.Models;

namespace Questionario.Db.Services
{
	public class SondaggiService 
	{
		private readonly ContextFactory _contextFactory;

		public SondaggiService(ContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public List<ValutazioneSondaggio> GetSondaggi()
		{
			List<ValutazioneSondaggio> p2 = new List<ValutazioneSondaggio>();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				List<Sondaggio> p = db.Sondaggi
					.Include(a => a.Domande)
					.Include(e => e.Domande.Select(r => r.Risposte))
					.ToList();
				
				foreach (var sondaggio in p)
				{
					if (sondaggio?.Domande != null && sondaggio.Domande.Count > 0)
						sondaggio.ListaServizi = string.Join("\n", sondaggio.Domande.Select(a => a.TitoloDomanda));

					p2.Add(new ValutazioneSondaggio(sondaggio));
				}
			}
			return p2;
		}
	}
}