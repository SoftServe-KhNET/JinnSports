using System.Web.Optimization;

namespace JinnSports.WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/MainStyles")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/AdminLTE/AdminLTE.css")
                .Include("~/Content/AdminLTE/skins/skin-blue.css"));

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

        }
    }
}