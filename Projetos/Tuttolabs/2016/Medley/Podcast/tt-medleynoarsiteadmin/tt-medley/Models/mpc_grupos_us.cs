namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_grupos_us")]
    public partial class mpc_grupos_us
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), StringLength(255)]
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

        public string grupo_id { get; set; }

        public string usuario_id { get; set; }

        public string empresa_id { get; set; }

        public string cargo_id { get; set; }

        public string unidade_id { get; set; }

        public string equipe_id { get; set; }

        public string gestor_id { get; set; }
    }
}
