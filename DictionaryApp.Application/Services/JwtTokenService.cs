using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace DictionaryApp.Application.Services
{
    public class JwtTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserIdFromToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token não encontrado.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var userId = jsonToken?.Claims.First(claim => claim.Type == "id").Value;

            return userId;
        }
    }
}
