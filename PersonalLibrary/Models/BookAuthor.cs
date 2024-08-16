namespace PersonalLibrary.Models
{
    public class BookAuthor
    {
        public int BookAuthorId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = new Book();
        public int AuthorId { get; set; }
        public Author Author { get; set; } = new Author();
    }
}
