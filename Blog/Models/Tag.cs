using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Tag
    {
        private ICollection<Images> images;

        public Tag()
        {
            this.images = new HashSet<Images>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Images> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }


    }
}