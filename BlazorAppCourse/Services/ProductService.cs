using BlazorAppCourse.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorAppCourse.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<List<Product>?> getProductsAsync()
        {
            var response = await _httpClient.GetAsync("v1/products");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);

            List<Product>? currentListProducts= JsonSerializer.Deserialize<List<Product>?>(content, _serializerOptions);
            foreach(Product p in currentListProducts)
            {
                p.images[0] = p.images[0].Trim('[', ']').Trim('"', '"');
            }
            return currentListProducts;
        }

        public async Task<Product?> getProductAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"v1/products/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new HttpRequestException(content);
            Product? currentProduct = JsonSerializer.Deserialize<Product>(content, _serializerOptions);
            currentProduct.image = currentProduct.images[0].Trim('[', ']').Trim('"', '"');
            currentProduct.categoryId = currentProduct.category.id;
            return currentProduct;
        }

        public async Task addProductsAsync(Product product)
        {
            var response = await _httpClient.PostAsync("v1/products", JsonContent.Create(product));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }
        public async Task updateProductsAsync(Product product)
        {
            if (product == null || product.id <= 0 || await getProductAsync(product.id) == null)
            {
                throw new ArgumentException("Product object is invalid. Ensure it has a valid Id.");
            }
            var response = await _httpClient.PutAsync($"v1/products/{product.id}", JsonContent.Create(product));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }

        public async Task deleteProductsAsync(int productId)
        {
            var response = await _httpClient.DeleteAsync($"v1/products/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }
    }

    public interface IProductService
    {
        Task<List<Product>?> getProductsAsync();
        Task<Product?> getProductAsync(int productId);
        Task addProductsAsync(Product product);
        Task updateProductsAsync(Product product);
        Task deleteProductsAsync(int productId);
    }
}
