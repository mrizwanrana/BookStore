using RizTechBookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RizTechBookStore.Repository
{
    public interface IBookRepository
    {
        Task<int> AddNewBook(BookModel bookModel);
        Task<List<BookModel>> GetAllBooks();
        Task<BookModel> GetBookById(int id);
        Task<List<BookModel>> GetTopBooks(int count);
        List<BookModel> SearchBooks(string bookName, string authorName);
        string GetAppName();
    }
}