using PersonalLibrary.Models.Enums;

namespace PersonalLibrary.Models.DTOs
{
    public class AddBookDTO
    {
        public string Title { get; set; } = string.Empty;
        public BookStatus Status { get; set; }
        public BookGenre Genre { get; set; }
        public BookLocation Location { get; set; }
        public string? Synopsis { get; set; }

        public List<string> AuthorNames{ get; set; } = new List<string>();
    }
}
