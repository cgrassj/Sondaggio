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
using System.Web.OData;
using System.Web.Http.OData.Routing;
using Questionario.Db;
using Questionario.Db.Models;
using Questionario.Db.Services;

namespace Questionario.Web.Controller
{
    /*
    Per aggiungere una route relativa a questo controller, può essere necessario apportare altre modifiche alla classe WebApiConfig. Unire queste istruzioni nel metodo Register della classe WebApiConfig. Tenere presente che per gli URL OData viene fatta distinzione tra maiuscole e minuscole.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Questionario.Db.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Utente>("Utenti");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UtentiController : ODataController
    {
	    private readonly UtentiService _utentiService;
	    private QuestionarioContext db = new QuestionarioContext();

		//public UtentiController(UtentiService utentiService)
		//{
		//	_utentiService = utentiService;
		//}

		// GET: odata/Utenti
		[EnableQuery]
        public IQueryable<Utente> GetUtenti()
        {
            return db.Utenti;
        }

        // GET: odata/Utenti(5)
        [EnableQuery]
        public SingleResult<Utente> GetUtente([FromODataUri] string key)
		{
			return SingleResult.Create(db.Utenti.Where(utente => utente.IdUtente == key));
		}
		 
		//public SingleResult<Utente> GetUtente([FromODataUri] string key)
		//{
		//	return SingleResult.Create(_utentiService.GetUtente(key, db.Utenti));
		//}
 
		

		// PUT: odata/Utenti(5)
		public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Utente> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Utente utente = await db.Utenti.FindAsync(key);
            if (utente == null)
            {
                return NotFound();
            }

            patch.Put(utente);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtenteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(utente);
        }

        // POST: odata/Utenti
        public async Task<IHttpActionResult> Post(Utente utente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Utenti.Add(utente);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UtenteExists(utente.IdUtente))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(utente);
        }

        // PATCH: odata/Utenti(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Utente> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Utente utente = await db.Utenti.FindAsync(key);
            if (utente == null)
            {
                return NotFound();
            }

            patch.Patch(utente);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtenteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(utente);
        }

        // DELETE: odata/Utenti(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Utente utente = await db.Utenti.FindAsync(key);
            if (utente == null)
            {
                return NotFound();
            }

            db.Utenti.Remove(utente);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UtenteExists(string key)
        {
            return db.Utenti.Count(e => e.IdUtente == key) > 0;
        }
    }
}
