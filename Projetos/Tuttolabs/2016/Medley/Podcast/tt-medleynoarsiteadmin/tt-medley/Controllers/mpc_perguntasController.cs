using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tt_medley.Models;

namespace tt_medley.Controllers
{
    public class mpc_perguntasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mpc_perguntas
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, mpc_perguntas ppd)
        {
            ViewBag.podcast = db.mpc_podcasts.ToList();
            var querySearch = from pe in db.mpc_perguntas select pe;
            if (!String.IsNullOrEmpty(searchString))
            {
                querySearch = from pe in db.mpc_perguntas join p in db.mpc_podcasts on new { A = pe.podcast_id } equals new { A = p.id } where p.nome.Contains(searchString) select pe;
                return View(querySearch.ToList());
            }
            
            return View(db.mpc_perguntas.ToList());
        }

        // GET: mpc_perguntas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_perguntas mpc_perguntas = db.mpc_perguntas.Find(id);
            if (mpc_perguntas == null)
            {
                return HttpNotFound();
            }
            return View(mpc_perguntas);
        }

        // GET: mpc_perguntas/Create
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                ViewBag.idPod = null;
                ViewBag.podcast = db.mpc_podcasts.ToList();
            }
            else
            {
                mpc_podcasts pod = db.mpc_podcasts.Find(id);
                ViewBag.idPod = pod;
                ViewBag.podcast = null;
            }
            
            return View();
        }

        // POST: mpc_perguntas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pergunta,resp1,resp2,resp3,resp4,resp_certa,podcast_id")] mpc_perguntas mpc_perguntas, string selectPodcast, string respCerta)
        {
            if (String.IsNullOrEmpty(mpc_perguntas.podcast_id))
            {
                mpc_perguntas.podcast_id = selectPodcast;
            }
            if (ModelState.IsValid)
            {
                //if (mpc_perguntas.pergunta == null || mpc_perguntas.resp1 == null || mpc_perguntas.resp2 == null || mpc_perguntas.resp3 == null || mpc_perguntas.resp4 == null)
                //{
                //    ViewBag.podcast = db.mpc_podcasts.ToList();
                //    ViewBag.errorMsg = "Este campo é obrigatório";
                //    return View();
                //}
                //ViewBag.podcast = db.mpc_podcasts.ToList();

                mpc_perguntas.resp_certa = Double.Parse(respCerta);
                db.mpc_perguntas.Add(mpc_perguntas);
                db.SaveChanges();
                mpc_perguntas = new mpc_perguntas();
                return RedirectToAction("Create");
            }

            return View(mpc_perguntas);
        }

        // GET: mpc_perguntas/Edit/5
        public ActionResult Edit(string id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_perguntas mpc_perguntas = db.mpc_perguntas.Find(id);

            // Busca o nome do podcast com base no id relacionado a pergunta
            var getPodName = db.mpc_podcasts.Where(x => x.id == mpc_perguntas.podcast_id).ToList();
            ViewBag.podId = getPodName[0].nome;

            if (mpc_perguntas == null)
            {
                return HttpNotFound();
            }
            return View(mpc_perguntas);
        }

        // POST: mpc_perguntas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,podcast_id,pergunta,resp1,resp2,resp3,resp4,resp_certa")] mpc_perguntas mpc_perguntas, String id, String podcast_id)
        {
            var pod = from p in db.mpc_podcasts where p.id == podcast_id select p.nome;
            ViewBag.podId = pod;
            if (ModelState.IsValid)
            {
                bool saveFailed;
                
                do
                {
                    saveFailed = false;
                    try
                    {
                        mpc_perguntas.podcast_id = podcast_id;
                        db.Entry(mpc_perguntas).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Update original values from the database 
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }

                } while (saveFailed);
                return RedirectToAction("Index");
            }
            return View(mpc_perguntas);
        }

        // GET: mpc_perguntas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_perguntas mpc_perguntas = db.mpc_perguntas.Find(id);
            if (mpc_perguntas == null)
            {
                return HttpNotFound();
            }
            return View(mpc_perguntas);
        }

        // POST: mpc_perguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mpc_perguntas mpc_perguntas = db.mpc_perguntas.Find(id);
            db.mpc_perguntas.Remove(mpc_perguntas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //HELPER Function
        public string getPodcast(string id)
        {
            if (id == null)
            {
                return "Nulo";
            }
            else
            {
                return db.mpc_podcasts.SingleOrDefault(d => d.id == id).nome;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
