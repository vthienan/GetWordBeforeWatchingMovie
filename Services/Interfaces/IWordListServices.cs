using System.Collections.Generic;
using System.Threading.Tasks;
using GetWordBeforeWatchingMovie.Core;

namespace GetWordBeforeWatchingMovie.Services.Interfaces
{
    public interface IWordListServices
    {
        Task<List<string>> GetWordLists(string imdbId, Level userEnglishLevel);
    }
}