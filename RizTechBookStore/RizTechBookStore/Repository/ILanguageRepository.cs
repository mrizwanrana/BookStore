using RizTechBookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RizTechBookStore.Repository
{
    public interface ILanguageRepository
    {
        Task<List<LanguageModel>> GetLanguages();
    }
}