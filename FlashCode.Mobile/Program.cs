using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls;

var builder = MauiApp.CreateBuilder();
builder.UseMauiApp<App>();

var app = builder.Build();
app.Run();

class App : Application
{
    public App()
    {
        MainPage = new LoginPage();
    }
}
