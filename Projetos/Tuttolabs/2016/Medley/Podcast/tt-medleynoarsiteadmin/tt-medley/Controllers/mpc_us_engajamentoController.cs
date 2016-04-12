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
    public class mpc_us_engajamentoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mpc_us_engajamento
        public ActionResult Index()
        {
            return View(db.mpc_us_engajamento.ToList());
        }

        // GET: mpc_us_engajamento/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_us_engajamento mpc_us_engajamento = db.mpc_us_engajamento.Find(id);
            if (mpc_us_engajamento == null)
            {
                return HttpNotFound();
            }
            return View(mpc_us_engajamento);
        }

        // GET: mpc_us_engajamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mpc_us_engajamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,C__createdAt,C__updatedAt,C__version,C__deleted,usuario_id,acesso,audiencia,rating,respostas,notas,indice_calculado")] mpc_us_engajamento mpc_us_engajamento)
        {
            if (ModelState.IsValid)
            {
                db.mpc_us_engajamento.Add(mpc_us_engajamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mpc_us_engajamento);
        }

        // GET: mpc_us_engajamento/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_us_engajamento mpc_us_engajamento = db.mpc_us_engajamento.Find(id);
            if (mpc_us_engajamento == null)
            {
                return HttpNotFound();
            }
            return View(mpc_us_engajamento);
        }

        // POST: mpc_us_engajamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,C__createdAt,C__updatedAt,C__version,C__deleted,usuario_id,acesso,audiencia,rating,respostas,notas,indice_calculado")] mpc_us_engajamento mpc_us_engajamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mpc_us_engajamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mpc_us_engajamento);
        }

        // GET: mpc_us_engajamento/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_us_engajamento mpc_us_engajamento = db.mpc_us_engajamento.Find(id);
            if (mpc_us_engajamento == null)
            {
                return HttpNotFound();
            }
            return View(mpc_us_engajamento);
        }

        // POST: mpc_us_engajamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mpc_us_engajamento mpc_us_engajamento = db.mpc_us_engajamento.Find(id);
            db.mpc_us_engajamento.Remove(mpc_us_engajamento);
            db.SaveChanges();
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
