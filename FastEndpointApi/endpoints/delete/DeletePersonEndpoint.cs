using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.delete;

/// <summary>
/// Endpoint for deleting a person by ID.
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
        Delete("/person/{id}"); // RESTful: DELETE /person/{id}
        AllowAnonymous();
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
