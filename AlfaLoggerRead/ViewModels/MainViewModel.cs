using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Input;
using AlfaLoggerRead.Extension;
using AlfaLoggerRead.Services.Export;
using Data.Entities;
using LoggerReader.Infrastructure.Command;
using LoggerReader.ViewModels.Base;
using Repository;
using Repository.DtoObjects;

namespace AlfaLoggerRead.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly LogsRepository _repository;
        private readonly IExport<IEnumerable<LoggingEventDto>> _export;
        public MainViewModel(
            LogsRepository repository,
            IExport<IEnumerable<LoggingEventDto>> export)
        {
            _repository = repository;
            _export = export;
        }

        private DateTime _dateStart = DateTime.Now;
        public string DateTimeStartString
        {
            get => _dateStart.ToString(CultureInfo.CurrentCulture);
            set => _dateStart = DateTime.Parse(value);
        }

        private DateTime _dateFinish = DateTime.Now;
        public string DateTimeFinishToString
        {
            get => _dateFinish.ToString(CultureInfo.CurrentCulture);
            set => _dateFinish = DateTime.Parse(value);
        }


        private string _title = "AlfaLoggReader";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private int _numPage = 1;
        public int NumPage
        {
            get => _numPage;
            set => Set(ref _numPage, value);
        }

        private int _sizePage = 50;
        public int SizePage
        {
            get => _sizePage;
            set => Set(ref _sizePage, value);
        }

        private ICommand? _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand ??= new LambdaCommand
            (OnRefreshCommandExecuted);
        private async void OnRefreshCommandExecuted(object p)
        {
            await ResponseRepository();
        }
        public async Task ResponseRepository()
        {

            var result = 
                await _repository.Events
                    (size: SizePage,
                    zeroStart: NumPage - 1,
                    filters: GetFilters()
                    );

            if (result.HasErrors)
            {
                Logs.Clear();
                return;
            }
            Logs = new ObservableCollection<LoggingEventDto>(result.Result);
        }

        private List<Expression<Func<Log, bool>>> GetFilters()
        {
            List<Expression<Func<Log, bool>>> filters = new();
            if (!string.IsNullOrEmpty(FilterPublishName))
            {
                filters.Add(
                    FilterPublishName
                        .Split("||")
                        .BuildContainsOrExpression<Log>(x => x.EventPublishName));
            }
            if (!string.IsNullOrEmpty(FilterMessage))
            {
                filters.Add(FilterMessage
                    .Split("||")
                    .BuildContainsOrExpression<Log>(x => x.Message));
            }
            if (!string.IsNullOrEmpty(FilterType))
            {
                filters.Add(FilterType
                    .Split("||")
                    .BuildContainsOrExpression<Log>(x => x.TypeEvent.ToString()));
            }
            filters.Add(x => x.Date >= _dateStart && x.Date <= _dateFinish);

            return filters;
        }

        private ObservableCollection<LoggingEventDto> _logs = new();
        public ObservableCollection<LoggingEventDto> Logs
        {
            get => _logs;
            set => Set(ref _logs, value);
        }

        private ICommand? _nextPageCommand;
        public ICommand NextCommand =>
            _nextPageCommand ??= new LambdaCommand(
                OnNextPageCommandExecuted);
        private void OnNextPageCommandExecuted(object p)
        {
            NumPage++;
            RefreshCommand.Execute(p);
        }
        private ICommand? _backPageCommand;
        public ICommand BackCommand =>
            _backPageCommand ??= new LambdaCommand(
                OnBackCommandExecuted, CanBackCommandExecute);
        private bool CanBackCommandExecute(object p) => NumPage >= 2;
        private void OnBackCommandExecuted(object p)
        {
            if(!CanBackCommandExecute(null!))
                return;
            NumPage--;
            RefreshCommand.Execute(p);
        }


        #region ExportToXlsxCommand Export data to excel file


        private ICommand? _exportToXlsxCommand;

        public ICommand ExportToXlsxCommand =>
            _exportToXlsxCommand ??= new LambdaCommandAsync(OnExportToXlsxCommandExecuted, CanExportToXlsxCommandExecute);

        private bool CanExportToXlsxCommandExecute(object p) => Logs.Any();

        private  Task OnExportToXlsxCommandExecuted(object p)
        {
            _export.ExportAsync(Logs);
            return Task.CompletedTask;
        }

        #endregion  



        private string _filterPublishName = string.Empty;
        public string FilterPublishName
        {
            get => _filterPublishName;
            set => Set(ref _filterPublishName, value);
        }


        private string _filterMessage = string.Empty;
        public string FilterMessage
        {
            get => _filterMessage;
            set => Set(ref _filterMessage, value);
        }


        private string _filterType = string.Empty;
        public string FilterType
        {
            get => _filterType;
            set => Set(ref _filterType, value);
        }

    }
}
