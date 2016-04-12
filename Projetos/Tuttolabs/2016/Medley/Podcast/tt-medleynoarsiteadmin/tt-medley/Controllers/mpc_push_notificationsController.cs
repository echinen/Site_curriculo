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
    public class mpc_push_notificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mpc_push_notifications
        public ActionResult Index()
        {
            return View(db.mpc_push_notifications.ToList());
        }

        // GET: mpc_push_notifications/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_push_notifications mpc_push_notifications = db.mpc_push_notifications.Find(id);
            if (mpc_push_notifications == null)
            {
                return HttpNotFound();
            }
            return View(mpc_push_notifications);
        }

        // GET: mpc_push_notifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mpc_push_notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,C__createdAt,C__updatedAt,C__version,C__deleted,dataenvio,titulo,mensagem,grupo_id")] mpc_push_notifications mpc_push_notifications)
        {
            if (ModelState.IsValid)
            {
                db.mpc_push_notifications.Add(mpc_push_notifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mpc_push_notifications);
        }

        // GET: mpc_push_notifications/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_push_notifications mpc_push_notifications = db.mpc_push_notifications.Find(id);
            if (mpc_push_notifications == null)
            {
                return HttpNotFound();
            }
            return View(mpc_push_notifications);
        }

        // POST: mpc_push_notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,C__createdAt,C__updatedAt,C__version,C__deleted,dataenvio,titulo,mensagem,grupo_id")] mpc_push_notifications mpc_push_notifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mpc_push_notifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mpc_push_notifications);
        }

        // GET: mpc_push_notifications/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_push_notifications mpc_push_notifications = db.mpc_push_notifications.Find(id);
            if (mpc_push_notifications == null)
            {
                return HttpNotFound();
            }
            return View(mpc_push_notifications);
        }

        // POST: mpc_push_notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mpc_push_notifications mpc_push_notifications = db.mpc_push_notifications.Find(id);
            db.mpc_push_notifications.Remove(mpc_push_notifications);
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
