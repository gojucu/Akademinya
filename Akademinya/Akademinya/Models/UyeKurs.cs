//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Akademinya.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UyeKurs
    {
        public int Id { get; set; }
        public int KursID { get; set; }
        public int UyeID { get; set; }
        public Nullable<bool> Aktif { get; set; }
        public string KursYorum { get; set; }
        public Nullable<int> KursPuan { get; set; }
        public System.DateTime DegerlendirmeTarihi { get; set; }
        public Nullable<bool> PuanVerdi { get; set; }
    
        public virtual Kurs Kurs { get; set; }
        public virtual Uye Uye { get; set; }
    }
}
