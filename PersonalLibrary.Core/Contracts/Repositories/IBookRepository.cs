using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Core.Contracts.Repositories
{
    public interface IBookRepository
    {
        public Task<IEnumerable<GetBookDTO>> GetBooks();
        public Task<GetBookDTO>GetBookById(int bookId);
        public Task<AddBookDTO> AddBook(AddBookDTO addBookDTO);
        public Task<UpdateBookDTO> UpdateBook(UpdateBookDTO updateBook, int bookId);
        public Task<bool> DeleteBook(int bookId);
    }
}
