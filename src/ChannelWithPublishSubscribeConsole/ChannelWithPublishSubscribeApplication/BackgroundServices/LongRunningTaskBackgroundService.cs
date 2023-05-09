using Microsoft.OpenApi.Validations;

public sealed class LongRunningTaskBackgroundService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;

    public LongRunningTaskBackgroundService(
        IBackgroundTaskQueue taskQueue)
    {
        this._taskQueue = taskQueue;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                await Task.Delay(1000, stoppingToken);

                var task = await _taskQueue.DequeueAsync(stoppingToken);

                await task(stoppingToken);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                Console.WriteLine(
                    "Operation was cancelled because host is shutting down");
            }
            catch (AggregateException aggregateException)
            {
                Console.WriteLine(//aggregateException.Flatten(), 
                    "Aggregate exception occurred");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred executing task work item");
            }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
    }
}
