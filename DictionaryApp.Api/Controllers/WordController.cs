using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WordController : ControllerBase
{
    private readonly IWordService _wordService;
    private readonly IHistoryService _historyService;
    private readonly JwtTokenService _jwtTokenService;

    public WordController(IWordService wordService, IHistoryService historyService, JwtTokenService jwtTokenService)
    {
        _wordService = wordService;
        _historyService = historyService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Fullstack Challenge 🏅 - Dictionary" });
    }

    [HttpGet("entries/en")]
    public async Task<IActionResult> GetWords([FromQuery] int limit = 10, [FromQuery] int page = 1, [FromQuery] string search = "")
    {
        var wordList = await _wordService.GetWordsAsync(search, limit, page);
        return Ok(wordList);
    }

    [HttpGet("entries/en/{word}")]
    public async Task<IActionResult> GetWordDetails(string word)
    {
        var userId = _jwtTokenService.GetUserIdFromToken();
        var wordDetails = await _wordService.GetWordDetailsAsync(word);
        if (wordDetails == null)
            return NotFound(new { message = "Palavra não encontrada" });

        await _historyService.AddToHistoryAsync(userId, word);  // Adiciona a palavra no histórico

        return Ok(wordDetails);
    }


    [HttpPost("entries/en/{word}/favorite")]
    public async Task<IActionResult> AddToFavorites(string word)
    {
        var userId = _jwtTokenService.GetUserIdFromToken();
        await _wordService.AddWordToFavoritesAsync(word);
        return Ok(new { message = "Palavra adicionada aos favoritos." });
    }


    [HttpDelete("entries/en/{word}/unfavorite")]
    public async Task<IActionResult> RemoveFromFavorites(string word)
    {
        var userId = _jwtTokenService.GetUserIdFromToken();
        await _wordService.RemoveWordFromFavoritesAsync(word);
        return Ok(new { message = "Palavra removida dos favoritos." });
    }

}
