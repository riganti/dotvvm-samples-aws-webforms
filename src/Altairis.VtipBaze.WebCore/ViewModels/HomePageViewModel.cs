using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Controls;
using Altairis.VtipBaze.WebCore.Model;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class HomePageViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;

        public override string PageTitle => "VtipBáze";

        public bool ShowHeader { get; set; } = false;
        public bool ShowPager { get; set; } = true;

        public GridViewDataSet<JokeListModel> Jokes { get; set; }

        [FromRoute("PageIndex")]
        public int CurrentPageIndex { get; set; } = 1;

        [FromRoute("JokeId")]
        public int? JokeId { get; set; }

        [FromRoute("TagName")]
        public string TagName { get; set; }

        public List<string> TagNames { get; set; }

        public HomePageViewModel(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Task PreRender()
        {
            Jokes = new GridViewDataSet<JokeListModel>()
            {
                PagingOptions =
                {
                    PageSize = 6,
                    PageIndex = CurrentPageIndex - 1
                }
            };
            Jokes.LoadFromQueryable(SelectJokes());

            TagNames = SelectTags().ToList();

            return base.PreRender();
        }

        public IQueryable<string> SelectTags()
        {
            return dbContext.Tags
                .OrderBy(x => x.TagName)
                .Select(x => x.TagName);
        }

        public IQueryable<JokeListModel> SelectJokes()
        {
            var q = dbContext.Jokes.AsQueryable();

            if (Context.Route.RouteName.Equals("HomePage"))
            {
                q = q.Where(x => x.Approved);
                ShowHeader = true;
            }
            else if (Context.Route.RouteName.Equals("SingleJoke"))
            {
                q = q.Where(x => x.JokeId == JokeId.Value);
                if (!Context.HttpContext.User.Identity.IsAuthenticated) q = q.Where(x => x.Approved);
                ShowPager = false;
            }
            else if (Context.Route.RouteName.Equals("AdminHomePage"))
            {
                q = q.Where(x => !x.Approved);
            }
            else if (Context.Route.RouteName.Equals("TagSearch"))
            {
                if (!string.IsNullOrWhiteSpace(TagName)) q = q.Where(x => x.Tags.Any(t => t.TagName.Equals(TagName)));
            }

            return q.OrderByDescending(x => x.DateCreated)
                .Select(x => new JokeListModel()
                {
                    Id = x.JokeId,
                    Text = x.Text,
                    DateCreated = x.DateCreated,
                    Approved = x.Approved,
                    Tags = x.Tags.Select(t => new TagListModel()
                    {
                        TagName = t.TagName
                    })
                });
        }

        public void ApproveJoke(int jokeId)
        {
            var joke = dbContext.Jokes.Single(x => x.JokeId == jokeId);
            joke.Approved = true;
            dbContext.SaveChanges();
        }

        public void RejectJoke(int jokeId)
        {
            var joke = dbContext.Jokes.Single(x => x.JokeId == jokeId);
            dbContext.Jokes.Remove(joke);
            dbContext.SaveChanges();
        }

        public void AddTag(int jokeId, string adminNewTag)
        {
            var tagText = adminNewTag.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(tagText)) return;

            var tag = dbContext.Tags.SingleOrDefault(x => x.TagName.Equals(tagText));
            if (tag == null) tag = dbContext.Tags.Add(new Tag { TagName = tagText });

            var joke = dbContext.Jokes.Single(x => x.JokeId == jokeId);
            joke.Tags.Add(tag);

            dbContext.SaveChanges();
        }

        public void ClearTags(int jokeId)
        {
            var joke = dbContext.Jokes.Single(x => x.JokeId == jokeId);
            joke.Tags.Clear();
            dbContext.SaveChanges();
        }

    }
}

