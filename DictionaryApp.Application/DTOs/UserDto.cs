namespace DictionaryApp.Application.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }       // Identificador único do usuário
        public string Name { get; set; }     // Nome do usuário
        public string Email { get; set; }    // E-mail do usuário
        public string Token { get; set; }    // Token JWT gerado para autenticação
    }
}
