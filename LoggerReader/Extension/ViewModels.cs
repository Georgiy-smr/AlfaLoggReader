using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggerReader.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LoggerReader.Extension
{
    internal static class ViewModels
    {
        internal static IServiceCollection ViewModelRegistrar(this IServiceCollection service)
        {
            return service.MainViewModelRegistrar();
        }
        private static IServiceCollection MainViewModelRegistrar(this IServiceCollection service)
        {
            return service
                .AddSingleton<MainViewModel>()
                .AddSingleton(provider => new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainViewModel>()
                });
        }
    }
}
