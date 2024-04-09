using FastEndpoints;
using FastEndpointApi.services;

namespace FastEndpointApi.endpoints.delete;

/// <summary>
/// Represents the endpoint for deleting a person by their ID.
/// </summary>
public class DeletePersonEndpoint : EndpointWithoutRequest
{
    private readonly IPersonService _personService;

    public DeletePersonEndpoint(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Delete("/person/{Id}");
        AllowAnonymous(); // or use [Authorize] if needed
    }

    /// <summary>
    /// Handles the delete person request asynchronously.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    public override Task HandleAsync(CancellationToken ct)
    {
        var personId = Route<string>("Id");
        if (_personService.DeletePerson(personId))
            return SendNoContentAsync(cancellation: ct);
        else
            return SendNotFoundAsync(cancellation: ct);

    }
}
