// mauiprogram.cs
using Microsoft.Extensions.Logging;
using ContactMauiApp.Services;
using ContactMauiApp.ViewModels;
using ContactMauiApp.Views;

namespace ContactMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // services stick around for the whole app lifetime (singleton)
        // handy for things that manage state or resources, like our contact service
        builder.Services.AddSingleton<ContactService>();

        // viewmodels and pages are created fresh each time they're needed (transient)
        // keeps things clean, less chance of holding onto old data between views
        builder.Services.AddTransient<AddContactViewModel>();
        builder.Services.AddTransient<ContactsViewModel>();
        builder.Services.AddTransient<ContactDetailViewModel>();

        // registering our pages
        builder.Services.AddTransient<AddContactPage>();
        builder.Services.AddTransient<ContactsPage>();
        builder.Services.AddTransient<ContactDetailPage>();

        // setting up navigation routes
        Routing.RegisterRoute(nameof(AddContactPage), typeof(AddContactPage));
        Routing.RegisterRoute(nameof(ContactsPage), typeof(ContactsPage));
        Routing.RegisterRoute(nameof(ContactDetailPage), typeof(ContactDetailPage));

        return builder.Build();
    }
}

// ethan rettinger
