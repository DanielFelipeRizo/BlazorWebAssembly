using BlazorAppCourse.Models;
using System.Net.Http.Json;
using System.Text.Json;


namespace BlazorAppCourse.Services
{
    public class UserService: IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<List<User>?> getUsersAsync()
        {
            var response = await _httpClient.GetAsync("v1/users");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);

            return JsonSerializer.Deserialize<List<User>?>(content, _serializerOptions); ;
        }

        public async Task<User?> getUserAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"v1/users/{userId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new HttpRequestException(content);
            return JsonSerializer.Deserialize<User>(content, _serializerOptions);
        }

        public async Task addUserAsync(User? user)
        {
            var response = await _httpClient.PostAsync("v1/users", JsonContent.Create(user));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }
        public async Task updateUserAsync(User user)
        {
            if (user == null || user.id <= 0 || await getUserAsync(user.id) == null)
            {
                throw new ArgumentException("User object is invalid. Ensure it has a valid Id.");
            }
            var response = await _httpClient.PutAsync($"v1/users/{user.id}", JsonContent.Create(user));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }

        public async Task deleteUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"v1/users/{userId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }
    }

    public interface IUserService
    {
        Task<List<User>?> getUsersAsync();
        Task<User?> getUserAsync(int userId);
        Task addUserAsync(User? user);
        Task updateUserAsync(User? user);
        Task deleteUserAsync(int userId);
    }
}
