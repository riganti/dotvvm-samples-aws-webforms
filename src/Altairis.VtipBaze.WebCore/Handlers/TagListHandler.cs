using System;
using System.Linq;
using System.Web;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.WebCore.Handlers
{
    public class TagListHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            using (var dc = new VtipBazeContext())
            {
                var q = from t in dc.Tags
                        orderby t.TagName
                        select t.TagName;
                context.Response.Write(string.Join(",", q));
            }

            context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(20));
        }
    }
}
