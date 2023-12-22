using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public abstract class SiteViewModel : DotvvmViewModelBase
    {
        public abstract string PageTitle { get; }

        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(IdentityConstants.ApplicationScheme);
            Context.RedirectToLocalUrl("/");
        }
    }
}

