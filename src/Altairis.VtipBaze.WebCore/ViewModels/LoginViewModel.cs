using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class LoginViewModel : SiteViewModel
    {
        public override string PageTitle => "Sign In";

        public bool IsError { get; set; }
        public string FailureText { get; set; }


        [Required(ErrorMessage = "No user name provided")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "No password provided")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public void SignIn()
        {
            if (!Membership.ValidateUser(UserName, Password))
            {
                IsError = true;
                FailureText = "Invalid user credentials!";
                return;
            }

            FormsAuthentication.SetAuthCookie(UserName, RememberMe);
            var redirectUrl = FormsAuthentication.GetRedirectUrl(UserName, RememberMe);
            Context.RedirectToLocalUrl(redirectUrl);
        }
    }
}

