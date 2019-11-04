
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
    UyeGirisi: function () {

        //$("#katil").prop('disabled', true);

        var kullaniciAdi = $("#KullaniciAdi").val();
        var Sifre = $("#password").val();

        $.ajax({
            url: "UyeGirisi",
            async: false,
            data: { kullaniciAdi, Sifre },
            type: "post",
            success: function (sonuc) {
                if (sonuc) {
                    window.location.href = "";
                }
                else {
                    bootbox.alert({
                        message: "Kullanıcı adı yada şifre  Hatalı"
                    });
                    //$('#katil').removeAttr("disabled");
                }
            }
        })
    },
    UyeOl: function () {

        //$("#katil").prop('disabled', true);

        var kullaniciAdi = $("#KullaniciAdi").val();
        var mail = $("#Email").val();
        var telefonNo = $("#TelNo").val();
        var Sifre = $("#password").val();
        var uye = {
            kullaniciAdi: kullaniciAdi,
            mail: mail,
            telefonNo: telefonNo,
            Sifre: Sifre,
        }

        $.ajax({
            url: "UyeOl",
            async: false,
            data: uye,
            type: "post",
            success: function (sonuc) {
                if (sonuc) {
                    window.location.href = "";
                }
                else {
                    bootbox.alert({
                        message: "Bu bilgilere ait kayıtlı kullanıcı bulunmakta. Lütfen tekrar deneyiniz!"
                    });
                    //$('#katil').removeAttr("disabled");
                }
            }
        })
    },
    SifreDegistir: function () {



        var EskiSifre = $("#password1").val();
        var YeniSifre = $("#password").val();


        $.ajax({
            url: "Profil",
            async: false,
            data: { EskiSifre, YeniSifre },
            type: "post",
            success: function (sonuc) {
                if (sonuc) {
                    window.location.href = "";
                }
                else {
                    bootbox.alert({
                        message: "Eski şifreniz hatalı. Tekrar deneyiniz."
                    });
                    //$('#katil').removeAttr("disabled");
                }
            }
        })
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
            data: { kaydet, kart, sepet },
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
    PuanVer: function (id) {


        var url = "/PuanVer/" + id; //$('#yeni').data('url');

        $.get(url, function (data) {
            var dialog = bootbox.dialog({
                title: '',
                message: data,
                size: 'large',

                onEscape: function () {
                    // you can do anything here you want when the user dismisses dialog
                    window.location.reload(true);
                },
                buttons: {
                    cancel: {
                        label: "İptal",
                        className: 'btn-danger',
                        callback: function () {
                            //console.log('Custom cancel clicked');

                            window.location.reload(true);

                        }
                    }
                }
            }).find("div.modal-body").css({
                "width": "100%", "height": "70%", "overflow-y": "scroll"
            }).find(".bootbox-body").css("margin-top", "15px");

            //$('#gameModal').modal('show');
        });


    },
    Puan: function () {
        var puan = $("#Rating").val();
        var yorum = $("#Comment").val();
        var uyeKursId = $("#UyeKursID").val();
        

        $.ajax({
            url: "/PuanVerPost",
            async: false,
            data: { puan, yorum, uyeKursId},
            type: "post",

            success: function (sonuc) {
                if (sonuc) {
                    window.location.reload(true);
                }
                else {
                    bootbox.alert({
                        message: "Puan verilemedi tekrar deneyiniz",
                        size: 'small'
                    });
                    //$('#katil').removeAttr("disabled");
                }
            }

        })
    }

}