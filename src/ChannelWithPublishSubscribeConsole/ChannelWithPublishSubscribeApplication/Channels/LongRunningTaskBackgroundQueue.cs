using ChannelWithPublishSubscribeApplication.Channels.Contracts;
using System.Threading.Channels;

namespace ChannelWithPublishSubscribeApplication.Channels;

public sealed class LongRunningTaskBackgroundQueue : IBackgroundTaskQueue
{
    #region [ Field ]
    private readonly Channel<Func<CancellationToken, Task>> _queue;
    #endregion [ Field ]

    #region [ Ctor ]
    public LongRunningTaskBackgroundQueue()
    {
        UnboundedChannelOptions options = new()
        {
            SingleReader = true,
            SingleWriter = false
        };
        _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>(options);
    }
    #endregion  [Ctor]

    public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem)
    {
        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        var workItem =
            await _queue.Reader.ReadAsync(cancellationToken);

        return workItem;
    }
}
