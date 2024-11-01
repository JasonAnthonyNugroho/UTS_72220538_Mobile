using UTS_72220538.Models;
using UTS_72220538.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace UTS_72220538.Page
{
    public partial class CoursePage : ContentPage
    {
        private readonly CourseService _courseService;
        private ObservableCollection<Course> _courses;

        public CoursePage(CourseService courseService)
        {
            InitializeComponent();
            _courseService = courseService;
            _courses = new ObservableCollection<Course>();
            CoursesListView.ItemsSource = _courses;

            LoadCourses(); // Load courses when page is initialized
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCourses();
        }

        private async void OnAddCourse(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCourse());
        }
        private async Task LoadCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                _courses.Clear();
                foreach (var course in courses)
                {
                    course.IsSelected = false; // Initialize IsSelected to false
                    _courses.Add(course);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
            }
        }

        private void OnCheckboxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Find the checkbox that triggered the event
            var checkbox = sender as CheckBox;

            if (checkbox != null)
            {
                // Get the corresponding course
                var course = checkbox.BindingContext as Course;
                if (course != null)
                {
                    course.IsSelected = checkbox.IsChecked;
                }
            }

            // Check if any course is selected to enable buttons
            var anySelected = _courses.Any(c => c.IsSelected);
            DeleteCourseButton.IsEnabled = anySelected;
            UpdateCourseButton.IsEnabled = anySelected;
        }
        private async void OnRefreshList(object sender, EventArgs e)
        {
            await LoadCourses();
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
