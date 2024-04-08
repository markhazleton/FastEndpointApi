using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.delete;


/// <summary>
/// Endpoint for deleting a person.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeletePersonEndpoint"/> class.
/// </remarks>
/// <param name="personService">The person service.</param>
public class DeletePersonEndpoint(IPersonService personService) : Endpoint<string>
{

    /// <inheritdoc/>
    public override void Configure()
    {
        Delete("/api/person/delete/{PersonId}");
        AllowAnonymous();
    }

    /// <inheritdoc/>
    public override async Task HandleAsync(string PersonId, CancellationToken ct)
    {
        personService.DeletePerson(PersonId);
        await SendOkAsync(ct);
    }
}
