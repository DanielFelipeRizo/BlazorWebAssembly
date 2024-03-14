namespace BlazorAppCourse.Models
{
    public class Product
    {
        public int id { get; set; } = 0;
        public string title { get; set; } = string.Empty;
        public decimal price { get; set; } = 0;
        public string description { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public string[] images { get; set; }
        public int categoryId { get; set; } = 0;
        public Category category { get; set; } = new Category();
    }
}
