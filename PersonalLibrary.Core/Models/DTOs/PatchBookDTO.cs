using PersonalLibrary.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core.Models.DTOs
{
    public class PatchBookDTO
    {
        public string? Title { get; set; }
        public BookStatus? Status { get; set; }
        public BookGenre? Genre { get; set; }
        public BookLocation? Location { get; set; }
        public string? Synopsis { get; set; }
        public List<string>? AuthorNames { get; set; }
    }
}
