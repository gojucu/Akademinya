namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uye")]
    public partial class Uye
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uye()
        {
            Kurs = new HashSet<Kurs>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [Required]
        [StringLength(50)]
        public string Mail { get; set; }

        public virtual aspnet_Membership aspnet_Membership { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kurs> Kurs { get; set; }
    }
}
