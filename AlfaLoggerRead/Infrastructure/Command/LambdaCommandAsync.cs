namespace LoggerReader.Infrastructure.Command;

internal class LambdaCommandAsync : LoggerReader.Infrastructure.Command.Command
{
    private readonly Func<object, Task> _executeAsync;
    private readonly Func<object?, bool>? _canExecuteAsync;

    private volatile Task? _executingTask;

    /// <summary>Выполнять задачу принудительно в фоновом потоке</summary>
    public bool Background { get; set; }

    public LambdaCommandAsync(Func<object, Task> executeAsync, Func<object?, bool>? canExecuteAsync = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecuteAsync = canExecuteAsync;
    }
    protected override bool CanExecute(object? parameter) =>
        (_executingTask is null || _executingTask.IsCompleted)
        && (_canExecuteAsync?.Invoke(parameter) ?? true);

    protected override async void Execute(object? parameter)
    {
        var background = Background;

        var canExecute = background
            ? await Task.Run(() => CanExecute(parameter))
            : CanExecute(parameter);
        if (!canExecute) return;

        var executeAsync = background ? Task.Run(() => _executeAsync(parameter)) : _executeAsync(parameter);
        _ = Interlocked.Exchange(ref _executingTask, executeAsync);
        _executingTask = executeAsync;
        OnCanExecuteChanged();

        try
        {
            await executeAsync.ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {

        }

        OnCanExecuteChanged();
    }
}