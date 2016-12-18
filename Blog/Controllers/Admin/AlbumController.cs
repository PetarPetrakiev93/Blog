using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers.Admin
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var albums = database.Albums.OrderBy(a => a.Id).ToList();

                return View(albums);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    database.Albums.Add(album);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(album);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var album = database.Albums.FirstOrDefault(c => c.Id == id);
                if (album == null)
                {
                    return HttpNotFound();
                }

                return View(album);
            }
        }

        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    database.Entry(album).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(album);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new BlogDbContext())
            {
                var album = database.Albums.FirstOrDefault(c => c.Id == id);
                if (album == null)
                {
                    return HttpNotFound();
                }

                return View(album);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new BlogDbContext())
            {
                var album = database.Albums.FirstOrDefault(a => a.Id == id);

                var albumImages = album.Images.ToList();

                foreach (var image in albumImages)
                {
                    var comments = image.Comments.ToList();
                    foreach(var comment in comments)
                    {
                        database.Comments.Remove(comment);
                    }
                    database.Images.Remove(image);
                }

                database.Albums.Remove(album);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}