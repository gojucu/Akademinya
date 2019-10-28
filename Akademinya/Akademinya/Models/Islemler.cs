namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Islemler")]
    public partial class Islemler
    {
        public int Id { get; set; }

        public Guid UyeID { get; set; }

        public int KursID { get; set; }

        public int? KartID { get; set; }

        public DateTime IslemTarihi { get; set; }

        public virtual Kurs Kurs { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
