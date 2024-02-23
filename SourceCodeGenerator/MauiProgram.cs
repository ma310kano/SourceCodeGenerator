using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SourceCodeGeneration.Application;

namespace SourceCodeGenerator
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			builder.Services.AddSingleton<IImmutableObjectGenerationService, ImmutableObjectGenerationService>();
			builder.Services.AddSingleton<IEntityGenerationService, EntityGenerationService>();
			builder.Services.AddSingleton<IValueObjectGenerationService, ValueObjectGenerationService>();

			return builder.Build();
		}
	}
}
