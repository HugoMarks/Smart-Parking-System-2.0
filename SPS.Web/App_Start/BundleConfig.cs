using System.Web;
using System.Web.Optimization;

namespace SPS.Web
{
    public class BundleConfig
    {
        // Para obter mais informações sobre agrupamento, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

			bundles.Add(new StyleBundle("~/bundles/register").Include(
						"~/Scripts/register.js"));

			// Use a versão em desenvolvimento do Modernizr para desenvolver e aprender com ela. Após isso, quando você estiver
			// pronto para produção, use a ferramenta de compilação em http://modernizr.com para selecionar somente os testes que você precisa.
			//bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
			//            "~/Scripts/modernizr-*"));

			//bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
			//          "~/Scripts/bootstrap.js",
			//          "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.min.css",
                      "~/Content/site.css"));

            // Definir EnableOptimizations como false para depuração. Para obter mais informações,
            // visite http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
