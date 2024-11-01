using UTS_72220538.Models; // Pastikan Anda memiliki model Category
using UTS_72220538.Services; // Pastikan Anda memiliki CategoryService
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace UTS_72220538.Page
{
    public partial class CategoryPage : ContentPage
    {
        private readonly CategoryService _categoryService;
        private ObservableCollection<CategoryWithSelection> _categories;

        public CategoryPage(CategoryService categoryService)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _categories = new ObservableCollection<CategoryWithSelection>();
            CategoriesListView.ItemsSource = _categories;

            LoadCategories(); // Load categories when page is initialized
        }
        private async void LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync(); // Assuming this method exists
                _categories.Clear(); // Clear existing categories
                foreach (var category in categories)
                {
                    // Wrap category in your CategoryWithSelection model
                    _categories.Add(new CategoryWithSelection { CategoryId = category.CategoryId, Name = category.Name, Description = category.Description });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }
        private async void OnUpdateSelectedCategory(object sender, EventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as CategoryWithSelection;

            if (selectedCategory != null)
            {
                // Navigate to EditCategory page, passing the selected category and categoryService
                await Navigation.PushAsync(new EditCategory(selectedCategory, _categoryService));
            }
            else
            {
                await DisplayAlert("Error", "Please select a category to update.", "OK");
            }
        }

        // Other methods...
    }
}
