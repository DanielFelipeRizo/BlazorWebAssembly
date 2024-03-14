namespace BlazorAppCourse.Models
{
    public class User
    {
        public int id { get; set; } = 0;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string avatar { get; set; } = string.Empty;
    }
}
