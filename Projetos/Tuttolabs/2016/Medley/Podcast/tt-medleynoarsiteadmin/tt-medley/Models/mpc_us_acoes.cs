namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_us_acoes")]
    public partial class mpc_us_acoes
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

        public DateTimeOffset? dataacao { get; set; }

        public bool? login { get; set; }

        public bool? ouviu { get; set; }

        public string categoria_id { get; set; }

        public string audio_id { get; set; }
    }

    public class mpc_us_acoesRel
    {
        public string usuario { get; set; }

        public DateTimeOffset? dataacao { get; set; }

        public bool? login { get; set; }

        public bool? ouviu { get; set; }

        public string categoria { get; set; }

        public string audio { get; set; }
    }
}
