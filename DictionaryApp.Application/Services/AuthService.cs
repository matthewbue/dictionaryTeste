using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> SignInAsync(SignInDto signInDto)
    {
        var user = await _userRepository.GetByEmailAsync(signInDto.Email);
        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, signInDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("k3vPIQPPm3fVeawKBwZUl9ILPCgKOmqs6Hka9PdDqNc");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
               {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task SignUpAsync(SignUpDto signUpDto)
    {
        // Verificar se o email já está registrado
        var existingUser = await _userRepository.GetByEmailAsync(signUpDto.Email);
        if (existingUser != null)
        {
            throw new Exception("Usuário já existe com esse email.");
        }

        // Criação do usuário
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = signUpDto.Name,
            Email = signUpDto.Email,
        };

        // Criptografar a senha antes de salvar no banco de dados
        user.Password = _passwordHasher.HashPassword(user, signUpDto.Password);

        // Adicionar o usuário ao repositório
        await _userRepository.AddAsync(user);
    }
}
