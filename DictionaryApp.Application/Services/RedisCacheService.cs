using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConfiguration configuration)
    {
        var connection = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]);
        _database = connection.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, json, expiry);
    }
}
