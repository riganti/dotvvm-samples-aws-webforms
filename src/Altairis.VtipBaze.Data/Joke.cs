using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.VtipBaze.Data
{
    public class Joke
    {

        public Joke()
        {
            this.DateCreated = DateTime.Now;
        }

        [Key]
        public int JokeId { get; set; }

        [Required, MaxLength]
        public string Text { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public bool Approved { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

    }
}
