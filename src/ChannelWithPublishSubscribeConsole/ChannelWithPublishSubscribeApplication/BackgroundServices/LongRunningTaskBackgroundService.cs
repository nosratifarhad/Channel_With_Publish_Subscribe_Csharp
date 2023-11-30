
using ChannelWithPublishSubscribeApplication.Channels.Contracts;

namespace ChannelWithPublishSubscribeApplication.BackgroundServices;

public sealed class LongRunningTaskBackgroundService : BackgroundService
{
    #region [ Field ]

    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    #endregion [ Field ]

    #region [ Ctor ]

    public LongRunningTaskBackgroundService(IBackgroundTaskQueue backgroundTaskQueue)
    {
        this._backgroundTaskQueue = backgroundTaskQueue;
    }
    #endregion  [Ctor]

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(1000, stoppingToken);

                var task = await _backgroundTaskQueue.DequeueAsync(stoppingToken);

                await task(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled because host is shutting down");
            }
            catch (AggregateException)
            {
                Console.WriteLine("Aggregate exception occurred");
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred executing task work item");
            }
        }
    }
}
