using ChannelSampleApplication.BackgroundServices;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(Channel.CreateUnbounded<int>(new UnboundedChannelOptions() { SingleReader = true }));
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<int>>().Reader);
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<int>>().Writer);

builder.Services.AddHostedService<ReaderBackgroundService>();
builder.Services.AddHostedService<WriterBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
