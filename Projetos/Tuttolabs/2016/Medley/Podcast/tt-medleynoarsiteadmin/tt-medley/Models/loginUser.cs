using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tt_medley.Models
{
    public class loginUser
    {
        public string email { get; set; }
        public string senha { get; set; }
        public bool RememberMe { get; set; }
    }
}