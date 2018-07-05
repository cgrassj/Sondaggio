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
	[Route("api/sondaggi")]
	public class ApiSondaggiController : ApiController
	{
		private readonly ContextFactory _contextFactory;

		public ApiSondaggiController(ContextFactory contextFactory) => _contextFactory = contextFactory;

		public async Task<IHttpActionResult> Get()
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
				return Ok(await db.Sondaggi.ToListAsync());
		}

		[Route("api/sondaggi/{id}")]
		public async Task<IHttpActionResult> Get(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var sondaggio = await db.Sondaggi.FirstOrDefaultAsync(e => e.IdSondaggio == id);
				if (sondaggio == null)
					return NotFound();
				return Ok(sondaggio);
			}
		}

		public async Task<IHttpActionResult> Post(Sondaggio sondaggio)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Sondaggi.Add(sondaggio);
				await db.SaveChangesAsync();
				return Ok(sondaggio);
			}
		}

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

	}
}