using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using AlfaLoggerRead.Extension;
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
        public MainViewModel(
            LogsRepository repository)
        {
            _repository = repository;
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
                    filters: GetFiltersOrNull()
                    );

            if (result.HasErrors)
            {
                Logs.Clear();
                return;
            }
            Logs = new ObservableCollection<LoggingEventDto>(result.Result);
        }

        private List<Expression<Func<Log, bool>>>? GetFiltersOrNull()
        {
            List<Expression<Func<Log, bool>>> filters = new();
            if (!string.IsNullOrEmpty(FilterPublishName))
            {
                var fiterPublishName = 
                    FilterPublishName
                        .Split("||")
                        .BuildContainsOrExpression<Log>(x => x.EventPublishName);
                filters.Add(fiterPublishName);
            }

            if (!string.IsNullOrEmpty(FilterMessage))
            {
                var fiterMessage =
                    FilterMessage
                        .Split("||")
                        .BuildContainsOrExpression<Log>(x => x.Message);
                filters.Add(fiterMessage);
            }

            if (!string.IsNullOrEmpty(FilterType))
            {
                var fiterType =
                    FilterType
                        .Split("||")
                        .BuildContainsOrExpression<Log>(x => x.TypeEvent.ToString());
                filters.Add(fiterType);
            }
            return filters.Any() ? filters : null;
        }

        private ObservableCollection<LoggingEventDto> _logs;
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



        private string _filterPublishName;
        public string FilterPublishName
        {
            get => _filterPublishName;
            set => Set(ref _filterPublishName, value);
        }


        private string _filterMessage;
        public string FilterMessage
        {
            get => _filterMessage;
            set => Set(ref _filterMessage, value);
        }


        private string _filterType;
        public string FilterType
        {
            get => _filterType;
            set => Set(ref _filterType, value);
        }

    }
}
