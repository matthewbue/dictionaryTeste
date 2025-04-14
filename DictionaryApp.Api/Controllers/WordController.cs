using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // Adicionando proteção com token JWT
public class WordController : ControllerBase
{
    private readonly IWordService _wordService;
    private readonly IHistoryService _historyService;

    public WordController(IWordService wordService, IHistoryService historyService)
    {
        _wordService = wordService;
        _historyService = historyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWords([FromQuery] int limit = 10, [FromQuery] int page = 1)
    {
        var wordList = await _wordService.GetWordsAsync(limit, page);
        return Ok(wordList);
    }

    [HttpGet("{word}")]
    public async Task<IActionResult> GetWord(string word)
    {
        // Obter o usuário do token
        var userId = User.FindFirst("id")?.Value;
        if (userId == null)
        {
            return Unauthorized(new { message = "Token inválido ou ausente" });
        }

        // Buscar a palavra e adicionar ao histórico
        var wordDetails = await _wordService.GetWordDetailsAsync(word);
        if (wordDetails == null)
        {
            return NotFound(new { message = "Palavra não encontrada" });
        }

        // Adicionar ao histórico
        await _historyService.AddToHistoryAsync(userId, word);

        return Ok(wordDetails);
    }

     [HttpPost("{wordId}/favorite")]
    public async Task<IActionResult> AddWordToFavorites(string wordId)
    {
        await _wordService.AddToFavoritesAsync(wordId);
        return Ok(new { message = "Palavra adicionada aos favoritos." });
    }

    [HttpDelete("{wordId}/unfavorite")]
    public async Task<IActionResult> RemoveWordFromFavorites(string wordId)
    {
        await _wordService.RemoveFromFavoritesAsync(wordId);
        return Ok(new { message = "Palavra removida dos favoritos." });
    }
}
