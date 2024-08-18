using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Infrastructure
{
    public class LibraryDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; } 
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public LibraryDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
