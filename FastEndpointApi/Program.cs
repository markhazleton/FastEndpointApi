using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "FastEndpoints API";
        s.Version = "v1";
    };
});
var app = builder.Build();
app.UseFastEndpoints().UseSwaggerGen();
app.Run();
