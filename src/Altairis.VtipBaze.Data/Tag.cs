using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Altairis.VtipBaze.Data
{

    public class Tag
    {

        [Key]
        [RegularExpression(@"^\w+$")]
        public string TagName { get; set; }

        public virtual ICollection<Joke> Jokes { get; set; }

    }

}
