using DictionaryApp.Application.Dtos;

namespace DictionaryApp.Application.Interfaces
{
    public interface IWordService
    {
        Task<WordDto> GetWordDetailsAsync(string word);
        Task<IEnumerable<WordDto>> SearchWordsAsync(string search, int limit, int page);
        Task AddWordToFavoritesAsync(string wordId);
        Task RemoveWordFromFavoritesAsync(string wordId);
        Task<IEnumerable<FavoriteDto>> GetFavoriteWordsAsync();
        Task<WordListDto> GetWordsAsync(string search, int page, int limit);
        Task AddToFavoritesAsync(string wordId);  // Adicione esta linha
        Task RemoveFromFavoritesAsync(string wordId);  // Adicione esta linha
    }
}
