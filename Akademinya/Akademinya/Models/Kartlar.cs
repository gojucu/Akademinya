namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kartlar")]
    public partial class Kartlar
    {
        public int Id { get; set; }

        public Guid UyeID { get; set; }

        [Required]
        [StringLength(50)]
        public string KayitAdi { get; set; }

        [Required]
        [StringLength(20)]
        public string KartNo { get; set; }

        [Required]
        [StringLength(50)]
        public string KartUstundekiIsım { get; set; }

        public int Ay { get; set; }

        public int Yil { get; set; }

        [Required]
        [StringLength(10)]
        public string Cvc { get; set; }

        public bool Silindi { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
