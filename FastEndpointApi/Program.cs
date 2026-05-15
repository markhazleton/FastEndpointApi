using FastEndpointApi.services;
using FastEndpoints;
using FastEndpoints.ClientGen.Kiota;
using FastEndpoints.Swagger;
using Kiota.Builder;
using System.Text;
using System.Text.RegularExpressions;

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

const string GoogleAnalyticsTag = """
<!-- Google tag (gtag.js) -->
<script async src="https://www.googletagmanager.com/gtag/js?id=G-RY77Z11S9E"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());

  gtag('config', 'G-RY77Z11S9E');
</script>
""";

app.Use(async (context, next) =>
{
    var originalBody = context.Response.Body;
    await using var responseBuffer = new MemoryStream();
    context.Response.Body = responseBuffer;

    await next().ConfigureAwait(false);

    context.Response.Body = originalBody;

    var status = context.Response.StatusCode;
    var bodyNotAllowed =
        (status >= 100 && status < 200) ||
        status == StatusCodes.Status204NoContent ||
        status == StatusCodes.Status304NotModified ||
        HttpMethods.IsHead(context.Request.Method);

    if (bodyNotAllowed)
    {
        context.Response.ContentLength = 0;
        return;
    }

    responseBuffer.Position = 0;

    if (context.Response.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true)
    {
        using var reader = new StreamReader(responseBuffer, Encoding.UTF8, leaveOpen: true);
        var html = await reader.ReadToEndAsync().ConfigureAwait(false);

        html = Regex.Replace(
            html,
            "<script[^>]*src=\"https://www\\.googletagmanager\\.com/gtag/js\\?id=[^\"]+\"[^>]*>\\s*</script>",
            string.Empty,
            RegexOptions.IgnoreCase);

        html = Regex.Replace(
            html,
            "<script>\\s*window\\.dataLayer\\s*=\\s*window\\.dataLayer\\s*\\|\\|\\s*\\[\\];[\\s\\S]*?gtag\\('config',\\s*'[^']+'\\s*\\);\\s*</script>",
            string.Empty,
            RegexOptions.IgnoreCase);

        if (!html.Contains("G-RY77Z11S9E", StringComparison.OrdinalIgnoreCase) &&
            html.Contains("</head>", StringComparison.OrdinalIgnoreCase))
        {
            html = html.Replace(
                "</head>",
                $"{GoogleAnalyticsTag}{Environment.NewLine}</head>",
                StringComparison.OrdinalIgnoreCase);
        }

        var output = Encoding.UTF8.GetBytes(html);
        context.Response.ContentLength = output.Length;
        await context.Response.Body.WriteAsync(output).ConfigureAwait(false);
        return;
    }

    responseBuffer.Position = 0;
    await responseBuffer.CopyToAsync(originalBody).ConfigureAwait(false);
});

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
        await context.Response.WriteAsJsonAsync(new { error = error?.Message ?? "An error occurred." }).ConfigureAwait(false);
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
    await context.Response.SendFileAsync("wwwroot/index.html").ConfigureAwait(false);
});
app.Run();
