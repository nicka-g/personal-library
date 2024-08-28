﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core.Models.DTOs
{
    public class GetAuthorDTO
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public List<string> BookList { get; set; } = new List<string>();
    }
}
