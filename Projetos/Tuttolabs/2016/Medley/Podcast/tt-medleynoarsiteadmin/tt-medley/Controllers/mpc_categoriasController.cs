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
    public class mpc_categoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mpc_categorias
        public ActionResult Index()
        {
            List<mpc_categorias> lista = new List<mpc_categorias>();

            foreach (var item in db.mpc_categorias)
            {
                if(item.C__deleted == false)
                {
                    lista.Add(item);
                }
            }
            return View(lista);
        }

        // GET: mpc_categorias/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_categorias mpc_categorias = db.mpc_categorias.Find(id);
            if (mpc_categorias == null)
            {
                return HttpNotFound();
            }
            return View(mpc_categorias);
        }

        // GET: mpc_categorias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mpc_categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nome,descricao")] mpc_categorias mpc_categorias)
        {
            if (ModelState.IsValid)
            {
                if(mpc_categorias.nome == null)
                {
                    ViewBag.errorMessageNome = "Este campo e obrigatorio";
                    return View();
                }
                else
                {
                    db.mpc_categorias.Add(mpc_categorias);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }

            return View(mpc_categorias);
        }

        // GET: mpc_categorias/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_categorias mpc_categorias = db.mpc_categorias.Find(id);
            if (mpc_categorias == null)
            {
                return HttpNotFound();
            }
            return View(mpc_categorias);
        }

        // POST: mpc_categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,descricao")] mpc_categorias mpc_categorias, String id)
        {
            if (ModelState.IsValid)
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.Entry(mpc_categorias).State = EntityState.Modified;
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
            }
            return RedirectToAction("Index");
        }

        // GET: mpc_categorias/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_categorias mpc_categorias = db.mpc_categorias.Find(id);            
            if (mpc_categorias == null)
            {
                return HttpNotFound();
            }
            return View(mpc_categorias);
        }

        // POST: mpc_categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "id,C__deleted")] mpc_categorias mpc_categorias, String nome, String descricao, String id)
        {
            if (ModelState.IsValid)
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        mpc_categorias.nome = nome;
                        mpc_categorias.descricao = descricao;
                        mpc_categorias.C__deleted = true;
                        db.Entry(mpc_categorias).State = EntityState.Modified;
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
            }
            return RedirectToAction("Index");
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
