
//siteye giriş yapmadı ise kutu katılma kısmı
var uyariVer = function (yazi, sayfaYenile) {
    bootbox.alert({
        message: yazi,
        callback: function () {
            if (sayfaYenile === true) {
                //window.location.reload(true);
                window.location
            }
        }
    })
}

var islemler = {
    GirisYap: function () {
            uyariVer("Lütfen <a href='/UyeGirisi'> Üye Girişi </a> yapınız. <br> yada  <a href='/UyeOl'> Yeni üyelik </a> oluşturunuz ", true)
    },
    OdemeYap: function () {

        //$("#katil").prop('disabled', true);

        var kayitAdi = $("#KayitAdi").val();
        var KartNo = $("#Kartno").val();
        var kartUstundekiIsım = $("#KartUstundekiIsım").val();
        var ay = $("#Ay").val();
        var yil = $("#Yil").val();
        var cvc = $("#Cvc").val();
        var kaydet = $("#Kaydet").is(":checked") ? true : false;
        var sepet = []
        var hey = $(".sepetID").val();

        $(".sepetID").each(function (index, value) {
            sepet.push($(this).val());
            alert(sepet)
        });
        
        var kart = {
            kayitAdi: kayitAdi,
            KartNo: KartNo,
            kartUstundekiIsım: kartUstundekiIsım,
            ay: ay,
            yil: yil,
            cvc: cvc
        }

        $.ajax({
            url: "Odeme",
            async: false,
            data: { kaydet, kart,sepet },
            type: "post",

            success: function (sonuc) {
                if (sonuc) {
                    window.location.reload(true);
                }
                else {
                    bootbox.alert({
                        message: "Kartınız geçersiz. Lütfen tekrar deneyiniz.",
                        size: 'small'
                    });
                    //$('#katil').removeAttr("disabled");
                }
            }

        })
    },

}