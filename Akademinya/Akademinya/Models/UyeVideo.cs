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
    
    public partial class UyeVideo
    {
        public int Id { get; set; }
        public Nullable<int> VideoID { get; set; }
        public Nullable<int> UyeID { get; set; }
        public Nullable<bool> Izlendi { get; set; }
        public string KalinanZaman { get; set; }
    
        public virtual Uye Uye { get; set; }
        public virtual Video Video { get; set; }
    }
}
