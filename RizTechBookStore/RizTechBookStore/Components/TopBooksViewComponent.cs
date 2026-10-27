using Microsoft.AspNetCore.Mvc;
using RizTechBookStore.Repository;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RizTechBookStore.Components
{
    public class TopBooksViewComponent : ViewComponent
    {
        private readonly IBookRepository _bookRepository;
        public TopBooksViewComponent(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var topBooks = await _bookRepository.GetTopBooks(count); // Get top 'count' books
            return View(topBooks);
        }
    }
}
