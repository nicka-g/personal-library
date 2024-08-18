using PersonalLibrary.Core.Models.Enums;
using PersonalLibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public BookStatus Status { get; set; }
        public BookGenre Genre { get; set; }
        public BookLocation Location { get; set; }
        public string? Synopsis { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        //public ICollection<Author> Authors { get; set; } = new List<Author>();
    }
}
