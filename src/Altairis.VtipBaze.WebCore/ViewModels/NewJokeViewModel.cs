using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.ComponentModel.DataAnnotations;
using Altairis.VtipBaze.Data;
using System.Web.Security;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class NewJokeViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;

        public override string PageTitle => "Add your joke";

        public bool IsSubmitted { get; set; }

        [Required(ErrorMessage = "Empty text is not very funny")]
        public string JokeText { get; set; }

        public NewJokeViewModel(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Submit()
        {
            var joke = dbContext.Jokes.Add(new Joke
            {
                Text = JokeText,
                Approved = Context.HttpContext.User.Identity.IsAuthenticated
            });
            dbContext.SaveChanges();

            if (Context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Published directly
                Context.RedirectToRouteHybrid("SingleJoke", new { JokeId = joke.JokeId });
            }
            else
            {
                // Waiting for approval
                IsSubmitted = true;

                // Send message to users
                var recipients = from u in Membership.GetAllUsers().Cast<MembershipUser>()
                                    where u.IsApproved
                                    select u.Email;
                var client = new System.Net.Mail.SmtpClient();
                var message = new System.Net.Mail.MailMessage()
                {
                    Subject = "New joke to approve",
                    Body = joke.Text + "\r\n\r\nApprove or reject at " + Context.GetApplicationBaseUri() + "admin",
                    IsBodyHtml = false
                };
                message.To.Add(string.Join(",", recipients));
                client.Send(message);
            }
        }
    }
}

