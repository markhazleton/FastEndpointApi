# Getting Started with FastEndPoints in .NET Web API

## Introduction

FastEndPoints is a lightweight REST API framework for ASP.NET that implements the REPR (Request-Endpoint-Response) Pattern. It's designed to be a developer-friendly alternative to Minimal APIs and MVC Controllers, reducing boilerplate code while maintaining high performance.

This guide will walk you through setting up a FastEndPoints project and implementing basic functionality, using real examples from this demo project.

## Project Upgrade History

### .NET 10 Migration

This project has been upgraded to **.NET 10** with the following package updates:

- **FastEndpoints**: Updated to version **7.1.1**
- **FastEndpoints.Attributes**: Updated to version **7.1.1**
- **FastEndpoints.Swagger**: Updated to version **7.1.1**
- **FastEndpoints.ClientGen.Kiota**: Updated to version **7.1.1**
- **Bogus**: Updated to version **35.6.5**

### Breaking Changes in FastEndpoints 7.x

FastEndpoints 7.x introduced significant API changes that required updates to all endpoints. The following helper methods were **removed**:

- `SendAsync(response, cancellation: ct)`
- `SendNoContentAsync(cancellation: ct)`
- `SendNotFoundAsync(cancellation: ct)`
- `SendOkAsync(cancellation: ct)`
- `SendCreatedAtAsync()`

### Migration Solution

All endpoints now use `HttpContext.Response` directly for sending responses:

**Before (FastEndpoints 5.x/6.x):**
```csharp
public override async Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
{
    Response = Map.FromEntity(entity);
    await SendAsync(Response, 201, ct);
}
```

**After (FastEndpoints 7.x):**
```csharp
public override async Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
{
    Response = Map.FromEntity(entity);
    HttpContext.Response.StatusCode = 201;
    await HttpContext.Response.WriteAsJsonAsync(Response, ct);
}
```

**Common Response Patterns:**
```csharp
// 200 OK with JSON body
await HttpContext.Response.WriteAsJsonAsync(response, ct);

// 201 Created with JSON body
HttpContext.Response.StatusCode = 201;
await HttpContext.Response.WriteAsJsonAsync(response, ct);

// 204 No Content
HttpContext.Response.StatusCode = 204;
await Task.CompletedTask;

// 404 Not Found
HttpContext.Response.StatusCode = 404;
return;
```

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
        
        // FastEndpoints 7.x: Use HttpContext.Response directly
        HttpContext.Response.StatusCode = 201; // Created
        await HttpContext.Response.WriteAsJsonAsync(response, ct);
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

## CRUD Endpoints for Person (RESTful)

- **Create**: `POST /person` — Create a new person
- **Read All**: `GET /person` — Get all people
- **Read One**: `GET /person/{id}` — Get a person by ID
- **Update**: `PUT /person/{id}` — Update a person by ID
- **Delete**: `DELETE /person/{id}` — Delete a person by ID

All endpoints follow the REPR pattern and return consistent response models.

### Data Seeding

At startup, the application seeds 50 unique people using the [Bogus](https://github.com/bchavez/Bogus) library for realistic test data.

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
            // FastEndpoints 7.x: Use HttpContext.Response directly
            HttpContext.Response.StatusCode = 404;
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
        
        // FastEndpoints 7.x: Use HttpContext.Response directly
        await HttpContext.Response.WriteAsJsonAsync(response, ct);
    }
}
```

## Error Handling and Responses

FastEndpoints 7.x requires using `HttpContext.Response` directly for all response operations. Here are the common patterns:

### Success Responses

```csharp
// 200 OK with JSON response
await HttpContext.Response.WriteAsJsonAsync(response, ct);

// 201 Created with JSON response
HttpContext.Response.StatusCode = 201;
await HttpContext.Response.WriteAsJsonAsync(response, ct);

// 204 No Content (typically for DELETE operations)
HttpContext.Response.StatusCode = 204;
await Task.CompletedTask;
```

### Error Responses

```csharp
// 400 Bad Request with validation errors
HttpContext.Response.StatusCode = 400;
await HttpContext.Response.WriteAsJsonAsync(new { errors = ValidationFailures }, ct);

// 401 Unauthorized
HttpContext.Response.StatusCode = 401;
return;

// 403 Forbidden
HttpContext.Response.StatusCode = 403;
return;

// 404 Not Found
HttpContext.Response.StatusCode = 404;
return;

// 500 Internal Server Error
HttpContext.Response.StatusCode = 500;
await HttpContext.Response.WriteAsJsonAsync(new { error = "An error occurred" }, ct);
```

### Real-World Example

Here's a complete example from the project showing proper error handling:

```csharp
public override async Task HandleAsync(UpdatePersonRequest req, CancellationToken ct)
{
    var person = personService.UpdatePerson(req.Id.ToString(), new PersonEntity
    {
        FirstName = req.FirstName,
        LastName = req.LastName,
        Age = req.Age,
        Email = req.Email
    });

    if (person == null)
    {
        HttpContext.Response.StatusCode = 404;
        return;
    }

    Response = new PersonResponse
    {
        FullName = $"{person.FirstName} {person.LastName}",
        IsOver18 = person.Age > 18,
        PersonId = person.Id.ToString()
    };
    await HttpContext.Response.WriteAsJsonAsync(Response, ct);
}
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

## Troubleshooting

### Common Issues After Upgrading to FastEndpoints 7.x

#### Error: "The name 'SendAsync' does not exist in the current context"

**Cause**: FastEndpoints 7.x removed the `SendAsync()`, `SendNotFoundAsync()`, `SendNoContentAsync()`, and similar helper methods.

**Solution**: Use `HttpContext.Response` directly:

```csharp
// Old way (FastEndpoints 5.x/6.x)
await SendAsync(response, cancellation: ct);

// New way (FastEndpoints 7.x)
await HttpContext.Response.WriteAsJsonAsync(response, ct);
```

#### Error: "The name 'SendNotFoundAsync' does not exist in the current context"

**Cause**: Same as above - the helper method was removed.

**Solution**: Set the status code directly:

```csharp
// Old way
await SendNotFoundAsync(ct);

// New way
HttpContext.Response.StatusCode = 404;
return;
```

#### Error: "The name 'SendNoContentAsync' does not exist in the current context"

**Cause**: Same as above - the helper method was removed.

**Solution**: Set the status code to 204:

```csharp
// Old way
await SendNoContentAsync(ct);

// New way
HttpContext.Response.StatusCode = 204;
await Task.CompletedTask;
```

### .NET 10 Compatibility

This project targets **.NET 10** and has been tested with:
- FastEndpoints 7.1.1
- Bogus 35.6.5
- ASP.NET Core 10.0

If you encounter any issues, ensure you have:
1. .NET 10 SDK installed
2. All packages updated to their latest compatible versions
3. Reviewed the FastEndpoints 7.x migration guide

### Getting Help

If you encounter issues not covered here:
1. Check the [FastEndpoints Documentation](https://fast-endpoints.com/)
2. Review the [GitHub Issues](https://github.com/FastEndpoints/FastEndpoints/issues)
3. Examine the working code in this repository for reference implementations
