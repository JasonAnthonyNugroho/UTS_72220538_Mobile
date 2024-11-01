using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using UTS_72220538.Models;
using UTS_72220538.Services; // Ensure you have your models defined here

namespace UTS_72220538.Page
{
    public partial class AddCourse : ContentPage
    {
        private readonly CourseService _courseService;
        private readonly CategoryService _categoryService;
        private List<Category> _categories;

        public AddCourse()
        {
            InitializeComponent();
            _courseService = new CourseService(new HttpClient());
            _categoryService = new CategoryService(new HttpClient()); // Ensure you have this service
            LoadCategories(); // Load categories when the page is initialized
        }

        private async Task LoadCategories()
        {
            try
            {
                _categories = (await _categoryService.GetAllCategoriesAsync()).ToList();


                CourseCategoryPicker.ItemsSource = _categories;
                CourseCategoryPicker.ItemDisplayBinding = new Binding("Name"); // Display the category name
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }

        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            var selectedCategory = CourseCategoryPicker.SelectedItem as Category;

            // Gather the data from the input fields
            var course = new Course
            {
                Name = CourseNameEntry.Text,
                Description = CourseDescriptionEditor.Text,
                ImageName = CourseImageNameEntry.Text,
                Duration = int.TryParse(CourseDurationEntry.Text, out var duration) ? duration : 0,
                Category = selectedCategory // Set the selected category
            };

            try
            {
                // Send the course object to the API to add it
                var result = await _courseService.AddCourseAsync(course);
                if (result)
                {
                    await DisplayAlert("Success", "Course added successfully!", "OK");
                    // Optionally navigate back or clear fields
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add course.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add course: {ex.Message}", "OK");
            }
        }
    }
}
