using BlazorAppCourse.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorAppCourse.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }
        public async Task<List<Category>?> getCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("v1/categories");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
            return JsonSerializer.Deserialize<List<Category>>(content, _serializerOptions);
        }

        public async Task<Category?> getCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"v1/categories/{categoryId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
            return JsonSerializer.Deserialize<Category>(content, _serializerOptions);
        }

        public async Task addCategoriesAsync(Category category)
        {
            var response = await _httpClient.PostAsync("v1/categories", JsonContent.Create(category));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }
        public async Task<Category?> updateCategoriesAsync(Category category)
        {
            if (category == null || category.id <= 0 || await getCategoryAsync(category.id) == null)
            {
                throw new ArgumentException("Category object is invalid. Ensure it has a valid Id.");
            }

            var content = JsonSerializer.Serialize(category, _serializerOptions);
            var response = await _httpClient.PutAsync($"v1/categories/{category.id}", new StringContent(content));

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Update failed: {errorMessage}");
            }

            var updatedCategoryContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(updatedCategoryContent, _serializerOptions);
        }

        public async Task deleteCategoriesAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"v1/categories/{categoryId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }
    }

    public interface ICategoryService
    {
        Task<List<Category>?> getCategoriesAsync();
        Task<Category?> getCategoryAsync(int categoryId);
        Task<Category> updateCategoriesAsync(Category category);
        Task deleteCategoriesAsync(int categoryId);
    }
}
