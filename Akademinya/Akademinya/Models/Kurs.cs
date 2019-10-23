namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Kurs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kurs()
        {
            Uye = new HashSet<Uye>();
        }

        public int Id { get; set; }

        public bool Silindi { get; set; }

        [Required]
        [StringLength(50)]
        public string Ad { get; set; }

        [StringLength(50)]
        public string Acikklama { get; set; }

        public string Icerik { get; set; }

        [Required]
        [StringLength(50)]
        public string KursSahibi { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SonGuncellemeTarih { get; set; }

        public int? KategoriID { get; set; }

        public string Resim { get; set; }

        public virtual Kategori Kategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uye> Uye { get; set; }
    }
}
