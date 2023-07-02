using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Splat;
using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.Configuration;
using AwsCredentialManager.Core.Services;
using AwsCredentialManager.ViewModels;

namespace AwsCredentialManager
{
	public class Program
	{
		public enum AppCommand
		{
			UpdateCreds,
			CopyToken,
		}

		private const string ParameterNameAwsToken = $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.TokenCode)}";

		public readonly static Dictionary<string, string> CommandlineShortKeyMap = new Dictionary<string, string>()
		{
			{ "-C", ParameterNameAwsToken },
			{ "-T", ParameterNameAwsToken },
			{ "-A", $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.AccountId)}" },
			{ "-U", $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.UserName)}" },
			{ "-S", $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.ProfileSource)}" },
			{ "-P", $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.Profile)}" },
			{ "-M", $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.MfaGeneratorSecretKey)}" },
		};

		public static bool IsHelpCommand(string[] args)
		{
			return args?.Any(arg =>
				IsCommandArgument(arg, "h") ||
				IsCommandArgument(arg, "help")
			) ?? false;
		}

		public static void DisplayHelp()
		{
			// TODO: make the project able to print to the console, at the moment Console.WriteLine doesn't work with project type WinExe.
			Console.WriteLine($"Aws Credentials Manager HELP:");
			foreach (var shortCommand in CommandlineShortKeyMap)
			{
				Console.WriteLine($"{shortCommand.Key}  {shortCommand.Value}");
			}
		}

		// Initialization code. Don't use any Avalonia, third-party APIs or any SynchronizationContext-reliant code before AppMain is called:
		// things aren't initialized yet and stuff might break.
		[STAThread]
		public static void Main(string[] args)
		{
			Console.WriteLine($"Starting {nameof(AwsCredentialManager)} app...");

			if (IsHelpCommand(args)) {
				DisplayHelp();
				return;
			}

			var configBuilder = new ConfigurationBuilder()
                .SetBasePath(GetExecutingDir())
				.AddJsonFile(AppSettings.FILENAME, optional: true)
                .AddUserSecrets<Program>(optional: true)
				.AddCommandLine(args, CommandlineShortKeyMap);
			var config = configBuilder.Build();

			// Dependency Injection.
			RegisterServices(config);

			var awsSettings = Splat.Locator.Current.GetService<AwsSettings>();
			var hasTokenCodeParam = !string.IsNullOrWhiteSpace(awsSettings?.TokenCode); // hasTokenCodeParam()
            if (hasTokenCodeParam)
			{
				var viewModel = Splat.Locator.Current.GetService<AwsCredentialManagerViewModel>();
				viewModel.UpdateCredentialsCommand().Wait();
                Console.WriteLine(viewModel.Logs);
				return;
			}

			var isCopyTokenCommandLineRequest = HasCommandArgument(args, AppCommand.CopyToken);
			if (isCopyTokenCommandLineRequest)
			{
				var viewModel = Splat.Locator.Current.GetService<AwsCredentialManagerViewModel>();
				viewModel.GenerateTokenAndCopyToClipboardCommand().Wait();
				Console.WriteLine(viewModel.Logs);
				return;
			}

			var isUpdateCredsCommandLineRequest = HasCommandArgument(args, AppCommand.UpdateCreds);
			if (isUpdateCredsCommandLineRequest)
			{
				var viewModel = Splat.Locator.Current.GetService<AwsCredentialManagerViewModel>();
				viewModel.AutoUpdateCredentialsCommand().Wait();
				Console.WriteLine(viewModel.Logs);
				return;
			}

			BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
		}

		public static bool hasTokenCodeParam(string[] args)
        {
            var parameterNameAwsToken = $"{nameof(AppSettings.Aws)}:{nameof(AwsSettings.TokenCode)}";
            var isTokenParameterInCommandLine = args.Any(arg => IsCommandArgument(arg, parameterNameAwsToken));
			return isTokenParameterInCommandLine;
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.LogToTrace()
				.UseReactiveUI();


        private static string GetExecutingDir()
        {
            return System.AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
        }

        // https://www.reactiveui.net/docs/handbook/dependency-inversion/
        // https://dev.to/ingvarx/avaloniaui-dependency-injection-4aka
        // Example: https://github.com/rbmkio/radish/blob/master/src/Rbmk.Radish/Program.cs
        // Other ways of DI: https://github.com/egramtel/egram.tel/blob/master/src/Tel.Egram/Program.cs
        public static void RegisterServices(IConfiguration config)
		{
			RegisterServices(Locator.CurrentMutable, Locator.Current, config);
		}

		public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver, IConfiguration config)
		{
			if (config == null) { throw new ArgumentException($"Null parameter '{nameof(config)}'."); }
			var appSettings = config.Get<AppSettings>() ?? new AppSettings();

			services.Register<AppSettings>(() => config.Get<AppSettings>() ?? new AppSettings());
			services.Register<AppSettingsWriter>(() => new AppSettingsWriter());
			services.Register<IAwsCredentialUpdater>(() => new Core.Services.AwsCredentialUpdaterCmd());
			services.Register<IAwsCredentialManager>(() => new Core.Services.AwsCredentialManager());
			services.Register<AwsSettings>(() => appSettings.Aws);
			services.Register<AwsCredentialManagerViewModel>(() => new AwsCredentialManagerViewModel());
			
		}

		public static bool HasCommandArgument(string[] args, AppCommand command)
		{
			return args.Any(arg => IsCommandArgument(arg, command));
		}

		public static bool IsCommandArgument(string arg, AppCommand command)
		{
			return IsCommandArgument(arg, command.ToString());
		}

		public static bool IsCommandArgument(string arg, string command)
		{
			var commandName = command.ToLower();
			var argName = arg.ToLower().TrimStart('-', '/', '\\').Trim();
			return argName == commandName;
		}

	}
}
