using Microsoft.Extensions.Logging;
using System.Net.Http;
using UTS_72220538.Services;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using UTS_72220538.Page;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;


namespace UTS_72220538
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
            .UseMauiApp<App>() // Tambahkan baris ini agar aplikasi dapat dijalankan
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

            builder.Services.AddHttpClient<CourseService>(client =>
            {
                client.BaseAddress = new Uri("https://actualbackendapp.azurewebsites.net/api/Courses");
            });

            builder.Services.AddHttpClient<CategoryService>(client =>
            {
                client.BaseAddress = new Uri("https://actualbackendapp.azurewebsites.net/api/v1/Categories");
            });

            builder.Services.AddSingleton<CoursePage>();
            builder.Services.AddSingleton<CategoryPage>();

            builder.Services.AddSingleton<CourseService>();
            builder.Services.AddSingleton<CategoryService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
