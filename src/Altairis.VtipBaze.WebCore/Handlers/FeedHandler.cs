using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using Altairis.VtipBaze.Data;
using Altairis.Web;

namespace Altairis.VtipBaze.WebCore.Handlers
{
    public class FeedHandler : IHttpHandler
    {

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/atom+xml";
            context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));
            var baseUri = context.Request.ApplicationBaseUri();
            var feed = new SyndicationFeed
            {
                Id = baseUri.ToString(),
                Title = new TextSyndicationContent("VTIPBÁZE.CZ"),
                Description = new TextSyndicationContent("Newest jokes"),
                Items = GetFeedItems(baseUri)
            };
            feed.Links.Add(new SyndicationLink(baseUri));
            using (var xw = new System.Xml.XmlTextWriter(context.Response.Output))
            {
                xw.Formatting = System.Xml.Formatting.Indented;
                var ff = new Atom10FeedFormatter(feed);
                ff.WriteTo(xw);
            }
        }

        public IEnumerable<SyndicationItem> GetFeedItems(Uri baseUri)
        {
            using (var dc = new VtipBazeContext())
            {
                foreach (var j in dc.Jokes.Where(x => x.Approved).OrderByDescending(x => x.DateCreated).Take(10))
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
}
