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
    public class CourseService
    {
        private readonly HttpClient _httpClient;
        private const string CoursesEndpoint = "api/Courses";

        public CourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actualbackendapp.azurewebsites.net/");
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var response = await _httpClient.GetAsync(CoursesEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Course>>();
        }
            public async Task<Course> GetCourseByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/courses/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Course>(content);
        }

        public async Task CreateCourseAsync(Course course)
        {
            var content = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/courses", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var content = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/courses/{course.CourseId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/courses/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<bool> AddCourseAsync(Course course)
        {
            var response = await _httpClient.PostAsJsonAsync("api/courses", course);
            return response.IsSuccessStatusCode;
        }
    }
}
