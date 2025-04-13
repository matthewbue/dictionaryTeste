using DictionaryApp.Domain.Entities;
using DictionaryApp.Application.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DictionaryApp.Domain.Interfaces;

public class WordImportService : IWordImportService
{
    private readonly IWordRepository _wordRepository;

    public WordImportService(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task ImportWordsFromJson(string filePath)
    {
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var wordsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

        var words = new List<Word>();

        foreach (var word in wordsDictionary)
        {
            words.Add(new Word
            {
                WordName = word.Key,  // Palavra
                Definition = word.Value, // Definição
                LastAccessed = DateTime.UtcNow,  // Definir a data atual como a última acessada
                IsFavorite = false  // Inicialmente como não favorita
            });
        }

        await _wordRepository.AddWordsAsync(words);
    }
}
