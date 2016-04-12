using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tt_medley.Models;

namespace tt_medley.Controllers
{
    public class JsonController : Controller
    {

         private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Json
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult jsonOne(){
            List<grafico> data = new List<grafico>();

            data.Add(new grafico{ campo = "Charlotte, NC", valor = 15});

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult jsonMap()
        {
            List<grafico> data = new List<grafico>();

            data.Add(new grafico { campo = "Brazil", valor = 700 });
            data.Add(new grafico { campo = "Germany", valor = 1300 });
            data.Add(new grafico { campo = "China", valor = 300 });
            data.Add(new grafico { campo = "England", valor = 800 });
            data.Add(new grafico { campo = "United States", valor = 2200 });

            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }

  

    public class grafico
    {
       public string campo { get; set; }
       public double valor { get; set; }
       // public string tittle { get; set; }
    }
}