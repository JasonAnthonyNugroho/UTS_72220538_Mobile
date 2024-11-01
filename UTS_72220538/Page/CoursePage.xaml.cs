using UTS_72220538.Models; // Pastikan Anda memiliki model Course
using UTS_72220538.Services; // Pastikan Anda memiliki CourseService
using System;
using Microsoft.Maui.Controls;
namespace UTS_72220538.Page
{
    public partial class CoursePage : ContentPage
    {
        private readonly CourseService _courseService;

        public CoursePage(CourseService courseService)
        {
            InitializeComponent(); // Memanggil metode yang dihasilkan otomatis
            _courseService = courseService;
            LoadCourses();
        }

        private async void LoadCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                CoursesListView.ItemsSource = courses; // Akses ke CoursesListView di sini
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnAddCourse(object sender, EventArgs e)
        {
            var newCourse = new Course { Name = "New Course" };
            await _courseService.CreateCourseAsync(newCourse);
            LoadCourses();
        }

        private void OnCourseSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Course selectedCourse)
            {
                DisplayAlert("Selected Course", selectedCourse.Name, "OK");
                CoursesListView.SelectedItem = null; // Menghapus pilihan
            }
        }
    }
}