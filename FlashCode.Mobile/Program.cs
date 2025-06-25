using Microsoft.Maui;
using Microsoft.Maui.Hosting;

var builder = MauiApp.CreateBuilder();
builder.UseMauiApp<App>();

var app = builder.Build();
app.Run();

public class App : Application
{
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
    }
}
