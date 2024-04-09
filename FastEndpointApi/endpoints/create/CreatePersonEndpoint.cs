using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.create;

/// <summary>
/// Represents the endpoint for creating a person.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CreatePersonEndpoint"/> class.
/// </remarks>
/// <param name="_personService">The person service.</param>
public class CreatePersonEndpoint(IPersonService _personService) : Endpoint<CreatePersonRequest, PersonResponse, CreatePersonMapper>
{

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Post("/person/create");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the create person request asynchronously.
    /// </summary>
    /// <param name="req">The create person request.</param>
    /// <param name="ct">The cancellation token.</param>
    public override Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        PersonEntity entity = Map.ToEntity(req);
        entity = _personService.CreatePerson(entity);
        Response = Map.FromEntity(entity);
        return SendAsync(Response, cancellation: ct);
    }
}
