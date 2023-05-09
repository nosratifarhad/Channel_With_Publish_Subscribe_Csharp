using System.Threading.Channels;

namespace ChannelSampleApplication.BackgroundServices;

public class WriterBackgroundService : BackgroundService
{
    private readonly ChannelWriter<int> _channelWriter;
    public WriterBackgroundService(ChannelWriter<int> channelWriter)
    {
        _channelWriter = channelWriter;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int count = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
            Console.WriteLine("write item in channel");
            await _channelWriter.WriteAsync(count);
            count++;
        }
    }
}
