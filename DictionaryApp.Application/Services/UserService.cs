using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Domain.Entities;
using System.Threading.Tasks;

namespace DictionaryApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetCurrentUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
