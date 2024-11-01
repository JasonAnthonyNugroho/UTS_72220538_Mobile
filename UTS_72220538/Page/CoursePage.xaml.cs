using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using UTS_72220538.Models;
using UTS_72220538.Services;

namespace UTS_72220538.Page
{
    public partial class CoursePage : ContentPage
    {
        private readonly CourseService _courseService;
        private ObservableCollection<CourseWithSelected> _courses;

        public CoursePage(CourseService courseService)
        {
            InitializeComponent();
            _courseService = courseService;
            _courses = new ObservableCollection<CourseWithSelected>();
            CoursesListView.ItemsSource = _courses;

            LoadCourses(); // Load courses when page is initialized
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCourses();
        }

        private async Task LoadCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                _courses.Clear();
                foreach (var course in courses)
                {
                    // Create a CourseWithSelected instance
                    var courseWithSelection = new CourseWithSelected
                    {
                        CourseId = course.CourseId,
                        Name = course.Name,
                        ImageName = course.ImageName,
                        Duration = course.Duration,
                        Description = course.Description,
                        Category = course.Category, // Assigning the associated Category
                        IsSelected = false // Initialize IsSelected to false
                    };
                    _courses.Add(courseWithSelection);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
            }
        }


        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCourse());
        }

        private async void OnRefreshList(object sender, EventArgs e)
        {
            await LoadCourses();
        }

        private void OnCourseSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCourse = e.SelectedItem as CourseWithSelected;

            if (selectedCourse != null)
            {
                // You can enable the Update and Delete buttons here
                UpdateSelectedCourseButton.IsEnabled = true; // Example of enabling update button
            }
            else
            {
                // Handle deselection if necessary
                UpdateSelectedCourseButton.IsEnabled = false; // Disable if no selection
            }
        }

        private async void OnDeleteSelectedCourses(object sender, EventArgs e)
        {
            var selectedCourses = _courses.Where(c => c.IsSelected).ToList();

            if (selectedCourses.Count > 0)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    $"Are you sure you want to delete the selected courses?",
                    "Yes",
                    "No");
                if (confirm)
                {
                    try
                    {
                        foreach (var course in selectedCourses)
                        {
                            await _courseService.DeleteCourseAsync(course.CourseId);
                            _courses.Remove(course);
                        }
                        await DisplayAlert("Success", "Selected courses deleted successfully!", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete courses: {ex.Message}", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Warning", "Please select at least one course to delete.", "OK");
            }
        }

        private async void OnUpdateSelectedCourses(object sender, EventArgs e)
        {
            var selectedCourses = _courses.Where(c => c.IsSelected).ToList();

            if (selectedCourses.Count > 0)
            {
                foreach (var course in selectedCourses)
                {
                    // Navigate to EditCourse page for each selected course
                    await Navigation.PushAsync(new EditCourse(course));
                }
            }
            else
            {
                await DisplayAlert("Error", "Please select at least one course to update.", "OK");
            }
        }
    }
}
