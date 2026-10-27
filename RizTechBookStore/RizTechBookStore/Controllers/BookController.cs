using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using RizTechBookStore.Models;
using RizTechBookStore.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RizTechBookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository = null;
        private readonly ILanguageRepository _languageRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        public BookController(IBookRepository bookRepository, ILanguageRepository languageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<ViewResult> GetAllBooks()
        {
            var data = await _bookRepository.GetAllBooks();
            return View(data);
        }

        [Route("book-details")]
        public async Task<ViewResult> GetBook(int id)
        {
            var data = await _bookRepository.GetBookById(id);
            return View(data);
        }
        //public async Task<ViewResult> AddNewBook(bool isSuccess = false, int bookId = 0) 
        //{
        //    ViewBag.Language =new SelectList( await _languageRepository.GetLanguages(),"Id", "Name");
        //    ViewBag.IsSuccess = isSuccess;
        //    ViewBag.BookId = bookId;
        //    return View(); 
        //}
        [Authorize]
        public async Task<ViewResult> AddNewBook(bool isSuccess = false, int bookId = 0)
        {
            //ViewBag.Language = new SelectList(await _languageRepository.GetLanguages(), "Id", "Name");

            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookId;

            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> AddNewBook(BookModel bookModel)
        //{
        //    var errors = ModelState
        //        .Where(x => x.Value.Errors.Any())
        //        .SelectMany(x => x.Value.Errors
        //            .Select(e => $"{x.Key}: {e.ErrorMessage}"))
        //        .ToList();

        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var error in errors)
        //        {
        //            Console.WriteLine(error);
        //        }
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        int id = await _bookRepository.AddNewBook(bookModel);

        //        if (id > 0)
        //        {
        //            return RedirectToAction(
        //                "AddNewBook",
        //                new
        //                {
        //                    isSuccess = true,
        //                    bookId = id
        //                });
        //        }
        //    }

        //    ViewBag.Language = new SelectList(
        //        await _languageRepository.GetLanguages(),
        //        "Id",
        //        "Name",
        //        bookModel.LanguageId);

        //    return View(bookModel);
        //}
        [HttpPost]
        public async Task<IActionResult> AddNewBook(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                if (bookModel.CoverPhoto != null)
                {
                    string folder = "books/cover/";
                    bookModel.CoverImageUrl = await UploadImage(folder, bookModel.CoverPhoto);
                }

                if (bookModel.GalleryFiles != null)

                {
                    string folder = "books/gallery/";
                    bookModel.Gallery = new List<GalleryModel>();
                    foreach (var file in bookModel.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        { 
                            Name = file.Name,
                            URL = await UploadImage(folder, file)
                        };
                        bookModel.Gallery.Add(gallery);
                    }
                }

                if (bookModel.BookPdf != null)
                {
                    string folder = "books/pdf/";
                    bookModel.BookPdfUrl = await UploadImage(folder, bookModel.BookPdf);
                }

                int id = await _bookRepository.AddNewBook(bookModel);
                if (id > 0)
                {
                    return RedirectToAction("AddNewBook", new { isSuccess = true, bookId = id });
                }
            }
            //ViewBag.Language = new SelectList(await _languageRepository.GetLanguages(), "Id", "Name");
            return View(bookModel);
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            
            //folder += bookModel.CoverPhoto.FileName+Guid.NewGuid().ToString();
            folderPath = folderPath + Guid.NewGuid().ToString() + "_" + file.FileName;
            //bookModel.CoverImageUrl = "/" + folder;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }

    }

}
