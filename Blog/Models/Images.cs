﻿using System;
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
        private ICollection<Tag> tags;

        private ICollection<Comment> comments;

        public Images()
        {
            this.tags = new HashSet<Tag>();
            this.comments = new HashSet<Comment>();
        }
        public Images(string authorId, string title, string description, string contents, byte[] image, int albumId)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Description = description;
            this.Contents = contents;
            this.Image = image;
            this.AlbumId = albumId;
            this.tags = new HashSet<Tag>();
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

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        public virtual ICollection<Comment> Comments { get; set; }

        public bool IsAuthor (string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}