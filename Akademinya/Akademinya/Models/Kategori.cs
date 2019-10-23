namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kategori")]
    public partial class Kategori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategori()
        {
            Kurs = new HashSet<Kurs>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ad { get; set; }

        public int UstId { get; set; }

        public bool Silindi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kurs> Kurs { get; set; }
    }
}
