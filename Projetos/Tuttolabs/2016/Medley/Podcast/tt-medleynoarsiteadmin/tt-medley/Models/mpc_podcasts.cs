namespace tt_medley.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tt_medley_podcast.mpc_podcasts")]
    public partial class mpc_podcasts
    {
        //[StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), StringLength(255)]
        public string id { get; set; }

        //[Column("__createdAt")]
        //public DateTimeOffset C__createdAt { get; set; }

        [Column("__updatedAt")]
        public DateTimeOffset? C__updatedAt { get; set; }

        [Column("__version", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] C__version { get; set; }

        [Column("__deleted")]
        public bool C__deleted { get; set; }

        [Required(ErrorMessage ="O campo Nome é obrigatório!")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Selecione uma categoria!")]
        public string categoria_id { get; set; }

        //[Required(ErrorMessage = "O campo Descrição é obrigatório!")]
        public string descricao { get; set; }

        public string patrocinador { get; set; }

        [Required(ErrorMessage = "Selecione uma grupo!")]
        public string grupo_id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTimeOffset? datalancamento { get; set; }

        public string tempo { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double? rating { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public double? audiencia { get; set; }

        //[Required(ErrorMessage = "Selecione um áudio!")]
        public string audio_url { get; set; }

        public bool habilitado { get; set; }
    }

    public class mpc_podcastsCSV
    {
        public string nome { get; set; }

        public string categoria { get; set; }

        public string descricao { get; set; }

        public string patrocinador { get; set; }

        public string grupo { get; set; }

        public string data { get; set; }

        public string tempo { get; set; }

        public string rating { get; set; }

        public string audiencia { get; set; }

    }


}
