using System.Web.Optimization;

namespace JinnSports.WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var googleChartsCdnPath = "https://www.gstatic.com/charts/loader.js";
            var icheckCdnPath = "https://almsaeedstudio.com/themes/AdminLTE/plugins/iCheck/icheck.min.js";

            bundles.Add(new StyleBundle("~/Content/MainStyles")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/AdminLTE/AdminLTE.css")
                .Include("~/Content/AdminLTE/skins/skin-blue.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/themes/base/all.css")
                .Include("~/Content/auth-style.css")
                .Include("~/Content/style.css")
                .Include("~/Content/header.css")); 

            bundles.Add(new StyleBundle("~/Content/DataTableStyles")
                .Include("~/Content/DataTables/css/dataTables.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery-ui")
                .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/AdminLTETemplate")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/AdminLTE/app.js"));

            bundles.Add(new ScriptBundle("~/Scripts/DataTable")
                .Include("~/Scripts/DataTables/jquery.dataTables.min.js")
                .Include("~/Scripts/DataTables/dataTables.bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/Jinn/core/extensions.js",
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").IncludeDirectory(
                "~/Scripts/Jinn", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Jinn/*.css"));

            bundles.Add(new ScriptBundle("~/Scripts/GoogleCharts", googleChartsCdnPath)
                .Include("~/Scripts/loader.js"));

            bundles.Add(new ScriptBundle("~/Scripts/iCheck", icheckCdnPath)
                .Include("~/Scripts/icheck.min.js"));
        }
    }
}