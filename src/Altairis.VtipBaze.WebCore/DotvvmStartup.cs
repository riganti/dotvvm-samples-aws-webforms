using Altairis.VtipBaze.Data;
using Altairis.VtipBaze.WebCore.Handlers;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.VtipBaze.WebCore
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            config.AddWebFormsAdapters();

            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);

            config.Styles.Register<Button>().AddClass("button");
            config.Styles.Register<TextBox>().AddClass("textbox");
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            // register routes   
            config.RouteTable.Add("TagList", "tags", "Views/TagList.dothtml");
            config.RouteTable.Add("NewJoke", "new", "Views/NewJoke.dothtml");

            config.RouteTable.Add("HomePage", "{PageIndex:int}", "Views/HomePage.dothtml", new { PageIndex = 1 });
            config.RouteTable.Add("SingleJoke", "joke/{JokeId}", "Views/HomePage.dothtml");
            config.RouteTable.Add("TagSearch", "tags/{TagName}/{PageIndex:int}", "Views/HomePage.dothtml", new { PageIndex = 1 });
            config.RouteTable.Add("AdminHomePage", "admin/{PageIndex:int}", "Views/HomePage.dothtml", new { PageIndex = 1 });

            config.RouteTable.Add("Login", "login", "Views/Login.dothtml");

            config.RouteTable.Add("Feed", "feed.xml", presenterType: typeof(FeedPresenter));
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.RegisterStylesheetUrl("jquery-ui-css", "https://ajax.aspnetcdn.com/ajax/jquery.ui/1.10.3/themes/south-street/jquery-ui.css", null);
            config.Resources.RegisterStylesheetFile("site-css", "Styles/main.min.css", dependencies: new[] { "jquery-ui-css" });

            config.Resources.RegisterScriptFile("site", "Scripts/Site/ui.js", dependencies: new[] { "jquery-ui" });
            config.Resources.RegisterScriptFile("jquery-ui", "Scripts/jquery-ui-1.10.3.min.js", dependencies: new[] { "jquery" });
            config.Resources.RegisterScriptFile("jquery", "Scripts/jquery-2.0.3.min.js");
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.AddDefaultTempStorages("temp");
            options.Services.AddScoped<VtipBazeContext>();
        }
    }
}
