namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_usuarios")]
    public partial class mpc_usuarios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), StringLength(255)]
        public string id { get; set; }
        

        [Column("__updatedAt")]
        public DateTimeOffset? C__updatedAt { get; set; }

        [Column("__version", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] C__version { get; set; }

        [Column("__deleted")]
        public bool C__deleted { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
                
        [Required]
        [StringLength(11,ErrorMessage = "Digite os 11 algarismos do CPF (sem hífens ou pontos)")]
        public string cpf { get; set; }

        public string empresa_id { get; set; }

        public string cargo_id { get; set; }

        public string unidade_id { get; set; }

        public string equipe_id { get; set; }

        public string gestor_id { get; set; }

        public string id_matricula { get; set; }

        public bool? is_gestor { get; set; }

        public bool? is_habilitado { get; set; }

        public bool? is_admin { get; set; }

        public string admin_senha { get; set; }

        public string senha { get; set; }

        public string salt { get; set; }

        public string validation_code { get; set; }
    }
    public class mpc_usuariosCSV
    {
        public string nome { get; set; }

        public string email { get; set; }

        public string cpf { get; set; }

        public string cargo { get; set; }
        public string empresa { get; set; }
        public string unidade { get; set; }
        public string equipe { get; set; }
        public string gestor { get; set; }
        public string matricula { get; set; }
        public string engajamento { get; set; }
        public string notas { get; set; }
        public string audiencia { get; set; }


    }
}
