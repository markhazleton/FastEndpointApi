# FastEndpointDemo
This is a demo project for FastEndpoints library of NuGet packages 

https://fast-endpoints.com/

# FastEndpoints Person Management Demo
This repository contains a demo application showcasing the use of FastEndpoints in ASP.NET for managing Person entities. It demonstrates the creation of a service-oriented architecture, employing best practices for building scalable and maintainable web APIs.

## Features
- **CRUD Operations**: 
Endpoints for creating, reading, updating, and deleting Person entities.
- **In-Memory Data Store**: 
Utilizes an in-memory list to manage PersonEntity instances.
- **Person Service**: 
A service class that abstracts the logic for managing person entities.
- **Data Mapping**: 
Includes a PersonMapper class for efficient mapping between entities and DTOs.
- **Dependency Injection**:
Demonstrates the use of dependency injection to provide services to endpoints.
- **Endpoint Base Class**: 
A base class for endpoints that require PersonService, promoting code reuse.

## Getting Started
To run this demo, clone the repository and open the solution in Visual Studio. Run the application and use a tool like Postman or Swagger to interact with the available endpoints.

Feel free to explore the code to understand how FastEndpoints can be used to create efficient and organized web APIs.
