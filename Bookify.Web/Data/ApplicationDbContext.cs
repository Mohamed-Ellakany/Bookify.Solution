using Bookify.Web.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<BookCategory> BookCategories { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BookCategory>(b => b.HasKey(bc => new { bc.BookId, bc.CategoryId }));

            builder.HasSequence<int>("SerialNumber", schema: "shared").StartsAt(100001);

            builder.Entity<BookCopy>().Property(e => e.SerialNumber).HasDefaultValueSql("NEXT VALUE FOR shared.SerialNumber");


            base.OnModelCreating(builder);
        }

    }
}
