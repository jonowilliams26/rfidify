using RFIDify;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
var app = builder.Build();
app.Configure();
app.Run();