using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tt_medley.Models;
using PagedList;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell;
using System.Globalization;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;

namespace tt_medley.Controllers
{
    public class mpc_podcastsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public static List<mpc_podcasts> globalPodcasts = new List<mpc_podcasts>();
        // GET: mpc_podcasts
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, mpc_podcasts pd)
        {
            ViewBag.CurrentSort = sortOrder;
            //variáveis que definirão se o sort é ascending ou descending
            ViewBag.NomeSortOrder = String.IsNullOrEmpty(sortOrder) ? "nome_desc" : "";
            ViewBag.DescricaoSortOrder = sortOrder == "descricao" ? "descricao_desc" : "descricao";
            ViewBag.PatrocinadorSortOrder = sortOrder == "patrocinador" ? "patrocinador_desc" : "patrocinador";
            ViewBag.DataSortOrder = sortOrder == "datalancamento" ? "datalancmaneto_desc" : "datalancamento";
            ViewBag.TempoSortOrder = sortOrder == "tempo" ? "tempo_desc" : "tempo";
            ViewBag.CategoriaSortOrder = sortOrder == "categoria" ? "categoria_desc" : "categoria";
            ViewBag.GrupoSortOrder = sortOrder == "grupo" ? "grupo_desc" : "grupo";



            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var podcasts = from p in db.mpc_podcasts
                           select p;


            if (!String.IsNullOrEmpty(searchString))
            {
                podcasts = podcasts.Where(p => p.nome.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "nome_desc":
                    podcasts = podcasts.OrderByDescending(p => p.nome);
                    break;
                case "descricao":
                    podcasts = podcasts.OrderBy(p => p.descricao);
                    break;
                case "descricao_desc":
                    podcasts = podcasts.OrderByDescending(p => p.descricao);
                    break;
                case "categoria":
                    podcasts = from p in db.mpc_podcasts
                               join c in db.mpc_categorias on p.categoria_id equals c.id
                               orderby c.nome ascending
                               select p;
                    break;
                case "categoria_desc":
                    podcasts = from p in db.mpc_podcasts
                               join c in db.mpc_categorias on p.categoria_id equals c.id
                               orderby c.nome descending
                               select p;
                    break;
                case "grupo":
                    podcasts = from p in db.mpc_podcasts
                               join g in db.mpc_grupos on p.grupo_id equals g.id
                               orderby g.nome ascending
                               select p;
                    break;
                case "grupo_desc":
                    podcasts = from p in db.mpc_podcasts
                               join g in db.mpc_grupos on p.grupo_id equals g.id
                               orderby g.nome descending
                               select p;
                    break;
                case "patrocinador":
                    podcasts = podcasts.OrderBy(p => p.patrocinador);
                    break;
                case "patrocinador_desc":
                    podcasts = podcasts.OrderByDescending(p => p.patrocinador);
                    break;
                case "datalancamento":
                    podcasts = podcasts.OrderBy(p => p.datalancamento);
                    break;
                case "datalancamento_desc":
                    podcasts = podcasts.OrderByDescending(p => p.datalancamento);
                    break;
                case "tempo":
                    podcasts = podcasts.OrderBy(p => p.tempo);
                    break;
                case "tempo_desc":
                    podcasts = podcasts.OrderByDescending(p => p.tempo);
                    break;
                default:
                    podcasts = podcasts.OrderBy(p => p.nome);
                    break;
            }


            /*StreamWriter write = new StreamWriter(Server.MapPath(@"teste.csv"));
            CsvWriter csw = new CsvWriter(write);
            foreach (var record in db.mpc_podcasts.ToList())
            {
                csw.WriteRecord<mpc_podcasts>(record);
            }
            write.Close();*/
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            globalPodcasts = podcasts.ToList();
            return View(podcasts.ToPagedList(pageNumber, pageSize));
            //return View(podcasts.ToList());
            //return View(db.mpc_podcasts.ToList());
        }

        // GET: mpc_podcasts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_podcasts mpc_podcasts = db.mpc_podcasts.Find(id);
            if (mpc_podcasts == null)
            {
                return HttpNotFound();
            }
            return View(mpc_podcasts);
        }

        // GET: mpc_podcasts/Create
        public ActionResult Create()
        {
            ViewBag.categorias = db.mpc_categorias.ToList();
            ViewBag.grupos = db.mpc_grupos.ToList();
            return View();
        }

        // POST: mpc_podcasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nome,descricao,patrocinador,datalancamento,tempo")] mpc_podcasts mpc_podcasts, string selectedCategory, string selectedGroup, HttpPostedFileBase file)
        {
            var verificationName = db.mpc_podcasts.ToList();
            String path = null;
            var flag = false;
            string nameNoSpace = "";
            var able = false;

            if (db.mpc_podcasts.Where(p => p.C__deleted == false).FirstOrDefault(p => p.nome == mpc_podcasts.nome) != null)
            {
                flag = true;
            }
            /*foreach (var item in verificationName)
            {
                if (item.nome == mpc_podcasts.nome)
                {
                    flag = true;
                }
            }*/

            if (flag == true)
            {
                ViewBag.categorias = db.mpc_categorias.ToList();
                ViewBag.grupos = db.mpc_grupos.ToList();
                ViewBag.errorMessagePodcast = "Este nome de podcast já está sendo utilizado";
                return View();
            }
            else {
                bool isSavedSuccessfully = true;
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename


                    var fileName = Path.GetFileName(file.FileName);
                    var fileEX = Path.GetExtension(file.FileName);
                    var newName = "" + mpc_podcasts.nome + fileEX + "";



                    if (fileEX != ".mp3" || fileEX == null)
                    {
                        ViewBag.categorias = db.mpc_categorias.ToList();
                        ViewBag.grupos = db.mpc_grupos.ToList();
                        ViewBag.errorUploadMessage = "Formato de arquivo e invalido, por favor selecione um arquivo com a extensao .mp3";
                        return View();
                    }
                    else
                    {
                        try
                        {
                            nameNoSpace = newName.Replace(" ", "");
                            // store the file inside ~/App_Data/podcasts folder
                            path = Path.Combine(Server.MapPath("~/Podcasts"), nameNoSpace);
                            file.SaveAs(path);
                        }
                        catch (Exception ex)
                        {
                            isSavedSuccessfully = false;
                        }

                        /*  if (isSavedSuccessfully)
                          {
                              return Json(new { Message = nameNoSpace });
                          }
                          else
                          {
                              return Json(new { Message = "Error in saving file" });
                          }*/


                        if (mpc_podcasts.tempo == null)
                        {
                            //Renzo 2016-01-19 get audio file length
                            ShellFile so = ShellFile.FromFilePath(path);
                            double nanoseconds;
                            double.TryParse(so.Properties.System.Media.Duration.Value.ToString(),
                                out nanoseconds);

                            if (nanoseconds > 0)
                            {
                                int totalSeconds = Convert.ToInt32(nanoseconds / 10000000);
                                if (totalSeconds > 59)
                                {
                                    int minutes = totalSeconds / 60;
                                    int seconds = totalSeconds % 60;
                                    Console.WriteLine("=============================> " + minutes + ":" + seconds);
                                    if (seconds < 10)
                                    {
                                        mpc_podcasts.tempo = minutes + ":0" + seconds;
                                    }
                                    else {
                                        mpc_podcasts.tempo = minutes + ":" + seconds;
                                    }

                                }

                            }
                            //Renzo end
                        }
                    }

                }
                else
                {
                    ViewBag.errorUploadMessage = "Este campo é obrigatório, por favor selecione um arquivo com a extensão .mp3";
                    ViewBag.categorias = db.mpc_categorias.ToList();
                    ViewBag.grupos = db.mpc_grupos.ToList();
                    return View();
                }
                //Pegar grupo_id, categoria_id e audio_url



                if (path == null)
                {
                    mpc_podcasts.audio_url = "Formato de arquivo não suportado";
                }
                else
                {
                    var url = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/");
                    url = url.Replace("http:", "https:");

                    var audioName = path.Replace(" ", "");

                    string lastWord = audioName.Split('\\').Last();

                    mpc_podcasts.audio_url = url + "Podcasts/" + lastWord;
                    ModelState["audio_url"].Errors.Clear();
                }

                mpc_podcasts.grupo_id = selectedGroup;
                mpc_podcasts.categoria_id = selectedCategory;
                ModelState["grupo_id"].Errors.Clear();
                ModelState["categoria_id"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    mpc_podcasts.habilitado = able;
                    db.mpc_podcasts.Add(mpc_podcasts);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    TempData["AlertMessage"] = "Erro ao criar podcast: " + errors.ToList()[0].ErrorMessage.ToString();
                    ViewBag.categorias = db.mpc_categorias.ToList();
                    ViewBag.grupos = db.mpc_grupos.ToList();
                    return View(mpc_podcasts);
                }
            }
        }

        // GET: mpc_podcasts/Edit/5
        public ActionResult Edit(string id)
        {

            ViewBag.categorias = db.mpc_categorias.ToList();
            ViewBag.grupos = db.mpc_grupos.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_podcasts mpc_podcasts = db.mpc_podcasts.Find(id);
            if (mpc_podcasts == null)
            {
                return HttpNotFound();
            }
            return View(mpc_podcasts);
        }

        // POST: mpc_podcasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,descricao,patrocinador,datalancamento,tempo,audio_url")] mpc_podcasts mpc_podcasts, string categoria_id, string grupo_id, HttpPostedFileBase file, String id, string audio_url, string nome, string tempo)
        {

            ViewBag.categorias = db.mpc_categorias.ToList();
            ViewBag.grupos = db.mpc_grupos.ToList();
            String path = null;


            if (ModelState.IsValid)
            {
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename
                    var fileName = Path.GetFileName(file.FileName);
                    var fileEX = Path.GetExtension(file.FileName);
                    var newName = "" + mpc_podcasts.nome + fileEX + "";

                    if (fileEX != ".mp3" || fileEX == null)
                    {
                        ViewBag.categorias = db.mpc_categorias.ToList();
                        ViewBag.grupos = db.mpc_grupos.ToList();
                        ViewBag.errorUploadMessage = "Formato de arquivo e invalido, por favor selecione um arquivo com a extensao .mp3";
                        return View();
                    }
                    else
                    {
                        var nameNoSpace = newName.Replace(" ", "");
                        // store the file inside ~/App_Data/podcasts folder
                        path = Path.Combine(Server.MapPath("~/Podcasts"), nameNoSpace);
                        file.SaveAs(path);
                    }

                }

                if (path != null)
                {
                    var url = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/");
                    url = url.Replace("http:", "https:");

                    var audioName = path.Replace(" ", "");

                    string lastWord = audioName.Split('\\').Last();

                    mpc_podcasts.audio_url = url + "Podcasts/" + lastWord;
                }

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        mpc_podcasts.categoria_id = categoria_id;
                        mpc_podcasts.grupo_id = grupo_id;
                        db.Entry(mpc_podcasts).State = EntityState.Modified;
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

        // GET: mpc_podcasts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_podcasts mpc_podcasts = db.mpc_podcasts.Find(id);
            if (mpc_podcasts == null)
            {
                return HttpNotFound();
            }
            return View(mpc_podcasts);
        }

        //Download files

        public ActionResult downloadFile(string uri)
        {
            string fname = "export";
            var url = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/");
            url = url.Replace("http:", "https:");
            var path = Path.Combine(url + "csv", fname + ".csv");


            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");

            path = path.Replace("\\", "/");
            using (WebClient cliente = new WebClient())
            {
                cliente.DownloadFile(path, pathDownload + "\\export.csv");
                return RedirectToAction("Index");
            }

        }

        // GET csv
        public ActionResult getCsv()
        {
            //var model = db.mpc_podcasts.ToList();
            //return new csvActionResult<mpc_podcasts>(model, "podcasts.csv");
            var model = from p in globalPodcasts
                        select new mpc_podcastsCSV
                        {
                            data = String.Format("{0:dd/MM/yyyy HH:mm}", p.datalancamento),
                            nome = p.nome,
                            categoria = getCategoria(p.categoria_id),
                            descricao = p.descricao,
                            patrocinador = p.patrocinador,
                            grupo = getGrupo(p.grupo_id),
                            tempo = p.tempo,
                            rating = String.Format("{0:0.##}", p.rating),
                            audiencia = String.Format("{0:0.##}", p.audiencia)
                        };

            return new csvActionResult<mpc_podcastsCSV>(model.ToList(), "podcasts.csv");

        }

        // POST: mpc_podcasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mpc_podcasts mpc_podcasts = db.mpc_podcasts.Find(id);
            db.mpc_podcasts.Remove(mpc_podcasts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HabilitarPodcast(string idPodcast)
        {
            var mensagemInfo = string.Empty;

            try
            {
                var objPodcast = db.mpc_podcasts.Find(idPodcast);
                var objPergunta = db.mpc_perguntas.Where(p => p.podcast_id == idPodcast).Any();

                if (objPodcast.habilitado)
                {
                    mensagemInfo = "Podcast já está habilitado!";
                }
                else if (objPergunta.Equals(false))
                {
                    mensagemInfo = "Podcast não possui nenhuma pergunta vinculada. Por favor adicione pelo menos uma pergunta!";
                }
                else
                {
                    objPodcast.datalancamento = DateTime.Now;
                    objPodcast.habilitado = true;
                    mensagemInfo = "Podcast habilitado com sucesso!";

                    db.Entry(objPodcast).State = EntityState.Modified;
                    db.SaveChanges();

                    //Renzo 20140411 - Enviar push para podcast habilitado
                    MobileServiceClient client = new MobileServiceClient("https://tt-medley-podcast.azure-mobile.net/", "cxmyfPgDBPNFEDTpVkbwPGKilpsOId44");
                    MobileServiceInvalidOperationException exc = null;
                    try
                    {
                        Dictionary<string, string> apiParam = new Dictionary<string, string>();
                        apiParam.Add("podcast", idPodcast);
                        var apiCall = client.InvokeApiAsync<pushInfo>("send_push", HttpMethod.Post, apiParam);
                    }
                    catch (MobileServiceInvalidOperationException exception)
                    {
                        exc = exception;
                    }
                    //Renzo 20140411 End
                }
            }
            catch (Exception ex)
            {
                return Json(mensagemInfo = "Ocorreu um problema ao tentar habilitar o Podcast! Erro: " + ex.Message, JsonRequestBehavior.AllowGet);
            }

            
            return Json(mensagemInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgendarPodcast(string idPodcast, DateTime dataLancamento)
        {
            var objPodcast = db.mpc_podcasts.Find(idPodcast);
            string mensagemInfo = string.Empty;
            var objPergunta = db.mpc_perguntas.Where(p => p.podcast_id == idPodcast).Any();

            try
            {
                DateTime dtConvert = DateTime.ParseExact(dataLancamento.ToString().Replace("/", "").Replace(":", "").Replace(" ", ""),
                                        "MMddyyyyHHmmss",
                                        CultureInfo.InvariantCulture);

                if (objPergunta.Equals(false))
                {
                    mensagemInfo = "O Podcast não possui nenhuma pergunta vinculada. Por favor adicione pelo menos uma pergunta!";
                }
                else if (objPodcast.datalancamento != null)
                {
                    mensagemInfo = "O Podcast selecionado já está habilitado. Data de Lançamento: " + objPodcast.datalancamento;
                }
                else if (dtConvert < DateTime.Now)
                {
                    mensagemInfo = "A Data de Lançamento não pode ser menor do que a data atual.";
                }
                else
                {
                    mensagemInfo = "Podcast agendado com sucesso!";
                    objPodcast.datalancamento = dtConvert;
                    db.Entry(objPodcast).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(mensagemInfo = "Ocorreu um problema ao tentar agendar o Podcast! Erro: " + ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(mensagemInfo, JsonRequestBehavior.AllowGet);
        }

        //HELPER Function
        //Renzo 2015-01-14 - buscando o nome da categoria e do grupo ao passar o ID
        public string getCategoria(string id)
        {
            //return db.mpc_categorias.SingleOrDefault(d => d.id == id);
            if (id == null)
            {
                return "Nulo";
            }
            else
            {
                return db.mpc_categorias.SingleOrDefault(d => d.id == id).nome;
            }

        }
        public string getGrupo(string id)
        {
            //return db.mpc_categorias.SingleOrDefault(d => d.id == id);
            if (id == null)
            {
                return "Nulo";
            }
            else
            {
                return db.mpc_grupos.SingleOrDefault(d => d.id == id).nome;
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

    public class pushInfo
    {
        public string podcast { get; set; }
        public List<string> usuarios { get; set; }
    }

}
