using DictionaryApp.Application.Interfaces;
using DictionaryApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DictionaryApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 
        private readonly JwtTokenService _jwtTokenService;
        private readonly IHistoryService _historyService;
        private readonly IFavoriteService _favoriteService;


        public UserController(IUserService userService, JwtTokenService jwtTokenService, IHistoryService historyService, IFavoriteService favoriteService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _historyService = historyService;
            _favoriteService = favoriteService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = _jwtTokenService.GetUserIdFromToken();
            var userProfile = await _userService.GetCurrentUserAsync(userId);
            if (userProfile == null)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(userProfile);
        }

        [HttpGet("user/me/history")]
        public async Task<IActionResult> GetUserHistory()
        {
            var userId = _jwtTokenService.GetUserIdFromToken();
            var history = await _historyService.GetHistoryAsync(userId);
            return Ok(new { results = history });
        }

        [HttpGet("user/me/favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = _jwtTokenService.GetUserIdFromToken();
            var favorites = await _favoriteService.GetFavoriteWordsAsync(userId);
            return Ok(new { results = favorites });
        }


        // Outros endpoints do usuário podem ser adicionados aqui
    }
}
