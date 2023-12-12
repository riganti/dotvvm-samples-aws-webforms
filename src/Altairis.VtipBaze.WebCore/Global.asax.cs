using System;
using System.Web.Routing;
using Altairis.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace Altairis.VtipBaze.WebCore
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Enable JS + CSS bundling
            BundleTable.Bundles.Add(new StyleBundle("~/Styles/css").IncludeDirectory("~/Styles", "*.css"));
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/Site/js").IncludeDirectory("~/Scripts/Site", "*.js"));
            ScriptManager.ScriptResourceMapping.AddDefinition("site", new ScriptResourceDefinition { Path = "~/Scripts/Site/js" });

            // Setup URL routing
            RouteTable.Routes.MapPageRoute("HomePage", "{PageIndex}", "~/Pages/HomePage.aspx", false,
                new RouteValueDictionary(new { PageIndex = "1" }),              // defaults
                new RouteValueDictionary(new { PageIndex = @"^[0-9]+$" }),      // constraints
                new RouteValueDictionary(new { RouteName = "HomePage" }));      // data tokens
            RouteTable.Routes.MapPageRoute("SingleJoke", "joke/{JokeId}", "~/Pages/HomePage.aspx", false,
                null,                                                           // defaults
                new RouteValueDictionary(new { JokeId = @"^[0-9]+$" }),         // constraints
                new RouteValueDictionary(new { RouteName = "SingleJoke" }));    // data tokens
            RouteTable.Routes.MapPageRoute("TagSearch", "tags/{TagName}/{PageIndex}", "~/Pages/HomePage.aspx", false,
                new RouteValueDictionary(new { PageIndex = "1" }),              // defaults
                new RouteValueDictionary(new { PageIndex = @"^[0-9]+$" }),      // constraints
                new RouteValueDictionary(new { RouteName = "TagSearch" }));     // data tokens
            RouteTable.Routes.MapPageRoute("AdminHomePage", "admin/{PageIndex}", "~/Pages/HomePage.aspx", false,
                new RouteValueDictionary(new { PageIndex = "1" }),              // default
                new RouteValueDictionary(new { PageIndex = @"^[0-9]+$" }),      // constraints
                new RouteValueDictionary(new { RouteName = "AdminHomePage" })); // data tokens
            RouteTable.Routes.MapPageRoute("NewJoke", "new", "~/Pages/NewJoke.aspx");
            RouteTable.Routes.MapPageRoute("TagList", "tags", "~/Pages/TagList.aspx");
            RouteTable.Routes.MapPageRoute("Login", "login", "~/Pages/Login.aspx");
            RouteTable.Routes.MapHttpHandler<Handlers.TagListHandler>("tags.txt");
            RouteTable.Routes.MapHttpHandler<Handlers.FeedHandler>("feed.xml");
        }

    }
}