using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Questionario.Db;
using Questionario.Db.Models;

namespace Questionario.Web
{
	
	public class ApiSondaggiController : ApiController
	{
		private readonly ContextFactory _contextFactory;

		public ApiSondaggiController(ContextFactory contextFactory) => _contextFactory = contextFactory;
		[Route("api/sondaggi")]
		public async Task<IHttpActionResult> Get()
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
				return Ok(await db.Sondaggi
					.Include(a => a.Domande)
					.Include(e => e.Domande.Select(r => r.Risposte))
					.Include(e => e.Domande.Select(r => r.Risposte.Select(f=>f.Utente)))

					.ToListAsync());
		}

		[Route("api/sondaggi/{id}")]
		public async Task<IHttpActionResult> Get(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var sondaggio = await db.Sondaggi
					.Include(e => e.Domande)
					.Include(f=> f.Domande.Select(g=>g.Risposte))
					.Include(f=> f.Domande.Select(g=>g.Risposte.Select(k=>k.Utente)))
					.FirstOrDefaultAsync(e => e.IdSondaggio == id);
				if(sondaggio != null && sondaggio.Domande != null && sondaggio.Domande.Count > 0)
					sondaggio.ListaServizi = string.Join("\n", sondaggio.Domande.Select(a => a.TitoloDomanda));
				if (sondaggio == null)
					return NotFound();
				return Ok(sondaggio);
			}
		}

		[Route("api/sondaggi")]
		public async Task<IHttpActionResult> Post(Sondaggio sondaggio)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Sondaggi.Add(sondaggio);
				await db.SaveChangesAsync();
				return Ok(sondaggio);
			}
		}
		[Route("api/sondaggi/{id}")]
		[HttpDelete]
		public async Task<IHttpActionResult> Delete(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var sondaggio = await db.Sondaggi
					.FirstOrDefaultAsync(e => e.IdSondaggio == id);
				if (sondaggio == null)
					return NotFound();
				db.Sondaggi.Remove(sondaggio);
				await db.SaveChangesAsync();
				return Ok();
			}
		}

		[Route("api/sondaggi/")]
		[HttpPut]
		public async Task<IHttpActionResult> Put(int id, Sondaggio sondaggio)
		{
			if (id != sondaggio.IdSondaggio)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(sondaggio).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return Ok(sondaggio);
			}
		}

		[Route("api/sondaggi/{id:int}")]
		[HttpPatch]
		public async Task<IHttpActionResult> Patch(int id, Sondaggio sondaggio)
		{
			if (id != sondaggio.IdSondaggio)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(sondaggio).State = EntityState.Modified;
				var domande = sondaggio.ListaServizi.Split('\n').ToList();

				foreach (var domanda in domande)
				{
					if (sondaggio.Domande.All(a => a.TitoloDomanda != domanda))
					{
						var d = new Domanda {TitoloDomanda = domanda, IdSondaggio = sondaggio.IdSondaggio, Priorita = domande.IndexOf(domanda), dtAgg = DateTime.Now};
						sondaggio.Domande.Add(d);
						db.Entry(d).State = EntityState.Added;
					}
				}

				sondaggio.dtAgg = DateTime.Now;
				await db.SaveChangesAsync();
				return Ok(sondaggio);
			}
		}
	}
}
