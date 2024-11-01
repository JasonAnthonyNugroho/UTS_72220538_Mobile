using UTS_72220538.Models; // Pastikan Anda memiliki model Category
using UTS_72220538.Services; // Pastikan Anda memiliki CategoryService
using System;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace UTS_72220538.Page
{
    public partial class CategoryPage : ContentPage
    {
        private readonly CategoryService _categoryService;
        private ObservableCollection<CategoryViewModel> _categories;
        public CategoryPage(CategoryService categoryService)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _categories = new ObservableCollection<CategoryViewModel>();
            CategoriesListView.ItemsSource = _categories;
            LoadCategories(); // Load categories when page is initialized
        }

        private async void OnAddCategory(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory(new CategoryService(new HttpClient())));
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCategories();
        }
        private async Task LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                CategoriesListView.ItemsSource = categories.ToList(); // Pastikan ini tidak null
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }

        private void OnCategorySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Category selectedCategory)
            {
                DisplayAlert("Selected Category", selectedCategory.Name, "OK");
                CategoriesListView.SelectedItem = null; // Menghapus pilihan
            }
        }
        private async void OnRefreshList(object sender, EventArgs e)
        {
            LoadCategories();
        }
        private async void OnDeleteCategory(object sender, EventArgs e)
        {
            var selectedCategories = _categories.Where(c => c.IsSelected).ToList();

            if (selectedCategories.Count > 0)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete the selected categories?",
                    "Yes",
                    "No");
                if (confirm)
                {
                    try
                    {
                        foreach (var categoryViewModel in selectedCategories)
                        {
                            await _categoryService.DeleteCategoryAsync(categoryViewModel.Category.CategoryId);
                            _categories.Remove(categoryViewModel);
                        }
                        await DisplayAlert("Success", "Selected categories deleted successfully!", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete categories: {ex.Message}", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Warning", "Please select at least one category to delete.", "OK");
            }
        }

        private async void OnUpdateCategory(object sender, EventArgs e)
        {
            if (CategoriesListView.SelectedItem is Category selectedCategory)
            {
                // Navigate to EditCategory page, passing the selected category
                //await Navigation.PushAsync(new EditCategory(_categoryService, selectedCategory));
            }
            else
            {
                await DisplayAlert("Error", "Please select a category to update.", "OK");
            }
        }
        
    }
}