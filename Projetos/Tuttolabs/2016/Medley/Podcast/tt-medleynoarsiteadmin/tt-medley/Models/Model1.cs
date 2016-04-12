namespace tt_medley.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<mpc_cargos> mpc_cargos { get; set; }
        public virtual DbSet<mpc_categorias> mpc_categorias { get; set; }
        public virtual DbSet<mpc_empresas> mpc_empresas { get; set; }
        public virtual DbSet<mpc_equipes> mpc_equipes { get; set; }
        public virtual DbSet<mpc_grupos> mpc_grupos { get; set; }
        public virtual DbSet<mpc_grupos_us> mpc_grupos_us { get; set; }
        public virtual DbSet<mpc_perguntas> mpc_perguntas { get; set; }
        public virtual DbSet<mpc_podcasts> mpc_podcasts { get; set; }
        public virtual DbSet<mpc_push_notifications> mpc_push_notifications { get; set; }
        public virtual DbSet<mpc_unidades> mpc_unidades { get; set; }
        public virtual DbSet<mpc_us_acoes> mpc_us_acoes { get; set; }
        public virtual DbSet<mpc_us_engajamento> mpc_us_engajamento { get; set; }
        public virtual DbSet<mpc_us_perguntas> mpc_us_perguntas { get; set; }
        public virtual DbSet<mpc_us_podcasts> mpc_us_podcasts { get; set; }
        public virtual DbSet<mpc_usuarios> mpc_usuarios { get; set; }
        public virtual DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<mpc_cargos>()
             //   .Property(e => e.C__createdAt)
              //  .HasPrecision(3);

            modelBuilder.Entity<mpc_cargos>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_cargos>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_categorias>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_categorias>()
                .Property(e => e.C__version)
                .IsFixedLength();
            
            modelBuilder.Entity<mpc_empresas>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_empresas>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_equipes>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_equipes>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_grupos>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_grupos>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_grupos_us>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_grupos_us>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_grupos_us>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_perguntas>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_perguntas>()
                .Property(e => e.C__version)
                .IsFixedLength();

            //modelBuilder.Entity<mpc_podcasts>()
                //.Property(e => e.C__createdAt)
                //.HasPrecision(3);

            modelBuilder.Entity<mpc_podcasts>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_podcasts>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_podcasts>()
                .Property(e => e.datalancamento)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_push_notifications>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_push_notifications>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_push_notifications>()
                .Property(e => e.dataenvio)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_unidades>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_unidades>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_us_acoes>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_acoes>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_acoes>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_us_acoes>()
                .Property(e => e.dataacao)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_engajamento>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_engajamento>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_engajamento>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_us_perguntas>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_perguntas>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_perguntas>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_us_perguntas>()
                .Property(e => e.dataresposta)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_podcasts>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_podcasts>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_us_podcasts>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<mpc_usuarios>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<mpc_usuarios>()
                .Property(e => e.C__version)
                .IsFixedLength();

            modelBuilder.Entity<TodoItem>()
                .Property(e => e.C__createdAt)
                .HasPrecision(3);

            modelBuilder.Entity<TodoItem>()
                .Property(e => e.C__updatedAt)
                .HasPrecision(3);

            modelBuilder.Entity<TodoItem>()
                .Property(e => e.C__version)
                .IsFixedLength();
        }
    }
}
