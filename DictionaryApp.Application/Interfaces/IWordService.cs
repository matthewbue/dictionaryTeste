using DictionaryApp.Application.Dtos;

namespace DictionaryApp.Application.Interfaces
{
        public interface IWordService
        {
                Task<WordDto> GetWordDetailsAsync(string word);
                Task AddWordToFavoritesAsync(string wordId);
                Task RemoveWordFromFavoritesAsync(string wordName);
                Task<IEnumerable<FavoriteDto>> GetFavoriteWordsAsync();
                Task<WordListDto> GetWordsAsync(string search, int page, int limit);
                Task AddToFavoritesAsync(string wordId);  // Adicione esta linha
        }
}
