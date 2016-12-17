using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ImagesViewModel
    {
        
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        
        public string Contents { get; set; }

        
        public byte[] Image { get; set; }

        
        public string AuthorId { get; set; }
    }
}