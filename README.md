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
    - [Data Seeding](#data-seeding)
    - [Basic Configuration](#basic-configuration)
    - [Endpoint Implementation](#endpoint-implementation)
      - [Create Person](#create-person)
      - [Read Person by ID](#read-person-by-id)
      - [Read All Persons](#read-all-persons)
      - [Update Person](#update-person)
      - [Delete Person](#delete-person)
    - [Request and Response Models](#request-and-response-models)
    - [Mapping with FastEndpoints](#mapping-with-fastendpoints)
    - [Service Layer](#service-layer)
  - [Advanced Usage](#advanced-usage)
    - [HATEOAS Implementation](#hateoas-implementation)
    - [Error Handling](#error-handling)
  - [Static HTML Sample Pages](#static-html-sample-pages)
    - [index.html](#indexhtml)
    - [docs.html](#docshtml)
    - [test.html](#testhtml)
  - [Why Use FastEndpoints?](#why-use-fastendpoints)
  - [Contact \& Support](#contact--support)
  - [Additional Resources](#additional-resources)
  - [Glossary of Terms](#glossary-of-terms)
  - [Continuous Integration \& Deployment (CI/CD)](#continuous-integration--deployment-cicd)
  - [Visual Demo: Interactive Web UI](#visual-demo-interactive-web-ui)

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
├── services/                        # Business logic layer
│   ├── IPersonService.cs            # Service interface
│   ├── PersonEntity.cs              # Domain model
│   └── PersonService.cs             # Service implementation
├── wwwroot/                         # Static web assets
│   ├── index.html                   # Interactive web UI
│   ├── docs.html                    # API documentation sample page
│   └── test.html                    # Simple test page
└── ...
```

This structure promotes separation of concerns, with each endpoint having its own dedicated folder containing all related files. The `wwwroot` folder contains standalone HTML pages for demo and documentation purposes, making it easy to explore and test the API visually.

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

| Method | Endpoint         | Description            |
|--------|-----------------|------------------------|
| POST   | `/person`       | Create a new person    |
| GET    | `/person/{id}`  | Get a person by ID     |
| GET    | `/person`       | Get all persons        |
| PUT    | `/person/{id}`  | Update a person        |
| DELETE | `/person/{id}`  | Delete a person        |

When you run the application and navigate to the Swagger UI, you'll be able to test each of these endpoints interactively. This makes it easy to understand how the API works and see the HATEOAS links in action.

## Implementation Details

Let's dive into how this API is built using FastEndpoints. Understanding these patterns will help you quickly adapt the concepts to your own projects.

### Data Seeding

On startup, the application seeds 5 unique people using the [Bogus](https://github.com/bchavez/Bogus) library. This provides sample data for immediate testing and exploration.

### Basic Configuration

FastEndpoints is configured in the `Program.cs` file:

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

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
```

### Endpoint Implementation

#### Create Person

```csharp
public class CreatePersonEndpoint(IPersonService _personService) : Endpoint<CreatePersonRequest, PersonResponse, CreatePersonMapper>
{
    public override void Configure()
    {
        Post("/person");
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

#### Read Person by ID

```csharp
public class ReadPersonEndpoint(IPersonService personService) : Endpoint<ReadPersonRequest, PersonResponse>
{
    public override void Configure()
    {
        Get("/person/{id}");
        AllowAnonymous();
    }

    public override Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        var person = personService.ReadPerson(req.Id);
        if (person == null)
            return SendNotFoundAsync(cancellation: ct);
        var baseUrl = HttpContext.Request.GetDisplayUrl();
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
        return SendAsync(Response, cancellation: ct);
    }
}
```

#### Read All Persons

```csharp
public class ReadPersonsEndpoint(IPersonService personService) : EndpointWithoutRequest<List<PersonResponse>>
{
    public override void Configure()
    {
        Get("/person");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var baseUrl = HttpContext.Request.GetDisplayUrl();
        var personResponses = personService.ReadPersons()
            .Select(person => new PersonResponse
            {
                FullName = $"{person.FirstName} {person.LastName}",
                IsOver18 = person.Age > 18,
                PersonId = person.Id.ToString(),
                Links =
                [
                    new LinkResource { Rel = "self", Href = $"{baseUrl}/{person.Id}", Method = "GET" },
                    new LinkResource { Rel = "delete", Href = $"{baseUrl}/{person.Id}", Method = "DELETE" }
                ]
            })
            .ToList();
        await SendAsync(personResponses, cancellation: ct);
    }
}
```

#### Update Person

```csharp
public class UpdatePersonEndpoint(IPersonService personService) : Endpoint<UpdatePersonRequest, PersonResponse>
{
    public override void Configure()
    {
        Put("/person/{id}");
        AllowAnonymous();
    }

    public override Task HandleAsync(UpdatePersonRequest req, CancellationToken ct)
    {
        var person = personService.UpdatePerson(req.Id.ToString(), new PersonEntity
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Age = req.Age,
            Email = req.Email
        });
        if (person == null)
            return SendNotFoundAsync(cancellation: ct);
        Response = new PersonResponse
        {
            FullName = $"{person.FirstName} {person.LastName}",
            IsOver18 = person.Age > 18,
            PersonId = person.Id.ToString()
        };
        return SendAsync(Response, cancellation: ct);
    }
}
```

#### Delete Person

```csharp
public class DeletePersonEndpoint : EndpointWithoutRequest
{
    private readonly IPersonService _personService;
    public DeletePersonEndpoint(IPersonService personService)
    {
        _personService = personService;
    }
    public override void Configure()
    {
        Delete("/person/{id}");
        AllowAnonymous();
    }
    public override Task HandleAsync(CancellationToken ct)
    {
        var personId = Route<string>("Id");
        if (_personService.DeletePerson(personId))
            return SendNoContentAsync(cancellation: ct);
        else
            return SendNotFoundAsync(cancellation: ct);
    }
}
```

### Request and Response Models

```csharp
public class CreatePersonRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

public class UpdatePersonRequest : CreatePersonRequest
{
    public string Id { get; set; } = string.Empty;
}

public class ReadPersonRequest
{
    public string Id { get; set; } = string.Empty;
}

public class PersonResponse
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
    public string PersonId { get; internal set; }
    public List<LinkResource> Links { get; set; } = [];
}

public class LinkResource
{
    public string Rel { get; set; }
    public string Href { get; set; }
    public string Method { get; set; }
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
    private readonly List<PersonEntity> _people = new();
    public PersonService()
    {
        // Seed 5 unique people using Bogus
        var faker = new Faker<PersonEntity>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Age, f => f.Random.Int(18, 80))
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName));
        _people.AddRange(faker.Generate(5));
    }
    public PersonEntity CreatePerson(PersonEntity person)
    {
        _people.Add(person);
        return person;
    }
    public bool DeletePerson(string? id)
    {
        if (id == null) return false;
        if (Guid.TryParse(id, out Guid guid))
        {
            var person = _people.FirstOrDefault(p => p.Id == guid);
            if (person != null)
            {
                _people.Remove(person);
                return true;
            }
        }
        return false;
    }
    public PersonEntity? ReadPerson(string id)
    {
        if (Guid.TryParse(id, out Guid guid))
            return _people.FirstOrDefault(p => p.Id == guid);
        return null;
    }
    public List<PersonEntity> ReadPersons() => _people.ToList();
    public PersonEntity? UpdatePerson(string id, PersonEntity updatedPerson)
    {
        if (Guid.TryParse(id, out Guid guid))
        {
            var person = _people.FirstOrDefault(p => p.Id == guid);
            if (person != null)
            {
                var updated = new PersonEntity
                {
                    Id = person.Id,
                    FirstName = updatedPerson.FirstName,
                    LastName = updatedPerson.LastName,
                    Age = updatedPerson.Age,
                    Email = updatedPerson.Email
                };
                _people.Remove(person);
                _people.Add(updated);
                return updated;
            }
        }
        return null;
    }
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

## Static HTML Sample Pages

The `wwwroot` folder contains several standalone HTML pages that demonstrate and document the API. These are plain, easy-to-follow HTML files that require no build tools or frameworks—just open them in your browser.

### index.html

The main interactive web UI for the Person API. This page provides a user-friendly way to:

- Create a new person
- View all people in the system
- Get a person by ID
- Update a person's details
- Delete a person
- Quickly access Swagger UI and the related article

The UI is built with Bootstrap for a clean, responsive look and uses JavaScript to call the API endpoints directly. This makes it easy to test all CRUD operations and see HATEOAS links in action. The code is simple and well-commented, making it easy to follow and adapt.

### docs.html

A sample documentation page that demonstrates how to present API documentation or usage instructions in a clean, readable format. This page is a plain HTML file and can be used as a template for your own API documentation. It is easy to modify and extend, and serves as a quick reference for API consumers.

### test.html

A minimal test page for quickly verifying API connectivity or experimenting with JavaScript fetch calls. This file is intentionally simple, making it a great starting point for learning how to interact with the API using plain JavaScript. You can use it to test endpoints or as a base for your own experiments.

All these pages are self-contained and require no dependencies—just open them in your browser and start exploring.

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

## Continuous Integration & Deployment (CI/CD)

This project uses GitHub Actions to automate building, testing, and deploying the ASP.NET Core application to Azure Web Apps. The workflow is defined in `.github/workflows/main_fastendpointsdemo.yml` and includes:

- **Automatic Build & Test**: On every push to the `main` branch, the workflow restores dependencies, builds the project, and runs tests.
- **Artifact Publishing**: The build output is published and uploaded as an artifact for deployment.
- **Azure Web App Deployment**: The application is deployed to Azure using the official `azure/webapps-deploy` action, with secure authentication via OIDC.
- **Post-Deployment Health Check**: After deployment, the workflow performs a health check on the live site to verify successful deployment.

This ensures that every change to the `main` branch is automatically validated and deployed, providing a robust DevOps pipeline for the demo application.

## Visual Demo: Interactive Web UI

A modern, interactive web UI is included with the demo and can be accessed at `/index.html` (served from `wwwroot/index.html`). This page provides a user-friendly way to explore and interact with the Person API without needing external tools.

**Features:**

- **Create Person:** Fill out a form to add a new person to the in-memory data store.
- **Get All People:** View a table of all people currently in the system.
- **Get Person by ID:** Retrieve and display a person's details and HATEOAS links by entering their ID.
- **Update Person:** Update an existing person's details using their ID.
- **Delete Person:** Remove a person from the system by ID.
- **Swagger & Article Links:** Quick access to the Swagger UI and the related article.

The UI is built with Bootstrap for a clean, responsive look and uses JavaScript to call the API endpoints directly. This makes it easy to test all CRUD operations and see HATEOAS links in action.

> **Tip:** The web UI is a great way to demo the API to others or to quickly verify functionality during development.
