using FastEndpointApi.services;
using FastEndpoints;
using FastEndpoints.ClientGen.Kiota;
using FastEndpoints.Swagger;
using Kiota.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v1"; //must match what's being passed in to the map method below
    };
});
builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();
app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
});
app.UseSwaggerGen();
app.MapApiClientEndpoint("/cs-client", c =>
{
    c.SwaggerDocumentName = "v1"; //must match document name set above
    c.Language = GenerationLanguage.CSharp;
    c.ClientNamespaceName = "MarkHazleton";
    c.ClientClassName = "FastEndpoint.Demo";
},
o => //endpoint customization settings
{
    o.CacheOutput(p => p.Expire(TimeSpan.FromDays(365))); //cache the zip
    o.ExcludeFromDescription(); //hides this endpoint from swagger docs
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.Run();
