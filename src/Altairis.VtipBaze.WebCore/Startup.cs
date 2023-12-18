using DotVVM.Framework.Routing;
using Microsoft.Owin;
using Owin;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(Altairis.VtipBaze.WebCore.Startup))]
namespace Altairis.VtipBaze.WebCore
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // use DotVVM
            var applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(applicationPhysicalPath);
            dotvvmConfiguration.AssertConfigurationIsValid();
        }
    }
}
