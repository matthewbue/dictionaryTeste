using DictionaryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WordImportController : ControllerBase
{
    private readonly IWordImportService _wordImportService;

    public WordImportController(IWordImportService wordImportService)
    {
        _wordImportService = wordImportService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportWords()
    {
        try
        {
            var filePath = @"E:\Downloads\words_dictionary.json";  // Atualizado com o caminho correto
            await _wordImportService.ImportWordsFromJson(filePath);
            return Ok(new { message = "Palavras importadas com sucesso." });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
