namespace DictionaryApp.Domain.Entities
{
    public class Favorite
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string WordId { get; set; }
        public DateTime Added { get; set; }
    }
}
