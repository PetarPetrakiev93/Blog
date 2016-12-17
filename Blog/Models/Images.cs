using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class Images
    {
        public Images()
        {

        }
        public Images(string authorId, string title, string description, string contents, byte[] image, int albumId)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Description = description;
            this.Contents = contents;
            this.Image = image;
            this.AlbumId = albumId;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [AllowHtml]
        public string Contents { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Albums")]
        public int AlbumId { get; set; }

        public virtual Album Albums { get; set; }

        

        public bool IsAuthor (string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}