using FastEndpointApi.services;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
