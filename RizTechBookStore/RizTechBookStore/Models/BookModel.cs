using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using RizTechBookStore.Enums;
using RizTechBookStore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace RizTechBookStore.Models
{
    public class BookModel
    {
        //[DataType(DataType.Date)]
        //[Display(Name = "Choose Date and Time")]
        //public string MyField { get; set; }
        public int Id { get; set; }
        //[MyCustomValidation("Azure")]
        [Required(ErrorMessage = "Please enter the Title")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Please enter the Author Name")]
        public string Author { get; set; }
        [Required(ErrorMessage ="Please enter the description")]
        [StringLength(500)]
        public string Description { get; set; }
        
        public string Category { get; set; }
        
        public string Language { get; set; }
        public int LanguageId { get; set; }
       
        [Required(ErrorMessage ="Please enter the total pages")]
        [Display(Name = "Total Pages")]
        public int? TotalPages { get; set; }
        [Display(Name ="Choose the cover photo of your book")]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }
        [Display(Name = "Choose the gallery images of your book")]
        public IFormFileCollection GalleryFiles { get; set; }
        public List<GalleryModel> Gallery { get; set; }
        [Display(Name = "Choose the pdf file of your book")]
        public IFormFile BookPdf { get; set; }
        public string BookPdfUrl { get; set; }
    }
}
