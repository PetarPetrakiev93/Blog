using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ImagesController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(Images model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            Images service = new Images();
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                using (var database = new BlogDbContext())
                {
                    var authorId = database.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;
                    model.AuthorId = authorId;
                    database.Images.Add(model);
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
                var content = database.Images.Include(a => a.Author).ToList();
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


    }
}