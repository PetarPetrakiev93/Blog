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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("ListAlbums");
        }
        public ActionResult ListAlbums()
        {
            using (var database = new BlogDbContext())
            {
                var albums = database.Albums.Include(a => a.Images).OrderBy(a => a.Name).ToList();

                return View(albums);
            }
        }

        public ActionResult ListImages(int? albumId)
        {
            if (albumId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var images = database.Images.Where(a => a.AlbumId == albumId).Include(a => a.Author).Include(i => i.Tags).ToList();

                return View(images);
            }
        }

    }
}