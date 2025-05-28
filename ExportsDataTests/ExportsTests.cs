using AlfaLoggerRead.Services.Export;
using Data.Enums;
using Repository.DtoObjects;

namespace ExportsDataTests
{
    public class ExportsTests
    {
        [Fact]
        public async void Test1()
        {
            ExportLoggingEventDto sut = new ExportLoggingEventDto();
            List<LoggingEventDto> events = new List<LoggingEventDto>()
            {
                new LoggingEventDto("testName", "testMessage", TypeEvent.Information, DateTime.Now),
                new LoggingEventDto("testName1", "testMessage1", TypeEvent.Information, DateTime.Now),
                new LoggingEventDto("testName2", "testMessage2", TypeEvent.Information, DateTime.Now),
                new LoggingEventDto("testName3", "testMessage3", TypeEvent.Information, DateTime.Now),
                new LoggingEventDto("testName4", "testMessage4", TypeEvent.Information, DateTime.Now),
                new LoggingEventDto("testName5", "testMessage5", TypeEvent.Information, DateTime.Now),
            };
            await sut.ExportAsync(events);
        }
    }
}