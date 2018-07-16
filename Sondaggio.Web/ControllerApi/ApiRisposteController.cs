using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using Questionario.Db;
using Questionario.Db.Models;

namespace Questionario.Web
{
	[Route("api/risposte")]
	public class ApiRisposteController : ApiController
	{
		private readonly ContextFactory _contextFactory;

		public ApiRisposteController(ContextFactory contextFactory) => _contextFactory = contextFactory;

		[Route("api/risposte")]
		public async Task<IHttpActionResult> Get()
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var risposte = db
					.Risposte
					.Include(e => e.Domanda)
					.Include(e => e.Utente)
					.Include(e => e.Domanda.Sondaggio);

				return Ok(await risposte.ToListAsync());
			}
		}

		[Route("api/risposte/{id}")]
		public async Task<IHttpActionResult> Get(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var risposta = await db
					.Risposte
					.Include(e => e.Domanda)
					.Include(e => e.Utente)
					.Include(e => e.Domanda.Sondaggio)
					.FirstOrDefaultAsync(e => e.IdRisposta == id);
				if (risposta == null)
					return NotFound();
				return Ok(risposta);
			}
		}

		public async Task<IHttpActionResult> Post(Risposta risposta)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Risposte.Add(risposta);
				await db.SaveChangesAsync();
				return Ok(risposta);
			}
		}

		[Route("api/risposte/{idDomanda}/{idUtente}")]
		public async Task<IHttpActionResult> Post(int IdDomanda, string IdUtente)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				Risposta risposta = new Risposta();
				risposta.IdDomanda = IdDomanda;
				risposta.IdUtente = IdUtente;
				risposta.StelleRisposta = -1;
				db.Risposte.Add(risposta);
				await db.SaveChangesAsync();
				return Ok(risposta);
			}
		}

		public async Task<IHttpActionResult> Delete(int id)
		{
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				var risposta = await db.Risposte.FirstOrDefaultAsync(e => e.IdRisposta == id);
				if (risposta == null)
					return NotFound();
				db.Risposte.Remove(risposta);
				await db.SaveChangesAsync();
				return Ok();
			}
		}

		[Route("api/risposte/{id:int}")]
		[HttpPut]
		public async Task<IHttpActionResult> Put(int id, Risposta risposta)
		{
			if (id != risposta.IdRisposta)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(risposta).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return Ok(risposta);
			}
		}

		[Route("api/risposte/{id:int}")]
		[HttpPatch]
		public async Task<IHttpActionResult> Patch(int id, Risposta risposta)
		{
			if (id != risposta.IdRisposta)
				return BadRequest();
			using (var db = _contextFactory.GetContext<QuestionarioContext>())
			{
				db.Entry(risposta).State = EntityState.Modified;
				risposta.dtAgg = DateTime.Now;
				await db.SaveChangesAsync();
				return Ok(risposta);
			}
		}

		public void SendMail(string cognomeNome, string sottoTitoloSondaggio, string descrizioneSondaggio)
		{
			MailMessage m = new MailMessage();
			SmtpClient client = new SmtpClient();
			client.Port = 25;
			client.Host = "smtp.cgil.lombardia.it";
			m.Subject = "oggetto email";
			m.Body = TestoEmail.Replace("CognomeNome", cognomeNome).Replace("SottoTitoloSondaggio", sottoTitoloSondaggio).Replace("DescrizioneSondaggio", descrizioneSondaggio);
			client.Send(m);
		}

		protected const string TestoEmail = "<div>Gentile {{CognomeNome}}</div><div>vorremmo ringraziarLa per averci scelto e vorremmo invitarLa a rispondere ad un breve sondaggio per aiutarci a migliorare il nostro servizio</div><div>&nbsp;</div><div>La preghiamo di accedere al seguente <a href='{{URL}}'>link</a></div><div>&nbsp;</div><div>&nbsp;</div><div>&nbsp;</div><div>&nbsp;</div><div><div>Cordialmente:</div>{{SottoTitoloSondaggio}} {{DescrizioneSondaggio}}</div>";

	}
}
