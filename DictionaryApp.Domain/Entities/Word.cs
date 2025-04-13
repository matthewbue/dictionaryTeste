public class Word
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Gera um novo GUID por padrão
    public string WordName { get; set; }
    public string Definition { get; set; }
    public DateTime LastAccessed { get; set; }
    public bool IsFavorite { get; set; }
}
