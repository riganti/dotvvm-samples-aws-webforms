using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Altairis.VtipBaze.WebCore.Model
{
    public class JokeListModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<TagListModel> Tags { get; set; }
        public bool Approved { get; set; }
        public string AdminNewTag { get; set; }
    }
}