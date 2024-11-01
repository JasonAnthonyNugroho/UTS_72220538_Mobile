using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UTS_72220538.Models;
using System.Net.Http.Json;
namespace UTS_72220538.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private const string CategoriesEndpoint = "api/v1/Categories";
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actualbackendapp.azurewebsites.net/");
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("https://actualbackendapp.azurewebsites.net/api/v1/Categories");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Category>>(content);
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync(CategoriesEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category), System.Text.Encoding.UTF8, "application/json");

            // Gantilah URL di sini jika perlu
            var response = await _httpClient.PostAsync("https://actualbackendapp.azurewebsites.net/api/v1/Categories", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/categories/{category.CategoryId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"api/categories/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error deleting category: {response.StatusCode} - {errorMsg}");
            }
        }


    }
}
