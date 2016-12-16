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
        [Key]
        public int Id { get; set; }

        
        public string Title { get; set; }

        public string Description { get; set; }

        [AllowHtml]
        public string Contents { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        private readonly BlogDbContext db = new BlogDbContext();
        public int UploadImageInDataBase(HttpPostedFileBase file, Images contentViewModel)
        {
            contentViewModel.Image = ConvertToBytes(file);

            var Content = new Images
            {
                Title = contentViewModel.Title,
                Description = contentViewModel.Description,
                Contents = contentViewModel.Contents,
                Image = contentViewModel.Image,
                
            };
            
            
            db.Images.Add(Content);

            return 1;
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}