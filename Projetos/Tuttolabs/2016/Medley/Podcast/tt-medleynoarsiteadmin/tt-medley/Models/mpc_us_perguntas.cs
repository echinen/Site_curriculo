namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_us_perguntas")]
    public partial class mpc_us_perguntas
    {
        [StringLength(255)]
        public string id { get; set; }

        [Column("__createdAt")]
        public DateTimeOffset C__createdAt { get; set; }

        [Column("__updatedAt")]
        public DateTimeOffset? C__updatedAt { get; set; }

        [Column("__version", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] C__version { get; set; }

        [Column("__deleted")]
        public bool C__deleted { get; set; }

        public string usuario_id { get; set; }

        public string podcast_id { get; set; }

        public string pergunta_id { get; set; }

        public double? resposta { get; set; }

        public bool? is_certa { get; set; }

        public DateTimeOffset? dataresposta { get; set; }
    }
}
