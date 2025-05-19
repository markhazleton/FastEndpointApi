using FastEndpointApi.services;
using FastEndpoints;
using FastEndpoints.ClientGen.Kiota;
using FastEndpoints.Swagger;
using Kiota.Builder;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Add a global error handler for consistent error responses
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        await context.Response.WriteAsJsonAsync(new { error = error?.Message ?? "An error occurred." });
    });
});

app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
});
app.UseSwaggerGen();
app.UseSwaggerUi();
// The file /swagger-inject.js is now served as a static file.
// If your Swagger UI library does not support direct JS injection via options,
// you may need to manually add <script src="/swagger-inject.js"></script> to the Swagger UI HTML template.
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

app.UseStaticFiles(); // Enable serving static files from wwwroot

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});
app.Run();
