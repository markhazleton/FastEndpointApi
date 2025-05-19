# Getting Started with FastEndPoints in .NET Web API

## Introduction

FastEndPoints is a lightweight REST API framework for ASP.NET that implements the REPR (Request-Endpoint-Response) Pattern. It's designed to be a developer-friendly alternative to Minimal APIs and MVC Controllers, reducing boilerplate code while maintaining high performance.

This guide will walk you through setting up a FastEndPoints project and implementing basic functionality, using real examples from this demo project.

## Why Use FastEndPoints?

FastEndPoints offers several advantages over traditional MVC Controllers and even Minimal APIs:

- **Clean Architecture**: Promotes the REPR pattern for organized, maintainable code
- **Performance**: Comparable to Minimal APIs and faster than MVC Controllers
- **Reduced Boilerplate**: Simplified endpoint creation with minimal setup code
- **Auto-Discovery**: Automatic registration of endpoints during application startup
- **Built-in Validation**: Seamless integration with FluentValidation
- **Security Support**: Easy implementation of authentication and authorization
- **Swagger Integration**: Simple API documentation with the FastEndPoints.Swagger package

## Installation

Create a new ASP.NET Web API project and install the FastEndPoints NuGet package:

```bash
dotnet new webapi -n MyFastEndPointsApi
cd MyFastEndPointsApi
dotnet add package FastEndPoints
```

For Swagger documentation support, add:

```bash
dotnet add package FastEndPoints.Swagger
```

## Basic Setup

Update your `Program.cs` file to configure FastEndPoints:

```csharp
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

// Add FastEndPoints
builder.Services.AddFastEndpoints();

// Add Swagger if needed
builder.Services.AddSwaggerDoc();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
    app.UseOpenApi();
    app.UseSwaggerUi3(s => s.ConfigureDefaults());
}

// Use FastEndPoints middleware (must be before UseRouting)
app.UseFastEndpoints();

app.Run();
```

## Creating Your First Endpoint

This project uses the REPR pattern (Request-Endpoint-Response) for the `Person` resource. Here are real examples from the demo:

### 1. Create a Request DTO

File: `endpoints/create/CreatePersonRequest.cs`

```csharp
namespace endpoints.create;

public class CreatePersonRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}
```

### 2. Create a Response DTO

File: `endpoints/PersonResponse.cs`

```csharp
namespace endpoints;

public class PersonResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public List<LinkResource> Links { get; set; }
}
```

### 3. Create an Endpoint

File: `endpoints/create/CreatePersonEndpoint.cs`

```csharp
using FastEndpoints;
using endpoints.create;
using endpoints;
using services;

public class CreatePersonEndpoint : Endpoint<CreatePersonRequest, PersonResponse>
{
    private readonly IPersonService _personService;

    public CreatePersonEndpoint(IPersonService personService)
    {
        _personService = personService;
    }

    public override void Configure()
    {
        Post("/api/person");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Creates a new person";
            s.Description = "Creates a new person with the provided information";
            s.ExampleRequest = new CreatePersonRequest
            {
                FirstName = "Jane",
                LastName = "Doe",
                Age = 30
            };
        });
    }

    public override async Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        var person = await _personService.CreatePersonAsync(req.FirstName, req.LastName, req.Age);
        var response = new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Age = person.Age,
            Links = new List<LinkResource>()
            {
                new LinkResource($"/api/person/{person.Id}", "self", "GET")
            }
        };
        await SendAsync(response, cancellation: ct);
    }
}
```

## Adding Validation

File: `endpoints/create/CreatePersonRequestValidator.cs`

```csharp
using FastEndpoints;
using FluentValidation;
using endpoints.create;

public class CreatePersonRequestValidator : Validator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters");

        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Age must be greater than 0");
    }
}
```

The validator will automatically be discovered and applied to the endpoint - no registration required.

## Advanced Usage

### Using Dependency Injection

This project uses a service for business logic. See `services/IPersonService.cs` and `services/PersonService.cs`:

```csharp
// services/IPersonService.cs
public interface IPersonService
{
    Task<PersonEntity> CreatePersonAsync(string firstName, string lastName, int age);
    // ...other methods...
}

// services/PersonService.cs
public class PersonService : IPersonService
{
    public async Task<PersonEntity> CreatePersonAsync(string firstName, string lastName, int age)
    {
        // Implementation (e.g., database operations)
        await Task.Delay(100); // Simulating async work
        return new PersonEntity
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Age = age
        };
    }
    // ...other methods...
}
```

Register the service in `Program.cs`:

```csharp
builder.Services.AddSingleton<IPersonService, PersonService>();
```

### Working with Different HTTP Methods

This project includes endpoints for reading, updating, and deleting a person. For example:

- `endpoints/read/ReadPersonEndpoint.cs` (GET)
- `endpoints/update/UpdatePersonEndpoint.cs` (PUT)
- `endpoints/delete/DeletePersonEndpoint.cs` (DELETE)

Example for reading a person:

```csharp
// endpoints/read/ReadPersonEndpoint.cs
using FastEndpoints;
using endpoints.read;
using endpoints;
using services;

public class ReadPersonEndpoint : Endpoint<ReadPersonRequest, PersonResponse>
{
    private readonly IPersonService _personService;

    public ReadPersonEndpoint(IPersonService personService)
    {
        _personService = personService;
    }

    public override void Configure()
    {
        Get("/api/person/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        var person = await _personService.GetPersonByIdAsync(req.Id);
        if (person == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        var response = new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Age = person.Age,
            Links = new List<LinkResource>()
            {
                new LinkResource($"/api/person/{person.Id}", "self", "GET")
            }
        };
        await SendAsync(response, cancellation: ct);
    }
}
```

## Error Handling and Responses

This project uses FastEndPoints' built-in response helpers. For example, in `ReadPersonEndpoint` above, `SendNotFoundAsync(ct)` is used if the person is not found.

Other response helpers include:

```csharp
await SendAsync(response, cancellation: ct); // 200 OK with body
await SendOkAsync(ct); // 200 OK without body
await SendCreatedAtAsync("/api/person/{id}", response, cancellation: ct); // 201 Created
await SendErrorsAsync(ct); // 400 Bad Request with validation errors
await SendUnauthorizedAsync(ct); // 401 Unauthorized
await SendForbiddenAsync(ct); // 403 Forbidden
```

## Security and Authentication

You can secure endpoints using FastEndPoints' built-in methods. For example, in an endpoint's `Configure()` method:

```csharp
public override void Configure()
{
    Post("/api/person");
    // AllowAnonymous(); // No auth required
    // OR
    // Roles("Admin"); // Require specific roles
    // OR
    // PolicyName("AdminOnly"); // Use policy
}
```

## Custom Conventions

You can customize FastEndPoints behavior in `Program.cs`:

```csharp
app.UseFastEndpoints(config =>
{
    // Global route prefix
    config.Endpoints.RoutePrefix = "api";
    // Custom error responses
    config.Errors.ResponseBuilder = (failures, _, statusCode) => 
    {
        return new 
        {
            Status = statusCode,
            Message = "Validation failed",
            Errors = failures.GroupBy(f => f.PropertyName)
                             .ToDictionary(
                                 g => g.Key,
                                 g => g.Select(f => f.ErrorMessage).ToArray()
                              )
        };
    };
});
```

## Conclusion

FastEndPoints provides a clean, organized way to build REST APIs in ASP.NET by implementing the REPR pattern. Its focus on developer productivity, performance, and built-in features like validation and security makes it an excellent choice for modern API development.

For more detailed information, check out the [official FastEndPoints documentation](https://fast-endpoints.com/).

## Additional Resources

- [FastEndPoints GitHub Repository](https://github.com/FastEndpoints/FastEndpoints)
- [NuGet Package](https://www.nuget.org/packages/FastEndpoints/)
- [Swagger Support](https://www.nuget.org/packages/FastEndpoints.Swagger/)
- [Security Package](https://www.nuget.org/packages/FastEndpoints.Security/)
