using System;
using System.Linq;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.WebCore.Pages
{
    public partial class TagList : System.Web.UI.Page
    {
        VtipBazeContext DataContext = new VtipBazeContext();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Tag> SelectTags()
        {
            return this.DataContext.Tags.OrderBy(x => x.TagName);
        }
    }
}