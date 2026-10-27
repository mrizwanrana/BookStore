namespace RizTechBookStore.Data
{
    public class BookGallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BooksId { get; set; }
        public string URL { get; set; }
        public Books Book { get; set; }
    }
}
