using DoreanStore.Repositories;
using DoreanStore.Services;
using Microsoft.Extensions.Logging;

namespace DoreanStore
{
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<FdroidService>();
            builder.Services.AddScoped<IOService>();
            builder.Services.AddScoped<JsonService>();
            builder.Services.AddScoped<IndexRepository>();
            builder.Services.AddSingleton(typeof(GenericRepository<>));
#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
