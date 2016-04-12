using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace tt_medley.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_podcasts> mpc_podcasts { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_categorias> mpc_categorias { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_grupos> mpc_grupos { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_grupos_us> mpc_grupos_us { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_usuarios> mpc_usuarios { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_empresas> mpc_empresas { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_cargos> mpc_cargos { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_equipes> mpc_equipes { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_push_notifications> mpc_push_notifications { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_perguntas> mpc_perguntas { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_us_acoes> mpc_us_acoes { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_unidades> mpc_unidades { get; set; }

        public System.Data.Entity.DbSet<tt_medley.Models.mpc_us_engajamento> mpc_us_engajamento { get; set; }
    }
}