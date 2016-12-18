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
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult List(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var images = database.Tags.Include(t => t.Images.Select(i => i.Tags))
                    .Include(t => t.Images.Select(i => i.Author))
                    .FirstOrDefault(t => t.Id == id)
                    .Images
                    .ToList();

                return View(images);
            }
            
        }
    }
}