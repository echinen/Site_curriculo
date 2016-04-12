using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tt_medley.Models
{
    public class TreeViewNodeVM
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public TreeViewNodeVM()
        {
            ChildNode = new List<TreeViewNodeVM>();
        }

        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string treeId { get; set; }
        public string NodeName
        {
            get { return GetNodeName(); }
        }
        public IList<TreeViewNodeVM> ChildNode { get; set; }

        public string GetNodeName()
        {
            return ItemName;
        }

        public TreeViewNodeVM GetTreeViewList()
        {
            int treeIdNumber = 1;
            string treeIdName = "j1_";

            TreeViewNodeVM rootNode = new TreeViewNodeVM()
            {
                ItemId = "",
                ItemName = "root"
            };

            //Empresas
            TreeViewNodeVM empresaNode = new TreeViewNodeVM()
            {
                ItemId = "mpc_empresas",
                ItemName = "Empresa",
                treeId = treeIdName + treeIdNumber.ToString()
            };
            treeIdNumber++;

            var empresas = db.mpc_empresas.Where(e=> e.C__deleted == false).ToList();
            foreach(var empresa in empresas)
            {
                TreeViewNodeVM empresasItems = new TreeViewNodeVM()
                {
                    ItemId = empresa.id,
                    ItemName = empresa.nome,
                    treeId = treeIdName + treeIdNumber.ToString()
                };
                treeIdNumber++;
                empresaNode.ChildNode.Add(empresasItems);
            };
            rootNode.ChildNode.Add(empresaNode);

            //Cargos
            TreeViewNodeVM cargoNode = new TreeViewNodeVM()
            {
                ItemId = "mpc_cargos",
                ItemName = "Cargo",
                treeId = treeIdName + treeIdNumber.ToString()
            };
            treeIdNumber++;
            var cargos = db.mpc_cargos.Where(e => e.C__deleted == false).ToList();
            foreach (var cargo in cargos)
            {
                TreeViewNodeVM cargosItems = new TreeViewNodeVM()
                {
                    ItemId = cargo.id,
                    ItemName = cargo.nome,
                    treeId = treeIdName + treeIdNumber.ToString()
                };
                treeIdNumber++;
                cargoNode.ChildNode.Add(cargosItems);
            };
            rootNode.ChildNode.Add(cargoNode);

            //Unidades
            TreeViewNodeVM unidadeNode = new TreeViewNodeVM()
            {
                ItemId = "mpc_unidades",
                ItemName = "Unidade",
                treeId = treeIdName + treeIdNumber.ToString()
            };
            treeIdNumber++;
            var unidades = db.mpc_unidades.Where(e => e.C__deleted == false).ToList();
            foreach (var unidade in unidades)
            {
                TreeViewNodeVM unidadesItems = new TreeViewNodeVM()
                {
                    ItemId = unidade.id,
                    ItemName = unidade.nome,
                    treeId = treeIdName + treeIdNumber.ToString()
                };
                treeIdNumber++;
                unidadeNode.ChildNode.Add(unidadesItems);
            };
            rootNode.ChildNode.Add(unidadeNode);

            //Equipes
            TreeViewNodeVM equipeNode = new TreeViewNodeVM()
            {
                ItemId = "mpc_equipes",
                ItemName = "Equipe",
                treeId = treeIdName + treeIdNumber.ToString()
            };
            treeIdNumber++;
            var equipes = db.mpc_equipes.Where(e => e.C__deleted == false).ToList();
            foreach (var equipe in equipes)
            {
                TreeViewNodeVM equipesItems = new TreeViewNodeVM()
                {
                    ItemId = equipe.id,
                    ItemName = equipe.nome,
                    treeId = treeIdName + treeIdNumber.ToString()
                };
                treeIdNumber++;
                equipeNode.ChildNode.Add(equipesItems);
            };
            rootNode.ChildNode.Add(equipeNode);
            
            //Gestor
            TreeViewNodeVM gestorNode = new TreeViewNodeVM()
            {
                ItemId = "mpc_usuarios",
                ItemName = "Gestor",
                treeId = treeIdName + treeIdNumber.ToString()
            };
            treeIdNumber++;
            var gestores = db.mpc_usuarios.Where(u => u.is_gestor == true).Where(e => e.C__deleted == false).ToList();
            foreach (var gestor in gestores)
            {
                TreeViewNodeVM gestoresItems = new TreeViewNodeVM()
                {
                    ItemId = gestor.id,
                    ItemName = gestor.nome,
                    treeId = treeIdName + treeIdNumber.ToString()
                };
                treeIdNumber++;
                gestorNode.ChildNode.Add(gestoresItems);
            };
            rootNode.ChildNode.Add(gestorNode);

            return rootNode;
        }
        /*
        private void BuildChildNode(TreeViewNodeVM rootNode)
        {
            if (rootNode.ItemName != "root")
            {
                List<TreeViewNodeVM> childNode = new List<TreeViewNodeVM>();

                switch (rootNode.ItemName)
                {
                    case "Empresa":
                        childNode = (from e1 in db.mpc_empresas
                                                          select new TreeViewNodeVM()
                                                          {
                                                              ItemId = e1.id,
                                                              ItemName = e1.nome
                                                          }).ToList<TreeViewNodeVM>();
                        break;
                    case "Cargo":
                        childNode = (from e1 in db.mpc_cargos
                                     select new TreeViewNodeVM()
                                     {
                                         ItemId = e1.id,
                                         ItemName = e1.nome
                                     }).ToList<TreeViewNodeVM>();
                        break;
                    case "Unidade":
                        childNode = (from e1 in db.mpc_unidades
                                     select new TreeViewNodeVM()
                                     {
                                         ItemId = e1.id,
                                         ItemName = e1.nome
                                     }).ToList<TreeViewNodeVM>();
                        break;
                    case "Equipe":
                        childNode = (from e1 in db.mpc_equipes
                                     select new TreeViewNodeVM()
                                     {
                                         ItemId = e1.id,
                                         ItemName = e1.nome
                                     }).ToList<TreeViewNodeVM>();
                        break;
                    default:
                        childNode = (from e1 in db.mpc_usuarios
                                     where e1.is_gestor == true
                                     select new TreeViewNodeVM()
                                     {
                                         ItemId = e1.id,
                                         ItemName = e1.email
                                     }).ToList<TreeViewNodeVM>();
                        break;
                }
                
                if (childNode.Count > 0)
                {
                    foreach (var childRootNode in childNode)
                    {
                        BuildChildNode(childRootNode);
                        rootNode.ChildNode.Add(childRootNode);
                    }
                }
            }
            else
            {
                rootNode.ChildNode.Add(new TreeViewNodeVM()
                {
                    ItemId = "mpc_empresas",
                    ItemName = "Empresa"
                });
                rootNode.ChildNode.Add(new TreeViewNodeVM()
                {
                    ItemId = "mpc_cargos",
                    ItemName = "Cargo"
                });
                rootNode.ChildNode.Add(new TreeViewNodeVM()
                {
                    ItemId = "mpc_unidades",
                    ItemName = "Unidade"
                });
                rootNode.ChildNode.Add(new TreeViewNodeVM()
                {
                    ItemId = "mpc_equipes",
                    ItemName = "Equipe"
                });
                rootNode.ChildNode.Add(new TreeViewNodeVM()
                {
                    ItemId = "mpc_usuarios",
                    ItemName = "Gestor"
                });
            }
        }*/
    }
}