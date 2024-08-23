using PersonalLibrary.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core.Models.DTOs
{
    public class GetBookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public BookStatus Status { get; set; }
        public BookGenre Genre { get; set; }
        public BookLocation Location { get; set; }
        public string? Synopsis { get; set; }
        public List<string> AuthorNames { get; set; } = new List<string>();
    }
}
