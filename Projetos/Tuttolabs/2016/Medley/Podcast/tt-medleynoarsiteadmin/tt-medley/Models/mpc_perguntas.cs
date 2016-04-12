namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_perguntas")]
    public partial class mpc_perguntas
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

        public string podcast_id { get; set; }

        [Required(ErrorMessage ="O campo pergunta é obrigatório!")]
        public string pergunta { get; set; }

        [Required(ErrorMessage = "O campo resp1 é obrigatório!")]
        public string resp1 { get; set; }

        [Required(ErrorMessage = "O campo resp2 é obrigatório!")]
        public string resp2 { get; set; }

        [Required(ErrorMessage = "O campo resp3 é obrigatório!")]
        public string resp3 { get; set; }

        [Required(ErrorMessage = "O campo resp4 é obrigatório!")]
        public string resp4 { get; set; }

        public double? resp_certa { get; set; }
    }
}
