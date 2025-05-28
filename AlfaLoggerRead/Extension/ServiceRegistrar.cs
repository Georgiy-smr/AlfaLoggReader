using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlfaLoggerRead.Services.Export;
using LoggerReader.Services.UserDialogs;
using Microsoft.Extensions.DependencyInjection;
using Repository.DtoObjects;

namespace LoggerReader.Extension
{
    internal static class ServiceRegistrar
    {
        internal static IServiceCollection UserDialogs(this IServiceCollection service)
        {
            return service.AddSingleton<IUserDialog, FileUserDialog>();
        }
        internal static IServiceCollection AddExport(this IServiceCollection service)
        {
            return service.AddSingleton<IExport<IEnumerable<LoggingEventDto>>, ExportLoggingEventDto>();
        }
    }
}
