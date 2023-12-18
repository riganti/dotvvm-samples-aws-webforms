using DotVVM.Framework.ViewModel;
using System.Web.Security;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public abstract class SiteViewModel : DotvvmViewModelBase
    {
        public abstract string PageTitle { get; }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            Context.RedirectToLocalUrl("/");
        }
    }
}

