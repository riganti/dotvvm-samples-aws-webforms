using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.WebCore.Pages
{
    public partial class HomePage : System.Web.UI.Page
    {
        private VtipBazeContext DataContext = new VtipBazeContext();
        private string RouteName { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.RouteName = (this.RouteData.DataTokens["RouteName"] as string) ?? string.Empty;
        }

        public IQueryable<Joke> SelectJokes()
        {
            var q = this.DataContext.Jokes.AsQueryable();

            if (this.RouteName.Equals("HomePage"))
            {
                q = q.Where(x => x.Approved);
                this.WelcomePlaceHolder.Visible = true;
            }
            else if (this.RouteName.Equals("SingleJoke"))
            {
                var jokeId = int.Parse(this.RouteData.Values["JokeId"] as string);
                q = q.Where(x => x.JokeId == jokeId);
                if (!this.Request.IsAuthenticated) q = q.Where(x => x.Approved);
                this.PagingPanel.Visible = false;
            }
            else if (this.RouteName.Equals("AdminHomePage"))
            {
                q = q.Where(x => !x.Approved);
            }
            else if (this.RouteName.Equals("TagSearch"))
            {
                var tagName = this.RouteData.Values["TagName"] as string;
                if (!string.IsNullOrWhiteSpace(tagName)) q = q.Where(x => x.Tags.Any(t => t.TagName.Equals(tagName)));
            }

            return q.OrderByDescending(x => x.DateCreated);
        }

        protected void JokesList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var jokeId = int.Parse(e.CommandArgument as string);
            if (e.CommandName.Equals("AddTag"))
            {
                var textbox = (e.CommandSource as Control).NamingContainer.FindControl("TextBoxNewTag") as TextBox;
                var tagText = textbox.Text.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(tagText)) return;

                var tag = this.DataContext.Tags.SingleOrDefault(x => x.TagName.Equals(tagText));
                if (tag == null) tag = this.DataContext.Tags.Add(new Tag { TagName = tagText });

                var joke = this.DataContext.Jokes.Single(x => x.JokeId == jokeId);
                joke.Tags.Add(tag);

                this.DataContext.SaveChanges();
            }
            else if (e.CommandName.Equals("ClearTags"))
            {
                var joke = this.DataContext.Jokes.Single(x => x.JokeId == jokeId);
                joke.Tags.Clear();
                this.DataContext.SaveChanges();
            }
            else if (e.CommandName.Equals("Approve"))
            {
                var joke = this.DataContext.Jokes.Single(x => x.JokeId == jokeId);
                joke.Approved = true;
                this.DataContext.SaveChanges();
            }
            else if (e.CommandName.Equals("Reject"))
            {
                var joke = this.DataContext.Jokes.Single(x => x.JokeId == jokeId);
                this.DataContext.Jokes.Remove(joke);
                this.DataContext.SaveChanges();
            }
        }

    }
}