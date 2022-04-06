namespace BlogSystem.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStyles(bundles);
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/css/bootstrap.css"));

            //bundles.Add(new StyleBundle("~/Content/tether").Include("~/Content/tether/tether.css"));

            bundles.Add(new StyleBundle("~/Content/fonts").Include("~/css/font-awesome/css/fontawesome.css"));

            bundles.Add(new StyleBundle("~/Content/main").Include("~/css/main.css"));

            bundles.Add(new ScriptBundle("~/Content/dropzone").Include("~/scripts/dropzone/dropzone.css"));

            //bundles.Add(new StyleBundle("~/Content/users").Include("~/Content/users.css"));

            //bundles.Add(new StyleBundle("~/Content/administration").Include("~/Content/administration.css"));
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/tether").Include("~/Scripts/tether.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/scripts/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include("~/scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include("~/scripts/dropzone/dropzone.js"));

            //bundles.Add(new ScriptBundle("~/bundles/blog").Include("~/Scripts/blog.js"));
        }
    }
}