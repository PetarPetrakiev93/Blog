using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Models
{
 

    public class BlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlogDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public virtual IDbSet<Images> Images { get; set; }

        public virtual IDbSet<Album> Albums { get; set; }

        public virtual IDbSet<Tag> Tags { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public static BlogDbContext Create()
        {
            return new BlogDbContext();
        }
    }
}