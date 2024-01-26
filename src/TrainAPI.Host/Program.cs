using TrainAPI.Host.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore();
builder.AddHangfire();
var app = builder.Build();
await app.AddCore();
app.Run();

// for integration tests
namespace TrainAPI.Host
{
    public partial class Program;
}