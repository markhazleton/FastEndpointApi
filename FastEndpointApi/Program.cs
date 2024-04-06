using FastEndpointApi.services.person;
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
builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
