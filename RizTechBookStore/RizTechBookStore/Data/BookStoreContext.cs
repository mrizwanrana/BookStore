using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RizTechBookStore.Models;

namespace RizTechBookStore.Data
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>     //DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookGallery> BookGallery { get; set; }
    }
}
