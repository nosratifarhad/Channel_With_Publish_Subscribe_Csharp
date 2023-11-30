using System.Threading.Channels;

namespace ChannelSampleApplication.BackgroundServices;

public class ReaderBackgroundService : BackgroundService
{
    private readonly ChannelReader<int> _channelReader;

    public ReaderBackgroundService(ChannelReader<int> channelReader)
    {
        _channelReader = channelReader;
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
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            try
            {
                var result = await _channelReader.ReadAsync(stoppingToken);
                Console.WriteLine("read data : {0}", result);
            }
            catch (ChannelClosedException)
            {
                Console.WriteLine("Channel Closed.");
            }
        }
    }
}
