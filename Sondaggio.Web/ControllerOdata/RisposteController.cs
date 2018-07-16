using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Questionario.Db;
using Questionario.Db.Models;
using System.Net.Mail;

namespace Questionario.Web.Controller
{
	
		public class RisposteController : ODataController
		{
			private readonly QuestionarioContext _db;

			public RisposteController(ContextFactory contextFactory)
			{
				_db = contextFactory.GetContext<QuestionarioContext>();
			}

			// GET: odata/Risposte
			[EnableQuery]
			public IQueryable<Risposta> GetRisposte()
			{
				return _db.Risposte;
			}

			// GET: odata/Risposte(5)
			[EnableQuery]
			[Route("odata/risposte/{id}")]
			public SingleResult<Risposta> GetRisposta(int key)
				{
					return SingleResult.Create(_db.Risposte.Where(b => b.IdRisposta == key));
				}

			// PATCH: odata/Risposte(5)
			[AcceptVerbs("PATCH", "MERGE")]
				public async Task<IHttpActionResult> Patch(int key, Delta<Risposta> patch)
				{
					if (!ModelState.IsValid)
					{
						return BadRequest(ModelState);
					}

					var risposta = await _db.Risposte.FindAsync(key);
					if (risposta == null)
					{
						return NotFound();
					}

					patch.Patch(risposta);
					_db.Entry(risposta).State = EntityState.Modified;

					await _db.SaveChangesAsync();

					return Updated(risposta);
				}

			// POST: odata/Risposte
			public async Task<IHttpActionResult> Post(Risposta risposta)
				{
					if (!ModelState.IsValid)
					{
						return BadRequest(ModelState);
					}

					_db.Risposte.Add(risposta);
					await _db.SaveChangesAsync();

					return Created(risposta);
				}

			// DELETE: odata/Risposte(5)
			public async Task<IHttpActionResult> Delete(int key)
				{
					var entity = await _db.Risposte.FindAsync(key);
					if (entity == null)
					{
						return NotFound();
					}

					_db.Risposte.Remove(entity);
					await _db.SaveChangesAsync();

					return StatusCode(HttpStatusCode.OK);
				}

			protected override void Dispose(bool disposing)
				{
					if (disposing)
					{
						_db.Dispose();
					}
					base.Dispose(disposing);
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