using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tt_medley.Models;

namespace tt_medley.Controllers
{
    public class mpc_gruposController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mpc_grupos
        public ActionResult Index()
        {
            List<mpc_grupos> lista = new List<mpc_grupos>();

            foreach (var item in db.mpc_grupos)
            {
                if (item.C__deleted == false)
                {
                    lista.Add(item);
                }
            }
            return View(lista);
        }

        //GET: mpc_grupos/Users
        //Renzo 20160222 - busca de usuários para preencher grupo
        public JsonResult Users(string userstring)
        {
            //return View(db.mpc_usuarios.ToList());
            List<searchUsers> usuarios = db.mpc_usuarios.Where(u => u.nome.Contains(userstring)).Where(u => u.C__deleted == false).Select(u => new searchUsers { id = u.id, text = u.nome, parent = "#" }).ToList();
            return this.Json(usuarios, JsonRequestBehavior.AllowGet);
        }

        // GET: mpc_grupos/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_grupos mpc_grupos = db.mpc_grupos.Find(id);
            if (mpc_grupos == null)
            {
                return HttpNotFound();
            }

            return View(mpc_grupos);
        }

        // GET: mpc_grupos/Create
        public ActionResult Create()
        {
            TreeViewNodeVM arvore = new TreeViewNodeVM();
            ViewBag.treeView = arvore.GetTreeViewList();
            return View();
        }

        // POST: mpc_grupos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nome,descricao")] mpc_grupos mpc_grupos, string selectedElmsIds, string selectedUsersIds)
        {
            TreeViewNodeVM arvore = new TreeViewNodeVM();
            ViewBag.treeView = arvore.GetTreeViewList();
            //["j1_2","j1_3","j1_10"]
            string[] selectedItems;
            char[] stringSeparators = new char[] {','};
            string selectedItemsRaw = selectedElmsIds.Replace("\"", "");
            selectedItemsRaw = selectedItemsRaw.Replace("[", "");
            selectedItemsRaw = selectedItemsRaw.Replace("]", "");
            selectedItems = selectedItemsRaw.Split(stringSeparators);

            string[] selectedUsers;
            string selectedUsersRaw = selectedUsersIds.Replace("\"", "");
            selectedUsersRaw = selectedUsersRaw.Replace("[", "");
            selectedUsersRaw = selectedUsersRaw.Replace("]", "");
            selectedUsers = selectedUsersRaw.Split(stringSeparators);

            int agrupamentoId = 0;
            int selectedItemId = 0;
            
            if (ModelState.IsValid)
            {
                if (mpc_grupos.nome == null)
                {
                    ViewBag.errorMessageNome = "Este campo é obrigatório";
                    return View();
                }
                else
                {
                    db.mpc_grupos.Add(mpc_grupos);
                    db.SaveChanges();
                    List<mpc_grupos_us> grupoItems = new List<mpc_grupos_us>();
                    mpc_grupos_us grupoItem;
                    //foreach (var item in selectedItems)
                    //{
                        //Loop pelos elementos selecionados na jsTree, para cada um, buscar na arvore, achar o Id, e adicionar no mpc_grupos_us
                        foreach (var rootNode in ViewBag.treeView.ChildNode)
                        {
                            if (selectedItemId >= selectedItems.Length)
                            {
                                break;
                            }
                        //root nodes são as categorias/agrupamentos
                            if (rootNode.treeId == selectedItems[selectedItemId])
                            {
                                //Selecionado é igual a um dos roots, não utilizar
                                selectedItemId++;
                               
                            } //else
                            //{
                                //Entrar num loop dentro de cada grupo, de cada rootNode
                                foreach(var childNode in rootNode.ChildNode)
                                {
                                    if (selectedItemId >= selectedItems.Length)
                                    {
                                        break;
                                    }
                                    if (childNode.treeId == selectedItems[selectedItemId])
                                    {
                                        //Achei um item a adicionar em mpc_grupos_us
                                        grupoItem = new mpc_grupos_us();
                                        grupoItem.grupo_id = mpc_grupos.id;
                                        switch (agrupamentoId)
                                        {
                                            case 0: //empresa
                                                grupoItem.empresa_id = childNode.ItemId;
                                                break;
                                            case 1: //cargo
                                                grupoItem.cargo_id = childNode.ItemId;
                                                break;
                                            case 2: //unidade
                                                grupoItem.unidade_id = childNode.ItemId;
                                                break;
                                            case 3: //equipe
                                                grupoItem.equipe_id = childNode.ItemId;
                                                break;
                                            default: //gestor
                                                grupoItem.gestor_id = childNode.ItemId;
                                                break;
                                        }
                                        db.mpc_grupos_us.Add(grupoItem);
                                        selectedItemId++;

                                        //grupoItems.Add(grupoItem);
                                    }
                                }
                                
                            //}

                        //}
                        agrupamentoId++;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

                    grupoItems = new List<mpc_grupos_us>();

                    foreach (var userId in selectedUsers)
                    {
                        grupoItem = new mpc_grupos_us();
                        grupoItem.grupo_id = mpc_grupos.id;
                        grupoItem.usuario_id = userId;
                        db.mpc_grupos_us.Add(grupoItem);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    return RedirectToAction("Index");
                }
                
            }

            return View(mpc_grupos);
        }

        // GET: mpc_grupos/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_grupos mpc_grupos = db.mpc_grupos.Find(id);
            if (mpc_grupos == null)
            {
                return HttpNotFound();
            }
            List<mpc_grupos_us> mpc_grupos_us = db.mpc_grupos_us.Where(i => i.grupo_id == mpc_grupos.id).Where(i=>i.C__deleted == false).ToList<mpc_grupos_us>();
            List<string> userIds = new List<string>();
            foreach (var grupoItem in mpc_grupos_us)
            {
                if (grupoItem.usuario_id != null)
                {
                    userIds.Add(grupoItem.usuario_id);
                }
            }
            List<mpc_usuarios> usuarios = db.mpc_usuarios.Where(u => userIds.Contains(u.id)).ToList<mpc_usuarios>();
            TreeViewNodeVM arvore = new TreeViewNodeVM();
            ViewBag.treeView = arvore.GetTreeViewList();
            ViewBag.grupos_is = mpc_grupos_us;
            ViewBag.usuarios = usuarios;
            return View(mpc_grupos);
        }

        // POST: mpc_grupos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,descricao")] mpc_grupos mpc_grupos, String id, string selectedElmsIds, string selectedUsersIds)
        {
            TreeViewNodeVM arvore = new TreeViewNodeVM();
            ViewBag.treeView = arvore.GetTreeViewList();
            //["j1_2","j1_3","j1_10"]
            string[] selectedItems;
            char[] stringSeparators = new char[] { ',' };
            string selectedItemsRaw = selectedElmsIds.Replace("\"", "");
            selectedItemsRaw = selectedItemsRaw.Replace("[", "");
            selectedItemsRaw = selectedItemsRaw.Replace("]", "");
            selectedItems = selectedItemsRaw.Split(stringSeparators);

            string[] selectedUsers;
            string selectedUsersRaw = selectedUsersIds.Replace("\"", "");
            selectedUsersRaw = selectedUsersRaw.Replace("[", "");
            selectedUsersRaw = selectedUsersRaw.Replace("]", "");
            selectedUsers = selectedUsersRaw.Split(stringSeparators);

            int agrupamentoId = 0;
            int selectedItemId = 0;
            if (ModelState.IsValid)
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.Entry(mpc_grupos).State = EntityState.Modified;
                        db.SaveChanges();
                        List<mpc_grupos_us> mpc_grupos_us = db.mpc_grupos_us.Where(i => i.grupo_id == mpc_grupos.id).ToList<mpc_grupos_us>();
                        db.mpc_grupos_us.RemoveRange(mpc_grupos_us);
                        db.SaveChanges();

                        List<mpc_grupos_us> grupoItems = new List<mpc_grupos_us>();
                        mpc_grupos_us grupoItem;
                        //foreach (var item in selectedItems)
                        //{
                        //Loop pelos elementos selecionados na jsTree, para cada um, buscar na arvore, achar o Id, e adicionar no mpc_grupos_us
                        foreach (var rootNode in ViewBag.treeView.ChildNode)
                        {
                            if (selectedItemId >= selectedItems.Length)
                            {
                                break;
                            }
                            //root nodes são as categorias/agrupamentos
                            if (rootNode.treeId == selectedItems[selectedItemId])
                            {
                                //Selecionado é igual a um dos roots, não utilizar
                                selectedItemId++;

                            } //else
                              //{
                              //Entrar num loop dentro de cada grupo, de cada rootNode
                            foreach (var childNode in rootNode.ChildNode)
                            {
                                if (selectedItemId >= selectedItems.Length)
                                {
                                    break;
                                }
                                if (childNode.treeId == selectedItems[selectedItemId])
                                {
                                    //Achei um item a adicionar em mpc_grupos_us
                                    grupoItem = new mpc_grupos_us();
                                    grupoItem.grupo_id = mpc_grupos.id;
                                    switch (agrupamentoId)
                                    {
                                        case 0: //empresa
                                            grupoItem.empresa_id = childNode.ItemId;
                                            break;
                                        case 1: //cargo
                                            grupoItem.cargo_id = childNode.ItemId;
                                            break;
                                        case 2: //unidade
                                            grupoItem.unidade_id = childNode.ItemId;
                                            break;
                                        case 3: //equipe
                                            grupoItem.equipe_id = childNode.ItemId;
                                            break;
                                        default: //gestor
                                            grupoItem.gestor_id = childNode.ItemId;
                                            break;
                                    }
                                    db.mpc_grupos_us.Add(grupoItem);
                                    selectedItemId++;

                                    //grupoItems.Add(grupoItem);
                                }
                            }

                            //}

                            //}
                            agrupamentoId++;
                        }
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }

                        grupoItems = new List<mpc_grupos_us>();

                        foreach (var userId in selectedUsers)
                        {
                            grupoItem = new mpc_grupos_us();
                            grupoItem.grupo_id = mpc_grupos.id;
                            grupoItem.usuario_id = userId;
                            db.mpc_grupos_us.Add(grupoItem);
                        }
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }

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
            return View(mpc_grupos);
        }

        // GET: mpc_grupos/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mpc_grupos mpc_grupos = db.mpc_grupos.Find(id);
            if (mpc_grupos == null)
            {
                return HttpNotFound();
            }
            return View(mpc_grupos);
        }

        // POST: mpc_grupos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "id,C_delete")] mpc_grupos mpc_grupos, String nome, String descricao, String id)
        {
            if (ModelState.IsValid)
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        mpc_grupos.nome = nome;
                        mpc_grupos.descricao = descricao;
                        mpc_grupos.C__deleted = true;
                        db.Entry(mpc_grupos).State = EntityState.Modified;
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
    public class searchUsers
    {
        public string text { get; set; }
        public string id { get; set; }
        public string parent { get; set; }
    }
}
