using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggerReader.Services.UserDialogs;
using Microsoft.Extensions.DependencyInjection;

namespace LoggerReader.Extension
{
    internal static class ServiceRegistrar
    {
        internal static IServiceCollection UserDialogs(this IServiceCollection service)
        {
            return service.AddSingleton<IUserDialog, FileUserDialog>();
        }
    }
}
