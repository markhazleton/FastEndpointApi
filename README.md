# FastEndpoints: A Developer's Guide

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Welcome to this hands-on exploration of [FastEndpoints](https://fast-endpoints.com/)! If you're looking to streamline your ASP.NET Core API development while maintaining high performance, you're in the right place.

This guide introduces you to the FastEndpoints framework through a practical Person Management demo application. By walking through this project, you'll discover how FastEndpoints can help you build clean, maintainable APIs with minimal boilerplate code.

**Author**: Mark Hazleton  
**Article**: [Taking FastEndpoints For a Test Drive](https://markhazleton.com/articles/taking-fastendpoints-for-a-test-drive.html)

## Table of Contents

- [FastEndpoints: A Developer's Guide](#fastendpoints-a-developers-guide)
  - [Table of Contents](#table-of-contents)
  - [What is FastEndpoints?](#what-is-fastendpoints)
  - [Key Features](#key-features)
  - [Project Structure](#project-structure)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Running the Project](#running-the-project)
  - [API Endpoints](#api-endpoints)
  - [Implementation Details](#implementation-details)
    - [Basic Configuration](#basic-configuration)
    - [Endpoint Implementation](#endpoint-implementation)
    - [Request and Response Models](#request-and-response-models)
    - [Mapping with FastEndpoints](#mapping-with-fastendpoints)
    - [Service Layer](#service-layer)
  - [Advanced Usage](#advanced-usage)
    - [HATEOAS Implementation](#hateoas-implementation)
    - [Error Handling](#error-handling)
  - [Why Use FastEndpoints?](#why-use-fastendpoints)
  - [Contact \& Support](#contact--support)
  - [Additional Resources](#additional-resources)
  - [Glossary of Terms](#glossary-of-terms)

## What is FastEndpoints?

FastEndpoints is a lightweight REST API framework for ASP.NET that implements the REPR (Request-Endpoint-Response) Pattern. It's designed to be a developer-friendly alternative to Minimal APIs and MVC Controllers, significantly reducing boilerplate code while maintaining high performance.

In this guide, we'll explore a simple Person Management API that demonstrates the power and simplicity of FastEndpoints. You'll see firsthand how this framework encourages clean separation of concerns and implements modern API practices like HATEOAS, making your code more maintainable and your APIs more user-friendly.

## Key Features

This demo project is designed to showcase FastEndpoints in action through a simple yet comprehensive person management API. Here's what you'll find:

- **CRUD Operations**: Complete set of endpoints for creating, reading, updating, and deleting Person entities
- **In-Memory Data Store**: A simple in-memory collection to demonstrate data persistence without database complexity
- **Person Service Layer**: A clean abstraction of business logic through service interfaces and implementations
- **Smart Data Mapping**: Built-in mapping capabilities for transforming between request models, domain entities, and response DTOs
- **Dependency Injection**: Practical examples of how DI is used in FastEndpoints to provide services where needed
- **Reusable Base Classes**: Demonstrations of how to create base endpoint classes to promote code reuse
- **HATEOAS Links**: Implementation of hypermedia links in API responses for improved client navigation
- **Interactive Documentation**: Integration with Swagger/OpenAPI for easy API exploration and testing

Each feature is implemented in a straightforward way, making it easy to understand and adapt to your own projects.

## Project Structure

The demo project follows a clean, organized structure that aligns with the REPR pattern. Let's look at how the files are organized:

```plaintext
FastEndpointApi/
├── Program.cs                       # Application entry point and configuration
├── endpoints/                       # All API endpoints
│   ├── LinkResource.cs              # HATEOAS link representation
│   ├── PersonResponse.cs            # Shared response DTO
│   ├── create/                      # Create person endpoint
│   │   ├── CreatePersonEndpoint.cs
│   │   ├── CreatePersonMapper.cs
│   │   └── CreatePersonRequest.cs
│   ├── read/                        # Read person endpoint(s)
│   │   ├── ReadPersonEndpoint.cs
│   │   ├── ReadPersonMapper.cs
│   │   ├── ReadPersonRequest.cs
│   │   └── ReadPersonsEndpoint.cs
│   ├── update/                      # Update person endpoint
│   │   ├── UpdatePersonEndpoint.cs
│   │   └── UpdatePersonRequest.cs
│   └── delete/                      # Delete person endpoint
│       ├── DeletePersonEndpoint.cs
│       └── DeletePersonRequest.cs
└── services/                        # Business logic layer
    ├── IPersonService.cs            # Service interface
    ├── PersonEntity.cs              # Domain model
    └── PersonService.cs             # Service implementation
```

This structure promotes separation of concerns, with each endpoint having its own dedicated folder containing all related files. This organization makes it easy to navigate and maintain the codebase as it grows.

## Getting Started

Ready to dive in? Let's get this demo project up and running on your machine!

### Prerequisites

Before you begin, make sure you have:

- **.NET 9.0 SDK** or later installed on your machine
- An IDE of your choice: **Visual Studio**, **VS Code**, or **Rider**

### Running the Project

Follow these simple steps to get the project running:

1. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/MarkHazleton/FastEndpointDemo.git
   cd FastEndpointDemo
   ```

2. Build and run the project:

   ```bash
   cd FastEndpointApi
   dotnet build
   dotnet run
   ```

3. Open your browser and navigate to `https://localhost:7000/swagger` to explore the API using the interactive Swagger UI.

Once the application is running, you can use Swagger to test all the endpoints without needing any additional tools. This makes it easy to understand how the API works and how the different parts interact.

## API Endpoints

Let's explore the available endpoints in our Person Management API:

| Method | Endpoint              | Description                   |
|--------|----------------------|-------------------------------|
| POST   | `/person/create`     | Create a new person          |
| GET    | `/person/{id}`       | Get a person by ID           |
| GET    | `/persons`           | Get all persons              |
| PUT    | `/person/{id}`       | Update a person              |
| DELETE | `/person/{id}`       | Delete a person              |

When you run the application and navigate to the Swagger UI, you'll be able to test each of these endpoints interactively. This makes it easy to understand how the API works and see the HATEOAS links in action.

## Implementation Details

Let's dive into how this API is built using FastEndpoints. Understanding these patterns will help you quickly adapt the concepts to your own projects.

### Basic Configuration

First, let's look at how FastEndpoints is configured in the `Program.cs` file - it's remarkably simple:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v1";
    };
});
builder.Services.AddSingleton<IPersonService, PersonService>();

// ...

app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
});
app.UseSwaggerGen();
app.UseSwaggerUi();
```

### Endpoint Implementation

One of the core concepts in FastEndpoints is the implementation of individual endpoints that follow the REPR pattern. Let's look at how the create person endpoint is implemented:

```csharp
// From FastEndpointApi/endpoints/create/CreatePersonEndpoint.cs
public class CreatePersonEndpoint(IPersonService _personService) 
    : Endpoint<CreatePersonRequest, PersonResponse, CreatePersonMapper>
{
    public override void Configure()
    {
        Post("/person/create");
        AllowAnonymous();
    }

    public override Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        PersonEntity entity = Map.ToEntity(req);
        entity = _personService.CreatePerson(entity);
        Response = Map.FromEntity(entity);
        return SendAsync(Response, cancellation: ct);
    }
}
```

### Request and Response Models

FastEndpoints encourages the use of dedicated request and response models for each endpoint. This approach promotes clean separation of concerns and type safety throughout your API.

Here's how a request model is defined to capture input data:

```csharp
// From FastEndpointApi/endpoints/create/CreatePersonRequest.cs
public class CreatePersonRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}
```

And here's the response model that provides a unified representation of a person with HATEOAS links:

```csharp
// From FastEndpointApi/endpoints/PersonResponse.cs
public class PersonResponse
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
    public string PersonId { get; internal set; }
    public List<LinkResource> Links { get; set; } = [];
}
```

### Mapping with FastEndpoints

One of the most powerful features of FastEndpoints is its built-in mapping capabilities. Instead of using a separate mapping library, FastEndpoints provides a simple way to define mappings between your request models, domain entities, and response models:

```csharp
// From FastEndpointApi/endpoints/create/CreatePersonMapper.cs
public class CreatePersonMapper : Mapper<CreatePersonRequest, PersonResponse, PersonEntity>
{
    public override PersonEntity ToEntity(CreatePersonRequest r) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = r.FirstName,
        LastName = r.LastName,
        Age = r.Age,
        Email = r.Email
    };

    public override PersonResponse FromEntity(PersonEntity e) => new()
    {
        FullName = $"{e.FirstName} {e.LastName}",
        IsOver18 = e.Age >= 18,
        PersonId = e.Id.ToString()
    };
}
```

### Service Layer

Following good design principles, this demo separates business logic into a service layer. The service layer manages person entities with a simple in-memory store:

```csharp
// From FastEndpointApi/services/PersonService.cs
public class PersonService : IPersonService
{
    private readonly List<PersonEntity> _people = [];

    public PersonEntity CreatePerson(PersonEntity person)
    {
        _people.Add(person);
        return person;
    }

    // Other methods for reading, updating, deleting persons...
}
```

## Advanced Usage

Once you've mastered the basics, FastEndpoints offers several advanced features that can enhance your API. Let's explore a couple of them implemented in this demo.

### HATEOAS Implementation

HATEOAS (Hypermedia as the Engine of Application State) improves API discoverability by including relevant links in responses. This demo shows how easy it is to implement with FastEndpoints:

```csharp
// From FastEndpointApi/endpoints/read/ReadPersonEndpoint.cs
Response = new PersonResponse
{
    FullName = $"{person.FirstName} {person.LastName}",
    IsOver18 = person.Age > 18,
    PersonId = person.Id.ToString(),
    Links =
        [
            new LinkResource { Rel = "self", Href = $"{baseUrl}/{person.Id}", Method = "GET" },
            new LinkResource { Rel = "delete", Href = $"{baseUrl}/{person.Id}", Method = "DELETE" }
        ]
};
```

By including these links, clients can dynamically discover available actions rather than having to know them in advance. This makes your API more self-documenting and flexible.

### Error Handling

Well-designed APIs need robust error handling. This demo project implements a global exception handler that provides consistent, client-friendly error responses:

```csharp
// From FastEndpointApi/Program.cs
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
```

This approach ensures that even unexpected exceptions are caught and returned in a consistent JSON format, improving the API's reliability and developer experience. FastEndpoints also provides additional error handling capabilities through its validation features, which aren't shown in this snippet but are documented in the FastEndpoints documentation.

## Why Use FastEndpoints?

If you're wondering whether FastEndpoints is right for your project, consider these advantages over traditional MVC Controllers and even Minimal APIs:

- **Clean Architecture**: Promotes the REPR pattern for organized, maintainable code
- **Performance**: Comparable to Minimal APIs and faster than MVC Controllers
- **Reduced Boilerplate**: Simplified endpoint creation with minimal setup code
- **Auto-Discovery**: Automatic registration of endpoints during application startup
- **Built-in Validation**: Seamless integration with FluentValidation
- **Security Support**: Easy implementation of authentication and authorization
- **Swagger Integration**: Simple API documentation with the FastEndPoints.Swagger package

By adopting FastEndpoints, you'll find that your API development becomes more streamlined, allowing you to focus on business logic rather than plumbing code.

## Contact & Support

Have questions or encountered an issue? Feel free to [create an issue](https://github.com/MarkHazleton/FastEndpointDemo/issues) on the GitHub repository.

## Additional Resources

Want to learn more? Check out these valuable resources:

- [Official FastEndPoints Documentation](https://fast-endpoints.com/)
- [FastEndPoints GitHub Repository](https://github.com/FastEndpoints/FastEndpoints)
- [NuGet Package](https://www.nuget.org/packages/FastEndpoints/)
- [Swagger Support](https://www.nuget.org/packages/FastEndpoints.Swagger/)

## Glossary of Terms

New to RESTful APIs or .NET development? Here's a handy glossary of terms used in this project:

- **REPR Pattern**: Request-Endpoint-Response Pattern - An architectural approach where each endpoint has its own request model, handler, and response model, promoting separation of concerns.

- **DTO**: Data Transfer Object - A simple object used to transfer data between processes or layers of an application.

- **REST**: Representational State Transfer - An architectural style for designing networked applications, emphasizing stateless operations and standard HTTP methods.

- **HATEOAS**: Hypermedia as the Engine of Application State - A REST constraint that provides hyperlinks in API responses, allowing clients to dynamically navigate the API.

- **Swagger/OpenAPI**: A specification and set of tools for documenting and exploring RESTful APIs.

- **Endpoint**: In the context of APIs, an endpoint is a specific URL where your service can be accessed by a client application.

- **Dependency Injection (DI)**: A technique where one object supplies the dependencies of another object, helping to achieve loose coupling between classes.

- **Mapper**: A component that transforms data from one object type to another, often used to convert between DTOs and domain entities.

- **Entity**: A class that represents a domain model and typically maps to a database table in ORM scenarios.

- **Minimal API**: A simplified approach in ASP.NET Core for building HTTP APIs with minimal dependencies and setup.

- **Fluent Validation**: A library for building validation rules using a fluent interface.

- **NuGet Package**: The package manager for .NET that simplifies the process of incorporating third-party libraries into a project.

- **ASP.NET Core**: A cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications.

This glossary should help junior developers better understand the concepts used throughout this project and in modern API development more broadly.
