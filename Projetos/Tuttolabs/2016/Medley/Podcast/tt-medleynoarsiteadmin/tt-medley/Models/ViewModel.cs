using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tt_medley.Models
{
    public class ViewModel
    {
        public List<mpc_categorias> allCategoria { get; set; }
        public List<mpc_grupos> allUser { get; set; }
        public List<mpc_podcasts> allPodecast { get; set; }
    }
}
