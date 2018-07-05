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
    builder.EntitySet<Sondaggio>("Sondaggi");
    builder.EntitySet<Domanda>("Domande"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SondaggiController : ODataController
    {
        private QuestionarioContext db = new QuestionarioContext();

        // GET: odata/Sondaggi
        [EnableQuery]
        public IQueryable<Sondaggio> GetSondaggi()
        {
            return db.Sondaggi;
        }

        // GET: odata/Sondaggi(5)
        [EnableQuery]
        public SingleResult<Sondaggio> GetSondaggio([FromODataUri] int key)
        {
            return SingleResult.Create(db.Sondaggi.Where(sondaggio => sondaggio.IdSondaggio == key));
        }

        // PUT: odata/Sondaggi(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Sondaggio> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sondaggio sondaggio = await db.Sondaggi.FindAsync(key);
            if (sondaggio == null)
            {
                return NotFound();
            }

            patch.Put(sondaggio);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SondaggioExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sondaggio);
        }

        // POST: odata/Sondaggi
        public async Task<IHttpActionResult> Post(Sondaggio sondaggio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sondaggi.Add(sondaggio);
            await db.SaveChangesAsync();

            return Created(sondaggio);
        }

        // PATCH: odata/Sondaggi(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Sondaggio> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sondaggio sondaggio = await db.Sondaggi.FindAsync(key);
            if (sondaggio == null)
            {
                return NotFound();
            }

            patch.Patch(sondaggio);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SondaggioExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sondaggio);
        }

        // DELETE: odata/Sondaggi(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Sondaggio sondaggio = await db.Sondaggi.FindAsync(key);
            if (sondaggio == null)
            {
                return NotFound();
            }

            db.Sondaggi.Remove(sondaggio);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Sondaggi(5)/Domande
        [EnableQuery]
        public IQueryable<Domanda> GetDomande([FromODataUri] int key)
        {
            return db.Sondaggi.Where(m => m.IdSondaggio == key).SelectMany(m => m.Domande);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SondaggioExists(int key)
        {
            return db.Sondaggi.Count(e => e.IdSondaggio == key) > 0;
        }
    }
}
