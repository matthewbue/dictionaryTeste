DictionaryApp
 Funcionalidades Implementadas

1. Serviços de Palavras (WordService)
    - GetWordDetailsAsync: Busca detalhes de uma palavra, primeiro no cache (Redis) e depois no banco de dados.
    - SearchWordsAsync: Pesquisa palavras com base em um filtro e retorna os resultados paginados.
    - AddWordToFavoritesAsync: Adiciona uma palavra aos favoritos do usuário.
    - RemoveWordFromFavoritesAsync: Remove uma palavra dos favoritos do usuário.
    - GetFavoriteWordsAsync: Retorna as palavras favoritas do usuário.

2. Serviços de Favoritos (FavoriteService)
    - GetFavoriteWordsAsync: Retorna as palavras favoritas de um usuário.

3. Serviços de Histórico (HistoryService)
    - AddToHistoryAsync: Adiciona uma palavra ao histórico de um usuário.
    - ClearHistoryAsync: Limpa o histórico de um usuário.
    - GetHistoryAsync: Retorna o histórico de um usuário.

4. Serviços de Autenticação (AuthService)
    - SignInAsync: Realiza o login de um usuário, gerando um token JWT se as credenciais forem válidas.
    - SignUpAsync: Realiza o cadastro de um novo usuário, verificando se o email já está em uso.

5. Banco de Dados e Repositórios
    - A aplicação usa um repositório genérico para acessar os dados de palavras, favoritos, histórico e usuários, permitindo fácil extensibilidade.

 Rodando o Projeto

### Pré-requisitos
- .NET 6.0 ou superior
- Docker (para rodar o Redis)

### Passos
1. Clonar o repositório:
    ```bash
    git clone https://github.com/seuusuario/DictionaryApp.git
    cd DictionaryApp
    ```
2. Rodar o Redis com Docker (opcional, caso queira usar Redis para cache):
    ```bash
    docker run -d --name redis-cache -p 6379:6379 redis
    ```
3. Restaurar as dependências do projeto:
    ```bash
    dotnet restore
    ```
4. Rodar a aplicação:
    ```bash
    dotnet run --project DictionaryApp.Api
    ```
    A aplicação estará disponível em `http://localhost:5000`.

離 Testes

Os testes são implementados utilizando xUnit e Moq para simular as interações com as dependências. Para rodar os testes:

1. Rodar os testes unitários:
    ```bash
    dotnet test
    ```
    Ou, se estiver usando o Visual Studio, basta clicar com o botão direito sobre os testes e escolher "Executar Testes".

 Estrutura do Projeto

A aplicação segue a arquitetura limpa (Clean Architecture) e está organizada da seguinte forma:
```
DictionaryApp
├── DictionaryApp.Api         # API (Controllers)
├── DictionaryApp.Application # Camada de serviços e lógica de aplicação
├── DictionaryApp.Common      # Utilitários e helpers
├── DictionaryApp.Domain      # Entidades e lógica de domínio
├── DictionaryApp.Infra       # Acesso ao banco de dados e repositórios
└── DictionaryApp.Tests       # Testes unitários
```

⚙ Dependências

- StackExchange.Redis: Para cache de palavras com Redis.
- Moq: Para mockar dependências durante os testes.
- xUnit: Framework de testes unitários.

 Próximos Passos

1. Refinar e expandir os testes para cobrir mais cenários, como falhas de rede e erros no banco de dados.
2. Adicionar mais funcionalidades, como a possibilidade de editar palavras e atualizar as definições.
3. Implementar autenticação JWT para o uso de APIs protegidas.

 Contribuições

Se você gostaria de contribuir para o projeto, sinta-se à vontade para abrir uma pull request. As contribuições são sempre bem-vindas! Para contribuir:

1. Faça um fork do repositório
2. Crie uma branch para sua feature (`git checkout -b minha-feature`)
3. Comite suas mudanças (`git commit -am 'Adiciona nova funcionalidade'`)
4. Dê push para a branch (`git push origin minha-feature`)
5. Abra uma pull request

 Licença

Este projeto é licenciado sob a licença MIT - veja o arquivo LICENSE para mais detalhes.

