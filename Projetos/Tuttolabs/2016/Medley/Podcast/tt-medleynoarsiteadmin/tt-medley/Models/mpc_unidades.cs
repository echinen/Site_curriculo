namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_unidades")]
    public partial class mpc_unidades
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

        public string nome { get; set; }

        public string descricao { get; set; }
    }
}
