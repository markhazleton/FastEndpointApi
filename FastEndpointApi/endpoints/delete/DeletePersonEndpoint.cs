using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.delete;

/// <summary>
/// Endpoint for deleting a person by ID.
/// </summary>
public class DeletePersonEndpoint(IPersonService personService) : EndpointWithoutRequest
{

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Delete("/person/{id}");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the delete person request asynchronously.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    public override Task HandleAsync(CancellationToken ct)
    {
        var personId = Route<string>("Id");
        if (personService.DeletePerson(personId))
            return SendNoContentAsync(cancellation: ct);
        else
            return SendNotFoundAsync(cancellation: ct);

    }
}
