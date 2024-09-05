using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core.Contracts.Repositories
{
    public interface IAuthorRepository
    {
        public Task<IEnumerable<GetAuthorDTO>> GetAuthors();
        public Task<GetAuthorDTO> GetAuthorById(int id);
        public Task UpdateAuthor(Book book, List<string> newAuthors);
    }
}
