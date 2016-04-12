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
    public class mpc_empresasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mpc_empresas
        public ActionResult Index()
        {
            List<mpc_empresas> lista = new List<mpc_empresas>();

            foreach (var item in db.mpc_empresas)
            {
                if (item.C__deleted == false)
                {
                    lista.Add(item);
                }
            }
            return View(lista);
        }

        // GET: mpc_empresas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_empresas mpc_empresas = db.mpc_empresas.Find(id);
            if (mpc_empresas == null)
            {
                return HttpNotFound();
            }
            return View(mpc_empresas);
        }

        // GET: mpc_empresas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mpc_empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nome,descricao")] mpc_empresas mpc_empresas)
        {
            if (ModelState.IsValid)
            {
                if (mpc_empresas.nome == null)
                {
                    ViewBag.errorNameMessage = "Este campo é obrigatório";
                    return View();
                }
                db.mpc_empresas.Add(mpc_empresas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mpc_empresas);
        }

        // GET: mpc_empresas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_empresas mpc_empresas = db.mpc_empresas.Find(id);
            if (mpc_empresas == null)
            {
                return HttpNotFound();
            }
            return View(mpc_empresas);
        }

        // POST: mpc_empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,descricao")] mpc_empresas mpc_empresas, String id)
        {
            if (ModelState.IsValid)
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.Entry(mpc_empresas).State = EntityState.Modified;
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
            return View(mpc_empresas);
        }

        // GET: mpc_empresas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_empresas mpc_empresas = db.mpc_empresas.Find(id);
            if (mpc_empresas == null)
            {
                return HttpNotFound();
            }
            return View(mpc_empresas);
        }

        // POST: mpc_empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "id,C__deleted")] mpc_empresas mpc_empresas, String nome, String descricao, String id)
        {
            if (ModelState.IsValid)
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        mpc_empresas.nome = nome;
                        mpc_empresas.descricao = descricao;
                        mpc_empresas.C__deleted = true;
                        db.Entry(mpc_empresas).State = EntityState.Modified;
                        //db.SaveChanges();

                        List<mpc_grupos_us> mpc_grupos_us = new List<Models.mpc_grupos_us>();
                        mpc_grupos_us = db.mpc_grupos_us.Where(g => g.empresa_id == id).ToList();
                        foreach (var grupo in mpc_grupos_us)
                        {
                            grupo.C__deleted = true;
                            db.Entry(grupo).State = EntityState.Modified;
                        }
                        //db.Entry(mpc_grupos_us).State = EntityState.Modified;
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
