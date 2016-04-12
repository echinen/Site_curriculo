using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tt_medley.Models;

namespace tt_medley.Controllers
{
    public class mpc_us_acoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public string GetUsuario(string id) {

            if (id == null)
            {
                return "Nulo";
            }
            else
            {
                if(id.Length <= 2)
                {
                    return "Nulo";
                }
                else
                {
                    return db.mpc_usuarios.SingleOrDefault(d => d.id == id).nome;
                }
             
            }
        }

        public string GetCategoria(string id)
        {

            if (id == null)
            {
                return "Nulo";
            }
            else
            {
                return db.mpc_categorias.SingleOrDefault(d => d.id == id).nome;
            }
        }

        public string GetPodcast(string id)
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

        // GET: mpc_us_acoes
        public ActionResult Index(string dtInicio, string dtFim, string searchStringPodcast, string searchStringCategoria, string currentFilterCategoria, string currentFilter, string searchString, int? page, mpc_us_acoesRel act)
        {
            ViewBag.dropCategory = db.mpc_categorias.ToList();
            //int pageSize = 50;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            string sqlquery = "select b.nome as usuario, a.dataacao,a.login,a.ouviu, c.nome as categoria, d.nome as audio from tt_medley_podcast.mpc_us_acoes a LEFT JOIN tt_medley_podcast.mpc_usuarios b on a.usuario_id = b.id LEFT JOIN tt_medley_podcast.mpc_categorias c on a.categoria_id = c.id LEFT JOIN tt_medley_podcast.mpc_podcasts d on a.audio_id = d.id where 1 = 1 ";
            object[] myObjArray = new object[1];
    
            if (!String.IsNullOrEmpty(searchString))
            {
                sqlquery = sqlquery + " AND b.nome like '%" + searchString + "%'";
            }
            if (!String.IsNullOrEmpty(searchStringCategoria))
            {
                sqlquery = sqlquery + " AND c.nome like '%" + searchStringCategoria + "%'";
            }
            if (!String.IsNullOrEmpty(searchStringPodcast))
            {
                sqlquery = sqlquery + " AND d.nome like '%" + searchStringPodcast + "%'";
            }
            if (!String.IsNullOrEmpty(dtInicio))
            {
                dtInicio = dtInicio.Replace("/", "-");
                DateTime dataIni = new DateTime();
                dataIni = DateTime.ParseExact(dtInicio, "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.CurrentCulture);
                
                sqlquery = sqlquery + " AND a.dataacao >= '" + String.Format("{0:yyyy/MM/dd}", dataIni) + "'";
            }
            if (!String.IsNullOrEmpty(dtFim))
            {
                dtFim = dtFim.Replace("/", "-");
                DateTime dataFim = new DateTime();
                dataFim = DateTime.ParseExact(dtFim, "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.CurrentCulture);
                sqlquery = sqlquery + " AND a.dataacao <= '" + String.Format("{0:yyyy/MM/dd}", dataFim.AddDays(1)) + "'";
            }

            sqlquery = sqlquery + " order by a.dataacao";

            if (!String.IsNullOrEmpty(searchString) || !String.IsNullOrEmpty(searchStringCategoria) || !String.IsNullOrEmpty(searchStringPodcast) || !String.IsNullOrEmpty(dtInicio) || !String.IsNullOrEmpty(dtFim))
            {
               
                DbRawSqlQuery<mpc_us_acoesRel> data = db.Database.SqlQuery<mpc_us_acoesRel>(sqlquery, myObjArray);

                return View(data.ToList());
            }

            return View();
        }

        // GET: mpc_us_acoes/Details/5 
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_us_acoes mpc_us_acoes = db.mpc_us_acoes.Find(id);
            if (mpc_us_acoes == null)
            {
                return HttpNotFound();
            }
            return View(mpc_us_acoes);
        }

        // GET: mpc_us_acoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mpc_us_acoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,C__createdAt,C__updatedAt,C__version,C__deleted,usuario_id,dataacao,login,ouviu,categoria_id,audio_id")] mpc_us_acoes mpc_us_acoes)
        {
            if (ModelState.IsValid)
            {
                db.mpc_us_acoes.Add(mpc_us_acoes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mpc_us_acoes);
        }

        // GET: mpc_us_acoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_us_acoes mpc_us_acoes = db.mpc_us_acoes.Find(id);
            if (mpc_us_acoes == null)
            {
                return HttpNotFound();
            }
            return View(mpc_us_acoes);
        }

        // POST: mpc_us_acoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,C__createdAt,C__updatedAt,C__version,C__deleted,usuario_id,dataacao,login,ouviu,categoria_id,audio_id")] mpc_us_acoes mpc_us_acoes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mpc_us_acoes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mpc_us_acoes);
        }

        // GET: mpc_us_acoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_us_acoes mpc_us_acoes = db.mpc_us_acoes.Find(id);
            if (mpc_us_acoes == null)
            {
                return HttpNotFound();
            }
            return View(mpc_us_acoes);
        }

        // POST: mpc_us_acoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mpc_us_acoes mpc_us_acoes = db.mpc_us_acoes.Find(id);
            db.mpc_us_acoes.Remove(mpc_us_acoes);
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
