using System;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting;
using System.Web.Security;
using Altairis.VtipBaze.Data;
using Altairis.Web;

namespace Altairis.VtipBaze.WebCore.Pages
{
    public partial class NewJoke : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (!this.IsValid) return;

            using (var dc = new VtipBazeContext())
            {
                var joke = dc.Jokes.Add(new Joke
                {
                    Text = this.TextTextBox.Text,
                    Approved = this.Request.IsAuthenticated
                });
                dc.SaveChanges();

                if (this.Request.IsAuthenticated)
                {
                    // Published directly
                    this.Response.RedirectToRoute("SingleJoke", new { JokeId = joke.JokeId });
                }
                else
                {
                    // Waiting for approval
                    this.MultiViewPage.SetActiveView(this.ViewResult);

                    // Send message to users
                    var recipients = from u in Membership.GetAllUsers().Cast<MembershipUser>()
                                     where u.IsApproved
                                     select u.Email;
                    var client = new System.Net.Mail.SmtpClient();
                    var message = new System.Net.Mail.MailMessage()
                    {
                        Subject = "New joke to approve",
                        Body = joke.Text + "\r\n\r\nApprove or reject at " + this.Request.ApplicationBaseUri() + "admin",
                        IsBodyHtml = false
                    };
                    message.To.Add(string.Join(",", recipients));
                    client.Send(message);
                }
            }
        }
    }
}