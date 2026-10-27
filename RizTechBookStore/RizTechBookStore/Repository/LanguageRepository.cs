using Microsoft.EntityFrameworkCore;
using RizTechBookStore.Data;
using RizTechBookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RizTechBookStore.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly BookStoreContext _bookStoreContext = null;
        public LanguageRepository(BookStoreContext bookStoreContext)
        {
            _bookStoreContext = bookStoreContext;
        }

        public async Task<List<LanguageModel>> GetLanguages()
        {
            var languages = await _bookStoreContext.Languages.ToListAsync();
            return languages.Select(l => new LanguageModel
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description
            }).ToList();
        }
    }
}
