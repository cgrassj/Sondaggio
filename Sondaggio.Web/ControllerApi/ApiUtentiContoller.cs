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
using Questionario.Db.Services;

namespace Questionario.Web
{
	[Route("api/utenti")]
	public class ApiUtentiController : ApiController
	{
		private readonly ContextFactory _contextFactory;
		private readonly UtentiService _utentiService;

		public ApiUtentiController(ContextFactory contextFactory, UtentiService utentiService)
		{
			_contextFactory = contextFactory;
			_utentiService = utentiService;
		}

		[Route("api/utenti/")]
		public async Task<IHttpActionResult> Get()
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
				return Ok(await db.Utenti.ToListAsync());
		}

		[Route("api/utenti/{id}")]
		public async Task<IHttpActionResult> Get(string id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())

				return Ok(await _utentiService.GetUtente(id, db.Utenti).FirstOrDefaultAsync());
		}

		public async Task<IHttpActionResult> Post(Utente utente)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Utenti.Add(utente);
				await db.SaveChangesAsync();
				return Ok(utente);
			}
		}

		public async Task<IHttpActionResult> Delete(string id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var utente = await db.Utenti		
					.FirstOrDefaultAsync(e => e.IdUtente == id);
				if (utente == null)
					return NotFound();
				db.Utenti.Remove(utente);
				await db.SaveChangesAsync();
				return Ok();
			}
		}

		public async Task<IHttpActionResult> Put(string id, Utente utente)
		{
			if (id != utente.IdUtente)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(utente).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return Ok(utente);
			}
		}
		[Route("api/utenti/{id}")]
		[HttpPatch]
		public async Task<IHttpActionResult> Patch(string id, Utente cmp)
		{
			if (id != cmp.IdUtente)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(cmp).State = EntityState.Modified;
				cmp.dtAgg = DateTime.Now;
				await db.SaveChangesAsync();
				return Ok(cmp);
			}
		}
	}
}
