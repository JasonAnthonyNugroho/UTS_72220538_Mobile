
using UTS_72220538.Models;
using Microsoft.Maui.Controls;
namespace UTS_72220538.Page
{
    public partial class EditCourse : ContentPage
    {
        private readonly Course _course;

        public EditCourse(Course course)
        {
            InitializeComponent();
            _course = course;

            CourseNameEntry.Text = _course.Name; 
            CourseDescriptionEntry.Text = _course.Description; 
        }

        private async void OnSaveChanges(object sender, EventArgs e)
        {
            _course.Name = CourseNameEntry.Text;
            _course.Description = CourseDescriptionEntry.Text;

            await Navigation.PopAsync();
        }
    }
}