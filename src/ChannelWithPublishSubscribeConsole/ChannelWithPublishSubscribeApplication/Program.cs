using ChannelWithPublishSubscribeApplication.BackgroundServices;
using ChannelWithPublishSubscribeApplication.Channels;
using ChannelWithPublishSubscribeApplication.Channels.Contracts;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IBackgroundTaskQueue, LongRunningTaskBackgroundQueue>();
        builder.Services.AddHostedService<LongRunningTaskBackgroundService>();

        var app = builder.Build();
        app.UseHttpsRedirection();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}