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
	[Route("api/domande")]
	public class ApiDomandeController : ApiController
	{
		private readonly ContextFactory _contextFactory;

		public ApiDomandeController(ContextFactory contextFactory) => _contextFactory = contextFactory;

		public async Task<IHttpActionResult> Get()
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var domande = db.Domande.Include(e => e.Sondaggio).Include(e => e.Risposte)
					.Include(e => e.Risposte.Select(r => r.Utente)).ToList();
				return Ok(domande);
			}
		}

		[Route("api/updateDomande/{idSondaggio}")]
		public async Task<IHttpActionResult> GetDomande(int idSondaggio)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var domande = db.Domande.Where(e => e.IdSondaggio== idSondaggio);
				if (domande == null)
					return NotFound();
				return Ok(await domande.ToListAsync());
			}
		}
		[Route("api/domande/{id}")]
		public async Task<IHttpActionResult> Get(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var domanda = await db.Domande
					.Include(e => e.Sondaggio).Include(e => e.Risposte)
					.FirstOrDefaultAsync(e => e.IdDomanda == id);
				if (domanda == null)
					return NotFound();
				return Ok(domanda);
			}
		}

		public async Task<IHttpActionResult> Post(Domanda domanda)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Domande.Add(domanda);
				await db.SaveChangesAsync();
				return Ok(domanda);
			}
		}

		public async Task<IHttpActionResult> Delete(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var domanda = await db.Domande
					.FirstOrDefaultAsync(e => e.IdDomanda == id);
				if (domanda == null)
					return NotFound();
				db.Domande.Remove(domanda);
				await db.SaveChangesAsync();
				return Ok();
			}
		}

		public async Task<IHttpActionResult> Put(int id, Domanda domanda)
		{
			if (id != domanda.IdDomanda)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(domanda).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return Ok(domanda);
			}
		}

	}
}