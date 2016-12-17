using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

        public int AlbumId { get; set; }

        public ICollection<Album> Albums { get; set; }

        private readonly BlogDbContext db = new BlogDbContext();
        public int UploadImageInDataBase(HttpPostedFileBase file, ImagesViewModel contentViewModel)
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