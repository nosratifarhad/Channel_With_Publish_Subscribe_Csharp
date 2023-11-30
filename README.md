# Hello  

## I have added two projects to this repository to help convey my point and provide more context for better understanding.

projct Number one :
### you need two BackgroundService for write item in channel And read item .
```csharp
// read item in channel
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        await Task.Delay(1000, stoppingToken);
        try
        {
            var result = await _channelReader.ReadAsync(stoppingToken);
            Console.WriteLine("read item : {0}", result);
        }
        catch (ChannelClosedException)
        {
            Console.WriteLine("Channel Closed.");
        }
    }
}
// write item in channel
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
```
### now you must add services 
```csharp
builder.Services.AddSingleton(Channel.CreateUnbounded<int>(new UnboundedChannelOptions() { SingleReader = true }));
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<int>>().Reader);
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<int>>().Writer);

builder.Services.AddHostedService<ReaderBackgroundService>();
builder.Services.AddHostedService<WriterBackgroundService>();
```
![My Remote Image](https://github.com/nosratifarhad/Publish_Subscribe_With_Channel_DotNet6/blob/main/src/imgs/Annotation.jpg)

I Dont hal for type to how to worker :))))))
