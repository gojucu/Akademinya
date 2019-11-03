using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Akademinya.Models
{
    public partial class model_kurs_liste
    {
        //kurs
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Acikklama { get; set; }
        public string Icerik { get; set; }
        public Nullable<decimal> Fiyat { get; set; }
        public Nullable<System.DateTime> SonGuncellemeTarih { get; set; }
        public Nullable<int> KategoriID { get; set; }
        public Nullable<int> UstKategoriID { get; set; }
        public string Resim { get; set; }
        public Nullable<bool> Ücretsiz { get; set; }



        //ürün Fatura
        public int uyekursID { get; set; }
        public int KursID { get; set; }
        public int UyeID { get; set; }
        public Nullable<bool> Aktif { get; set; }


    }
}