using PersonalLibrary.Models.Enums;

namespace PersonalLibrary.Models
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
