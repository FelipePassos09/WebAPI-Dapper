using TarefasApi.Endpoints;
using TarefasApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddPersistence();


var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.MapTarefasEndpoints();

app.Run();
