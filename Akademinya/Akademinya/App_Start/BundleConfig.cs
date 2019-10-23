using System.Web;
using System.Web.Optimization;

namespace Akademinya
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
              .Include("~/Content/bootstrap.css")
              .Include("~/Content/bower_components/datatables.net-bs/css/datatables.min.css")
              );


            bundles.Add(new StyleBundle("~/bundles/AdminCss")
                          .Include("~/Content/bower_components/bootstrap/dist/css/bootstrap.min.css")
                          //.Include("~/Content/all.css")
                          .Include("~/Content/AdminContent/css/Admin.css")
                          );

            bundles.Add(new ScriptBundle("~/bundles/adminJs").Include(
            //"~/Content/bower_components/jquery/dist/jquery.min.js",
            "~/Content/bower_components/jquery-ui/jquery-ui.min.js",
            "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
            "~/Scripts/popper.js",
            "~/Content/bower_components/raphael/raphael.min.js",
            "~/Content/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
            "~/Content/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
            "~/Content/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
            "~/Content/bower_components/jquery-knob/dist/jquery.knob.min.js",
            "~/Content/bower_components/moment/min/moment.min.js",
            "~/Content/bower_components/bootstrap-daterangepicker/daterangepicker.js",
            "~/Content/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
            "~/Content/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
            "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
            "~/Content/bower_components/fastclick/lib/fastclick.js",
            "~/Content/dist/js/adminlte.min.js",
            "~/Content/dist/js/pages/dashboard.js",
            "~/Scripts/bootbox.min.js",
            "~/Content/dist/js/demo.js",
            "~/Content/js/main.js"
            ));

            bundles.UseCdn = true;
        }
    }
}
