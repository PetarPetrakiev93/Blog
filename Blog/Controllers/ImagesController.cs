using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ImagesController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            using (var database = new BlogDbContext())
            {
                var model = new ImagesViewModel();
                model.Albums = database.Albums.OrderBy(a => a.Name).ToList();
                return View(model);
            }
                
        }
        [Route("Create")]
        [HttpPost]
        [Authorize]
        public ActionResult Create(ImagesViewModel model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            ImagesViewModel service = new ImagesViewModel();
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                using (var database = new BlogDbContext())
                {
                    var authorId = database.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;
                    var image = new Images(authorId, model.Title, model.Description, model.Contents, model.Image, model.AlbumId);
                    this.SetImageTags(image, model, database);
                    database.Images.Add(image);
                    database.SaveChanges();

                    return RedirectToAction("List");
                }
                
            }
            return View(model);
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var content = database.Images.Include(a => a.Author).Include(i => i.Tags).ToList();
                return View(content);                
            }
                
        }

        private BlogDbContext db = new BlogDbContext();
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Images where temp.Id == Id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var imageDetails = database.Images.Where(i => i.Id == id).Include(i => i.Author).Include(i => i.Tags).First();
                if (imageDetails == null)
                {
                    return HttpNotFound();
                }

                return View(imageDetails);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var imageDetails = database.Images.Where(i => i.Id == id).Include(i => i.Author).Include(i =>i.Albums).First();

                if (!IsUserAuthorizedToEdit(imageDetails))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ViewBag.TagsString = string.Join(", ", imageDetails.Tags.Select(t => t.Name));
                if (imageDetails == null)
                {
                    return HttpNotFound();
                }

                return View(imageDetails);
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var imageDetails = database.Images.Where(i => i.Id == id).Include(i => i.Author).First();
                var comments = imageDetails.Comments.ToList();
                foreach (var comment in comments)
                {
                    database.Comments.Remove(comment);
                }
                if (imageDetails == null)
                {
                    return HttpNotFound();
                }
                database.Images.Remove(imageDetails);
                database.SaveChanges();

                return RedirectToAction("List");
            }
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var image = database.Images.Where(i => i.Id == id).First();
                if (!IsUserAuthorizedToEdit(image))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                if (image == null)
                {
                    return HttpNotFound();
                }
                var model = new ImagesViewModel();
                model.Id = image.Id;
                model.Title = image.Title;
                model.Contents = image.Contents;
                model.Description = image.Description;
                model.AlbumId = image.AlbumId;
                model.Albums = database.Albums.OrderBy(a => a.Name).ToList();
                model.Tags = string.Join(", ", image.Tags.Select(t => t.Name));
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(ImagesViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var image = database.Images.FirstOrDefault(i => i.Id == model.Id);

                    image.Title = model.Title;
                    image.Contents = model.Contents;
                    image.Description = model.Description;
                    image.AlbumId = model.AlbumId;
                    this.SetImageTags(image, model, database);

                    database.Entry(image).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("List");
                }
            }
            return View(model);
        }

        private void SetImageTags(Images image, ImagesViewModel model, BlogDbContext database)
        {
            var tagsString = model.Tags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToLower()).Distinct();
            image.Tags.Clear();
            foreach (var tagString in tagsString)
            {
                Tag tag = database.Tags.FirstOrDefault(t => t.Name.Equals(tagString));

                if (tag == null)
                {
                    tag = new Tag() { Name = tagString };
                    database.Tags.Add(tag);

                    image.Tags.Add(tag);
                }
            }
        }

        private bool IsUserAuthorizedToEdit(Images image)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = image.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }

        public ActionResult _Comment(int? id)
        {
            using (var database = new BlogDbContext())
            {
                var comment = database.Comments.Where(c => c.ImageId == id).ToList();

                return View(comment);
            }
        }

        public ActionResult _CommentCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _CommentCreate(Comment comment, int id)
        {
            using (var database = new BlogDbContext())
            {
                comment.ImageId = id;
                comment.Time = DateTime.Now.ToString();
                database.Comments.Add(comment);
                database.SaveChanges();

                return RedirectToAction("List");
            }

        }
        
    }
}