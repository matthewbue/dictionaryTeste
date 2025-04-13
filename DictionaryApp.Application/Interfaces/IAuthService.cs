using DictionaryApp.Application.Dtos;

namespace DictionaryApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task SignUpAsync(SignUpDto signUpDto);
        Task<string> SignInAsync(SignInDto signInDto);
    }
}
