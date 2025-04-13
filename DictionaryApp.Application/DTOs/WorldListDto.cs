namespace DictionaryApp.Application.Dtos
{
    public class WordListDto
    {
        public List<Word> Results { get; set; }
        public int TotalDocs { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
    }
}
