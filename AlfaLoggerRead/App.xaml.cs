using System.Configuration;
using System.Data;
using System.Windows;
using AlfaLoggerLib.Extension;
using AlfaLoggerLib.Logging;
using LoggerReader;
using LoggerReader.Extension;
using LoggerReader.Services.UserDialogs;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace AlfaLoggerRead
{
    public partial class App
    {
        private readonly IServiceProvider? _provider;

        private IServiceCollection ConfigureNewServices() =>
            new ServiceCollection()
                .ViewModelRegistrar()
                .UserDialogs()
                .AddAlfaLogger()
                .AddRepositoryLogs()
        ;

        public App()
        {
            _provider = ConfigureNewServices().BuildServiceProvider();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await using var scope =
                _provider!.CreateAsyncScope();

            if (scope.ServiceProvider.GetRequiredService<IUserDialog>().Show() &
                await scope.ServiceProvider.GetRequiredService<LoggerInitialization>().InitializeAsync())
            {
                scope.ServiceProvider.GetRequiredService<MainWindow>().Show();
            }
            else throw new Exception();
        }
    }


}
