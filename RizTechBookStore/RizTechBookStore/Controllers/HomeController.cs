using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RizTechBookStore.Models;
using RizTechBookStore.Repository;
using RizTechBookStore.Service;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace RizTechBookStore.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IConfiguration _configuration;
        private readonly NewBookAlertConfig _newBookAlertConfig;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public HomeController(IOptionsSnapshot<NewBookAlertConfig> newBookAlertConfig, IMessageRepository messageRepository, 
            IUserService userService, IEmailService emailService)
        {
            //_newBookAlertConfig = newBookAlertConfig.Value;
            //_messageRepository = messageRepository;
            _userService = userService;
            _emailService = emailService;
        }
        #region
        //[ViewData]
        //public string CustomProperty { get; set; }
        //[ViewData]
        //public string Title { get; set; }
        //[ViewData]
        //public BookModel Book { get; set; }
        //public ViewResult Index()
        //{
        //    //Full path of the view file is: TempView/RizwanTemp.cshtml
        //    //return View("TempView/RizwanTemp.cshtml");
        //    //Relative path of the view file is: /Views/Home/TempView/RizwanTemp
        //    //return View("../../TempView/RizwanTemp");
        //    return View();
        //}
        //private readonly BookRepository _bookRepository = null;
        //public HomeController()
        //{
        //    _bookRepository = new BookRepository();
        //}
        #endregion
        public async Task<ViewResult> Index()
        {
            //UserEmailOptions userEmailOptions = new UserEmailOptions
            //{ 
            //    ToEmails = new List<string>() { "m.rizwan.rana@gmail.com"},
            //    PlaceHolders = new List<KeyValuePair<string, string>>()
            //    {
            //        new KeyValuePair<string, string>("{{UserName}}", "Rizwan")
            //    }
            //};
            //await _emailService.SendTestEmail(userEmailOptions);
            //var userId = _userService.GetUserId();
            //var isLoggedin = _userService.UserLoginOrNot();
            //var isDisplay = _newBookAlertConfig.DisplayNewBookAlert;
            //var value = _messageRepository.GetName();

            #region Start ViewBag
            //ViewBag.Title = "Technologies";
            //dynamic data = new ExpandoObject();
            //data.Id = 1;
            //data.Name = "John Doe";
            //data.Description = "Software Engineer";
            //ViewBag.Data = data;

            //ViewBag.Type = new BookModel()
            //{
            //    Id = 1,
            //    Title = "C# Programming",
            //    Author = "John Smith",
            //    Description = "Learn C# programming from scratch.",
            //    Category = "Programming",
            //    Language = "English",
            //    TotalPages = 500
            //};
            #endregion

            #region ViewData
            //ViewData["Title"] = "Technologies";
            //ViewData["book"] = new BookModel() 
            //{ 
            //    Id = 1,
            //    Author = "John Smith", 
            //    Title = "C# Programming", 
            //    Description = "Learn C# programming from scratch.", 
            //    Category = "Programming", 
            //    Language = "English",
            //    TotalPages = 500 
            //};
            #endregion

            #region ViewData Attribute
            //Title = "Home Page";
            //CustomProperty = "Custom Value";
            //Book = new BookModel()
            //{
            //    Id = 1,
            //    Author = "Muhammad Rizwan",
            //    Title = "C# Programming",
            //    Description = "Learn C# programming from scratch.",
            //    Category = "Programming",
            //    Language = "English",
            //    TotalPages = 500
            //};
            #endregion

            return View();
        }
        public ViewResult AboutUs()
        {
            //Title = "About Us Page";
            return View();
        }

        public ViewResult ContactUs()
        {
            //Title = "Contact Us Page";
            return View();
        }
        public ViewResult Positions()
        {
            //Title = "Positions Page";
            return View();
        }
    }
}
