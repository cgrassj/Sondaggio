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

namespace Questionario.Web.Controller
{
    /*
    Per aggiungere una route relativa a questo controller, può essere necessario apportare altre modifiche alla classe WebApiConfig. Unire queste istruzioni nel metodo Register della classe WebApiConfig. Tenere presente che per gli URL OData viene fatta distinzione tra maiuscole e minuscole.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Questionario.Db.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Domanda>("Domande");
    builder.EntitySet<Risposta>("Risposte"); 
    builder.EntitySet<Sondaggio>("Sondaggi"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DomandeController : ODataController
    {
        private QuestionarioContext db = new QuestionarioContext();

        // GET: odata/Domande
        [EnableQuery]
        public IQueryable<Domanda> GetDomande()
        {
            return db.Domande;
        }

        // GET: odata/Domande(5)
        [EnableQuery]
        public SingleResult<Domanda> GetDomanda([FromODataUri] int key)
        {
            return SingleResult.Create(db.Domande.Where(domanda => domanda.IdDomanda == key));
        }

        // PUT: odata/Domande(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Domanda> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Domanda domanda = await db.Domande.FindAsync(key);
            if (domanda == null)
            {
                return NotFound();
            }

            patch.Put(domanda);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomandaExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(domanda);
        }

        // POST: odata/Domande
        public async Task<IHttpActionResult> Post(Domanda domanda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Domande.Add(domanda);
            await db.SaveChangesAsync();

            return Created(domanda);
        }

        // PATCH: odata/Domande(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Domanda> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Domanda domanda = await db.Domande.FindAsync(key);
            if (domanda == null)
            {
                return NotFound();
            }

            patch.Patch(domanda);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomandaExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(domanda);
        }

        // DELETE: odata/Domande(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Domanda domanda = await db.Domande.FindAsync(key);
            if (domanda == null)
            {
                return NotFound();
            }

            db.Domande.Remove(domanda);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Domande(5)/Risposte
        [EnableQuery]
        public IQueryable<Risposta> GetRisposte([FromODataUri] int key)
        {
            return db.Domande.Where(m => m.IdDomanda == key).SelectMany(m => m.Risposte);
        }

        // GET: odata/Domande(5)/Sondaggio
        [EnableQuery]
        public SingleResult<Sondaggio> GetSondaggio([FromODataUri] int key)
        {
            return SingleResult.Create(db.Domande.Where(m => m.IdDomanda == key).Select(m => m.Sondaggio));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DomandaExists(int key)
        {
            return db.Domande.Count(e => e.IdDomanda == key) > 0;
        }
    }
}
