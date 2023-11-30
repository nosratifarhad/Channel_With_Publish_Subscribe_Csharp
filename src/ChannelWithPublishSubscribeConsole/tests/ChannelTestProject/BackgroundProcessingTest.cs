using ChannelWithPublishSubscribeApplication.Channels.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Xunit.Abstractions;


namespace ChannelTestProject;

public class BackgroundProcessingTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _applicationFactory;
    private readonly ITestOutputHelper _testOutputHelper;

    public BackgroundProcessingTest(ITestOutputHelper testOutputHelper,
        WebApplicationFactory<Program> applicationFactory)
    {
        _testOutputHelper = testOutputHelper;
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task ShouldProcessTaskInBackgroundWhenWrittenToQueue()
    {
        var backgroundTaskQueue = _applicationFactory.Services.GetRequiredService<IBackgroundTaskQueue>();
        var expectedCount = 2;

        var countdownEvent = new CountdownEvent(expectedCount);

        // for fun ;)
        for (var i = 0; i < 2; i++)
            await backgroundTaskQueue.QueueBackgroundWorkItemAsync(token =>
            {
                //decrement the count
                countdownEvent.Signal();

                _testOutputHelper.WriteLine(
                    $"I was called by the background service, should be called {countdownEvent.CurrentCount} times more.");

                return Task.CompletedTask;
            });

        //Waiting for 12 seconds because we intentionally waiting 5 secs in background service
        //These waits can be removed its only to show you how its working

        var countReached = countdownEvent.Wait(TimeSpan.FromSeconds(12));
        Assert.True(countReached);
    }
}
