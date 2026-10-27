using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RizTechBookStore.Data;
using RizTechBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RizTechBookStore.Repository
{
    public class BookRepository : IBookRepository
    {

        private readonly BookStoreContext _bookStoreContext = null;
        private readonly IConfiguration _configuration = null;


        public BookRepository(BookStoreContext bookStoreContext, IConfiguration configuration)
        {
            _bookStoreContext = bookStoreContext;
            _configuration = configuration;
        }
        public async Task<int> AddNewBook(BookModel bookModel)
        {
            var book = new Books()
            {
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                Category = bookModel.Category,
                LanguageId = bookModel.LanguageId,
                TotalPages = bookModel.TotalPages.HasValue ? bookModel.TotalPages.Value : 0,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CoverImageUrl = bookModel.CoverImageUrl,
                BookPdfUrl = bookModel.BookPdfUrl
            };
            //var gallery = new List<BookGallery>();
            if (bookModel.Gallery != null)
            {
                foreach (var file in bookModel.Gallery)
                {
                    book.BookGallery.Add(new BookGallery()
                    {
                        Name = file.Name,
                        URL = file.URL
                    });
                }
            }
            await _bookStoreContext.Books.AddAsync(book);
            await _bookStoreContext.SaveChangesAsync();
            // Sample data for demonstration purposes
            return book.Id; // Return the ID of the newly added book
        }
        public async Task<List<BookModel>> GetAllBooks()
        {
            var books = new List<BookModel>();
            var allBooks = await _bookStoreContext.Books.ToListAsync();
            if (allBooks.Any() == true)
            {
                foreach (var book in allBooks)
                {
                    books.Add(new BookModel
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        Description = book.Description,
                        Category = book.Category,
                        Language = book.Language?.Name, // Assuming Language is a string representation of LanguageId
                        LanguageId = book.LanguageId,
                        TotalPages = book.TotalPages,
                        CoverImageUrl = book.CoverImageUrl
                    });
                }
            }
            return books; // Return the list of all books
        }

        public async Task<List<BookModel>> GetTopBooks(int count)
        {
            return await _bookStoreContext.Books
                //.OrderByDescending(b => b.CreatedOn)
                .Select(b => new BookModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    Category = b.Category,
                    LanguageId = b.LanguageId,
                    Language = b.Language.Name,
                    TotalPages = b.TotalPages,
                    CoverImageUrl = b.CoverImageUrl
                }).Take(count).ToListAsync();
        }
        public async Task<BookModel> GetBookById(int id)
        {
            var book = await _bookStoreContext.Books.Where(x => x.Id == id).Select(x => new BookModel
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                Description = x.Description,
                Category = x.Category,
                LanguageId = x.LanguageId,
                Language = x.Language.Name,
                TotalPages = x.TotalPages,
                CoverImageUrl = x.CoverImageUrl,
                Gallery = x.BookGallery.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList(),
                BookPdfUrl = x.BookPdfUrl
            }).FirstOrDefaultAsync();
            return book;
        }

        public List<BookModel> SearchBooks(string bookName, string authorName)
        {
            return null;
        }

        public string GetAppName()
        {
            return "RizTech Book Store";
        }
    }
}
