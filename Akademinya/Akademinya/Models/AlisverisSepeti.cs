namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlisverisSepeti")]
    public partial class AlisverisSepeti
    {
        public int Id { get; set; }

        public Guid? UyeID { get; set; }

        public int? KursID { get; set; }

        public string Guid { get; set; }

        public virtual Kurs Kurs { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
