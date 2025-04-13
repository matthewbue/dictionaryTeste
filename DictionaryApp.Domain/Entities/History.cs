namespace DictionaryApp.Domain.Entities
{
    public class History
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Word { get; set; }
        public DateTime Added { get; set; }
    }
}
