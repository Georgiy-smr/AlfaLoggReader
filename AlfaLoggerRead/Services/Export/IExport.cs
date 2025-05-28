using OfficeOpenXml;
using Repository.DtoObjects;
using System.IO;

namespace AlfaLoggerRead.Services.Export;

public interface IExport<in T>
{
    Task ExportAsync(T objectToExport, CancellationToken token = default);
}

public class ExportLoggingEventDto : IExport<IEnumerable<LoggingEventDto>>
{
    public async Task ExportAsync(IEnumerable<LoggingEventDto> objectToExport, CancellationToken token = default)
    {

        // Убедитесь, что вы используете EPPlus версии 5 и выше
        // Для использования EPPlus необходимо установить лицензии на использование
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

        // Создаем новый Excel файл
        using var package = new ExcelPackage();
        // Добавляем новый лист
        var worksheet = package.Workbook.Worksheets.Add($"Logs");

        // Сохраняем файл
        var date = DateTime.Now.ToLongDateString();
        var time = DateTime.Now.ToLongTimeString().Replace(":","-");

        var filePath = Path.Combine(Environment.CurrentDirectory, $"LogsExport_{date}_{time}.xlsx");
        FileInfo excelFile = new FileInfo(filePath);
        await FillTemperatureAndDate(worksheet, objectToExport.ToAsyncEnumerable());
        await package.SaveAsAsync(excelFile, token);
    }


    private async Task FillTemperatureAndDate(ExcelWorksheet worksheet, IAsyncEnumerable<LoggingEventDto> data)
    {
        int row = 1;
        await foreach (var eventData in data)
        {
            worksheet.Cells[row + 1, 1].Value = eventData.Date;
            worksheet.Cells[row + 1, 2].Value = eventData.EventPublishName;
            worksheet.Cells[row + 1, 3].Value = eventData.TypeEvent;
            worksheet.Cells[row + 1, 4].Value = eventData.Message;
            row++;
        }
    }
}

