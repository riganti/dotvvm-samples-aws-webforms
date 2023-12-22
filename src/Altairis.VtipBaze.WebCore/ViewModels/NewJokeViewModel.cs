using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class NewJokeViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;
        private readonly SmtpClient smtpClient;

        public override string PageTitle => "Add your joke";

        public bool IsSubmitted { get; set; }

        [Required(ErrorMessage = "Empty text is not very funny")]
        public string JokeText { get; set; }

        public NewJokeViewModel(VtipBazeContext dbContext, SmtpClient smtpClient)
        {
            this.dbContext = dbContext;
            this.smtpClient = smtpClient;
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
                Context.RedirectToRouteHybrid("SingleJoke", new { JokeId = joke.Entity.JokeId });
            }
            else
            {
                // Waiting for approval
                IsSubmitted = true;

                // Send message to users
                var recipients = dbContext.Users.Select(u => u.Email);
                var message = new System.Net.Mail.MailMessage()
                {
                    Subject = "New joke to approve",
                    Body = joke.Entity.Text + "\r\n\r\nApprove or reject at " + Context.GetApplicationBaseUri() + "admin",
                    IsBodyHtml = false
                };
                message.To.Add(string.Join(",", recipients));
                smtpClient.Send(message);
            }
        }
    }
}

