using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var comments = database.Comments.ToList();
                return View(comments);
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
                var comment = database.Comments.FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return HttpNotFound();
                }

                return View(comment);
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new BlogDbContext())
            {
                var comment = database.Comments.FirstOrDefault(c => c.Id == id);

                database.Comments.Remove(comment);
                database.SaveChanges();

                return RedirectToAction("List", "Comment");
            }
        }
    }
}