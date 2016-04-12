using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tt_medley.Models;
using System.Security.Cryptography;
using PagedList;
using System.IO;
using CsvHelper;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Data.Entity.Validation;

namespace tt_medley.Controllers
{
    public class mpc_usuariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public static List<mpc_usuarios> globalUsuarios = new List<mpc_usuarios>();
        // GET: mpc_usuarios
        public ActionResult Index(string sortOrder, string currentFilter, string nameString, string emailString, string cpfString, int? page, mpc_usuarios ur)
        {

            ViewBag.CurrentSort = sortOrder;
            //variáveis que definirão se o sort é ascending ou descending
            ViewBag.NomeSortOrder = String.IsNullOrEmpty(sortOrder) ? "nome_desc" : "";
            ViewBag.EmpresaSortOrder = sortOrder == "empresa" ? "empresa_desc" : "empresa";
            ViewBag.CargoSortOrder = sortOrder == "cargo" ? "cargo_desc" : "cargo";
            ViewBag.UnidadeSortOrder = sortOrder == "unidade" ? "unidade_desc" : "unidade";
            ViewBag.EquipeSortOrder = sortOrder == "equipe" ? "equipe_desc" : "equipe";

            if (nameString != null)
            {
                page = 1;
            }
            else
            {
                nameString = currentFilter;
            }

            ViewBag.CurrentFilter = nameString;

            var usuarios = from p in db.mpc_usuarios
                           where p.C__deleted == false
                           select p;

            if (!String.IsNullOrEmpty(nameString))
            {
                usuarios = usuarios.Where(p => p.nome.Contains(nameString));
            }

            if (!String.IsNullOrEmpty(emailString))
            {
                usuarios = usuarios.Where(p => p.email.Contains(emailString));
            }

            if (!String.IsNullOrEmpty(cpfString))
            {
                usuarios = usuarios.Where(p => p.cpf.Contains(cpfString));
            }

            switch (sortOrder)
            {
                case "nome_desc":
                    usuarios = usuarios.OrderByDescending(p => p.nome);
                    break;
                case "empresa":
                    usuarios = from p in usuarios
                               join c in db.mpc_empresas on p.empresa_id equals c.id
                               orderby c.nome ascending
                               select p;
                    break;
                case "empresa_desc":
                    usuarios = from p in usuarios
                               join c in db.mpc_empresas on p.empresa_id equals c.id
                               orderby c.nome descending
                               select p;
                    break;
                case "cargo":
                    usuarios = from p in usuarios
                               join c in db.mpc_cargos on p.cargo_id equals c.id
                               orderby c.nome ascending
                               select p;
                    break;
                case "cargo_desc":
                    usuarios = from p in usuarios
                               join c in db.mpc_cargos on p.cargo_id equals c.id
                               orderby c.nome descending
                               select p;
                    break;
                case "unidade":
                    usuarios = from p in usuarios
                               join g in db.mpc_unidades on p.unidade_id equals g.id
                               orderby g.nome ascending
                               select p;
                    break;
                case "unidade_desc":
                    usuarios = from p in usuarios
                               join g in db.mpc_unidades on p.unidade_id equals g.id
                               orderby g.nome descending
                               select p;
                    break;
                case "equipe":
                    usuarios = from p in usuarios
                               join g in db.mpc_equipes on p.equipe_id equals g.id
                               orderby g.nome ascending
                               select p;
                    break;
                case "equipe_desc":
                    usuarios = from p in usuarios
                               join g in db.mpc_equipes on p.equipe_id equals g.id
                               orderby g.nome descending
                               select p;
                    break;                
                default:
                    usuarios = usuarios.OrderBy(p => p.nome);
                    break;
            }

            ViewBag.empresa = db.mpc_empresas.ToList();
            ViewBag.cargo = db.mpc_cargos.ToList();
            ViewBag.unidade = db.mpc_unidades.ToList();
            ViewBag.equipe = db.mpc_equipes.ToList();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            globalUsuarios = usuarios.ToList();
            return View(usuarios.ToPagedList(pageNumber, pageSize));
        }

        // GET: mpc_usuarios/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_usuarios mpc_usuarios = db.mpc_usuarios.Find(id);
            if (mpc_usuarios == null)
            {
                return HttpNotFound();
            }
            return View(mpc_usuarios);
        }

        // GET: mpc_usuarios/Create
        public ActionResult Create()
        {
            ViewBag.empresa = db.mpc_empresas.Where(u => u.C__deleted == false).ToList();
            ViewBag.cargo = db.mpc_cargos.Where(u => u.C__deleted == false).ToList();
            ViewBag.unidade = db.mpc_unidades.Where(u => u.C__deleted == false).ToList();
            ViewBag.equipe = db.mpc_equipes.Where(u => u.C__deleted == false).ToList();
            ViewBag.gestor = db.mpc_usuarios.Where(u => u.is_gestor == true).Where(u => u.C__deleted == false).ToList();
            return View();
        }

        // POST: mpc_usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nome,email,cpf,id_matricula,is_gestor,is_habilitado,senha,salt,validation_code,empresa_id,cargo_id,unidade_id,equipe_id,gestor_id")] mpc_usuarios mpc_usuarios, string currentFilter, string searchString, string isGestor)
        {

            searchString = currentFilter;
   
            ViewBag.CurrentFilter = searchString;

            var usuarios = from p in db.mpc_usuarios where p.is_gestor == true select p;



            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(p => p.nome.Contains(searchString));
                ViewBag.listGestor = usuarios;
            }


            if (ModelState.IsValid)
            {

                if (db.mpc_usuarios.SingleOrDefault(u => u.email == mpc_usuarios.email) != null)
                {

                
                /*foreach(var item in db.mpc_usuarios.ToList())
                {
                    if(item.email == mpc_usuarios.email)
                    {*/
                        ViewBag.empresa = db.mpc_empresas.Where(u => u.C__deleted == false).ToList();
                        ViewBag.cargo = db.mpc_cargos.Where(u => u.C__deleted == false).ToList();
                        ViewBag.unidade = db.mpc_unidades.Where(u => u.C__deleted == false).ToList();
                        ViewBag.equipe = db.mpc_equipes.Where(u => u.C__deleted == false).ToList();
                        ViewBag.gestor = db.mpc_usuarios.Where(u => u.is_gestor == true).Where(u => u.C__deleted == false).ToList();
                        ViewBag.erroExistingMail = "Este email já está em uso, por favor tente um endereço diferente";
                        return View();
                    
                        
                }

                RandomNumberGenerator rng = new RNGCryptoServiceProvider();
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                string token = Convert.ToBase64String(tokenData);

                mpc_usuarios.salt = token;
                //mpc_usuarios.gestor_id = selectedGestor;
                //mpc_usuarios.empresa_id = selectedEmpresa;
                //mpc_usuarios.unidade_id = selectedUnidade;
                //mpc_usuarios.cargo_id = selectedCargo;
                //mpc_usuarios.equipe_id = selectedEquipe;
                mpc_usuarios.is_gestor = Convert.ToBoolean(isGestor);
                mpc_usuarios.is_habilitado = false;
                mpc_usuarios.is_admin = false;
                db.mpc_usuarios.Add(mpc_usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(mpc_usuarios);
        }

        //POST: mpc_usuarios/Import
        //Renzo 2016-01-14: Action usada para importar arquivos CSV com usuários
        [HttpPost]
        public ActionResult Import()
        {
            int linha = 0;
            string campo = "";
            string path = "";
            StreamReader textReader = null;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        if (!Directory.Exists(Server.MapPath("~/imports/")))
                            Directory.CreateDirectory(Server.MapPath("~/imports/"));
                        path = Path.Combine(Server.MapPath("~/imports/"), fileName);
                        file.SaveAs(path);
                        textReader = new StreamReader(path, Encoding.Default,true);
                        var csv = new CsvReader(textReader);
                        string empresa = "", cargo = "", unidade = "", equipe = "", gestor = "";
                        mpc_usuarios usuario = new mpc_usuarios();
                        var empresas = from e in db.mpc_empresas select e;
                        var cargos = from c in db.mpc_cargos select c;
                        var unidades = from u in db.mpc_unidades select u;
                        var equipes = from e in db.mpc_equipes select e;
                        var gestores = from u in db.mpc_usuarios where u.is_gestor == true select u;

                        while (csv.Read())
                        {
                            linha++;
                            usuario = new mpc_usuarios();
                            campo = "matricula";
                            usuario.id_matricula = csv.GetField<string>("matricula");
                            campo = "subordinados";
                            usuario.is_gestor = csv.GetField<bool>("subordinados");
                            campo = "nome";
                            usuario.nome = csv.GetField<string>("nome");
                            campo = "cpf";
                            usuario.cpf = csv.GetField<string>("cpf").Replace(".","").Replace("-","");
                            campo = "email";
                            usuario.email = csv.GetField<string>("email");
                            if (db.mpc_usuarios.Where(u=> u.C__deleted == false).FirstOrDefault(u => u.email == usuario.email) != null)
                            {
                                throw new Exception("Email já cadastrado");
                            }

                            usuario.is_habilitado = false;
                            usuario.is_admin = false;
                            campo = "unidade";
                            unidade = csv.GetField<string>("unidade");
                            unidades = db.mpc_unidades.Where(u => u.nome.Contains(unidade));
                            usuario.unidade_id = unidades.ToList()[0].id;
                            campo = "cargo";
                            cargo = csv.GetField<string>("cargo");
                            cargos = db.mpc_cargos.Where(u => u.nome.Contains(cargo));
                            usuario.cargo_id = cargos.ToList()[0].id;
                            campo = "empresa";
                            empresa = csv.GetField<string>("empresa");
                            empresas = db.mpc_empresas.Where(u => u.nome.Contains(empresa));
                            usuario.empresa_id = empresas.ToList()[0].id;
                            campo = "equipe";
                            equipe = csv.GetField<string>("equipe");
                            equipes = db.mpc_equipes.Where(u => u.nome.Contains(equipe));
                            usuario.equipe_id = equipes.ToList()[0].id;
                            campo = "gestor";
                            gestor = csv.GetField<string>("gestor");
                            gestores = db.mpc_usuarios.Where(u => u.email.Contains(gestor)).Where(u => u.is_gestor == true);
                            usuario.gestor_id = gestores.ToList()[0].id;

                            db.mpc_usuarios.Add(usuario);
                        }

                        db.SaveChanges();
                        textReader.Close();

                        System.IO.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        if (textReader !=null)
                        {
                            textReader.Close();
                            System.IO.File.Delete(path);
                        }

                        if (ex is DbUpdateException)
                        {
                            TempData["AlertMessage"] = "Erro ao salvar usuários: " + ex.Message + ". Verifique se o nome e/ou email dos usuários são únicos.";
                            return RedirectToAction("Index");
                        } else if (ex is DbEntityValidationException) //(ex.Source == "EntityFramework")
                        {
                            DbEntityValidationException exc = ex as DbEntityValidationException;
                            TempData["AlertMessage"] = "Erro no modelo, verifique formato de emails, CPFs (somente números)";
                            return RedirectToAction("Index");
                        } else
                        { 
                            TempData["AlertMessage"] = "Erro ao ler planilha na linha " + linha.ToString() + ", campo " + campo + ". " + ex.Message;
                            return RedirectToAction("Index");
                        }
              
                    }
                }
            }
            TempData["AlertMessage"] = "Sucesso ao cadastrar usuários";
            return RedirectToAction("Index");
        }

        // GET: mpc_usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.empresa = db.mpc_empresas.Where(u=> u.C__deleted == false).ToList();
            ViewBag.cargo = db.mpc_cargos.Where(u => u.C__deleted == false).ToList();
            ViewBag.unidade = db.mpc_unidades.Where(u => u.C__deleted == false).ToList();
            ViewBag.equipe = db.mpc_equipes.Where(u => u.C__deleted == false).ToList();
            ViewBag.gestor = db.mpc_usuarios.Where(u => u.is_gestor == true).Where(u => u.C__deleted == false).ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_usuarios mpc_usuarios = db.mpc_usuarios.Find(id);
            if (mpc_usuarios == null)
            {
                return HttpNotFound();
            }
            return View(mpc_usuarios);
        }

        // POST: mpc_usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,email,cpf,is_gestor,is_habilitado,empresa_id,cargo_id,unidade_id,equipe_id,gestor_id")] mpc_usuarios mpc_usuarios)
        {
            if (ModelState.IsValid)
            {

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        //mpc_usuarios.gestor_id = selectedGestor;
                        //mpc_usuarios.empresa_id = selectedEmpresa;
                        //mpc_usuarios.unidade_id = selectedUnidade;
                        //mpc_usuarios.cargo_id = selectedCargo;
                        //mpc_usuarios.equipe_id = selectedEquipe;                    
                        db.Entry(mpc_usuarios).State = EntityState.Modified;
                        db.Entry(mpc_usuarios).Property(x => x.senha).IsModified = false;
                        db.Entry(mpc_usuarios).Property(x => x.salt).IsModified = false;
                        db.Entry(mpc_usuarios).Property(x => x.is_admin).IsModified = false;
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
            return View(mpc_usuarios);
        }

        // GET: mpc_usuarios/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_usuarios mpc_usuarios = db.mpc_usuarios.Find(id);
            if (mpc_usuarios == null)
            {
                return HttpNotFound();
            }
            return View(mpc_usuarios);
        }

        // POST: mpc_usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "id")] mpc_usuarios mpc_usuarios, string id)
        {
            if (id != null)
            //if (ModelState.IsValid)
            {

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        var del = db.mpc_usuarios.FirstOrDefault(u => u.id == id);


                        if (del == null)
                            return HttpNotFound();

                        del.C__deleted = true;

                        //mpc_usuarios.C__deleted = true;
                        //db.Entry(mpc_usuarios).State = EntityState.Modified;
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
            return View(mpc_usuarios);
        }


        // GET csv
        public ActionResult getCsv()
        {
            var model = from u in globalUsuarios
                        join eng in db.mpc_us_engajamento
                        on u.id equals eng.usuario_id into grouped
                        from g in grouped.DefaultIfEmpty(new mpc_us_engajamento { indice_calculado = 0, notas = 0, audiencia = 0})

                        select new mpc_usuariosCSV
                        {
                            nome = u.nome,
                            email = u.email,
                            cpf = u.cpf,
                            cargo = GetCargo(u.cargo_id),
                            empresa = GetEmpresa(u.empresa_id),
                            unidade = GetUnidade(u.unidade_id),
                            equipe = GetEquipe(u.equipe_id),
                            gestor = GetGestorMail(u.gestor_id),
                            matricula = u.id_matricula,
                            engajamento = String.Format("{0:0.##}", g.indice_calculado),
                            notas = String.Format("{0:0.##}", g.notas),
                            audiencia = String.Format("{0:0.##}", g.audiencia)
                        };
             return new csvActionResult<mpc_usuariosCSV>(model.ToList(), "usuarios.csv");

        }

        public string GetEmpresa(string id)
        {
            if (id == null)
            {
                return "";
            }
            else
            {
                return db.mpc_empresas.SingleOrDefault(d => d.id == id).nome;
            }
            
        }

        public string GetCargo(string id)
        {
            if (id == null)
            {
                return "";
            }
            else
            {
                return db.mpc_cargos.SingleOrDefault(d => d.id == id).nome;
            }
        }

        public string GetUnidade(string id)
        {
            if (id == null)
            {
                return "";
            }
            else
            {
                return db.mpc_unidades.SingleOrDefault(d => d.id == id).nome;
            }
        }

        public string GetEquipe(string id)
        {
            if (id == null)
            {
                return "";
            }
            else
            {
                return db.mpc_equipes.SingleOrDefault(d => d.id == id).nome;
            }
        }

        public string GetGestorMail(string id)
        {
            
            if (id == null || id == "")
            {
                return "";
            }
            else
            {
                return db.mpc_usuarios.SingleOrDefault(d => d.id == id).email;               
            }
        }
        
        public List<double> getEngage(string id)
        {
            List<double> engage = new List<double>(3);
            if (id == null || id == "")
            {
                return engage;
            }
            else
            {
                mpc_us_engajamento usuario = db.mpc_us_engajamento.SingleOrDefault(d => d.usuario_id == id);
                if (usuario == null)
                {
                    engage.Add(0);
                    engage.Add(0);
                    engage.Add(0);
                } else
                {
                    engage.Add(usuario.indice_calculado ?? 0);
                    engage.Add(usuario.notas ?? 0);
                    engage.Add(usuario.audiencia ?? 0);
                }

                return engage;
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
