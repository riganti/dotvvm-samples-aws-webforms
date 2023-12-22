using Altairis.VtipBaze.Data;
using DotVVM.Framework.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.VtipBaze.WebCore.Handlers
{
    public class FeedPresenter : IDotvvmPresenter
    {
        private readonly VtipBazeContext dbContext;

        public FeedPresenter(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task ProcessRequest(IDotvvmRequestContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";
            context.HttpContext.Response.Headers["Cache-Control"] = "private";
            context.HttpContext.Response.Headers["Expires"] = DateTime.UtcNow.AddMonths(1).ToString("R");

            var baseUri = context.GetApplicationBaseUri();
            var feed = new SyndicationFeed
            {
                Id = baseUri.ToString(),
                Title = new TextSyndicationContent("VTIPBÁZE.CZ"),
                Description = new TextSyndicationContent("Newest jokes"),
                Items = GetFeedItems(baseUri)
            };
            feed.Links.Add(new SyndicationLink(baseUri));
            using (var xw = new System.Xml.XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                xw.Formatting = System.Xml.Formatting.Indented;
                var ff = new Atom10FeedFormatter(feed);
                ff.WriteTo(xw);
            }
            return Task.CompletedTask;
        }

        public IEnumerable<SyndicationItem> GetFeedItems(Uri baseUri)
        {
            foreach (var j in dbContext.Jokes.Where(x => x.Approved).OrderByDescending(x => x.DateCreated).Take(10))
            {
                var itemUri = new UriBuilder(baseUri);
                itemUri.Path = "/joke/" + j.JokeId;

                var si = new SyndicationItem
                {
                    Title = new TextSyndicationContent(j.DateCreated.ToString()),
                    Content = new TextSyndicationContent(j.Text, TextSyndicationContentKind.Plaintext),
                    PublishDate = j.DateCreated,
                    LastUpdatedTime = j.DateCreated,
                    Id = itemUri.Uri.ToString(),
                };
                si.AddPermalink(itemUri.Uri);
                foreach (var tag in j.Tags)
                {
                    si.Categories.Add(new SyndicationCategory(tag.TagName));
                }
                yield return si;
            }
        }
    }
}