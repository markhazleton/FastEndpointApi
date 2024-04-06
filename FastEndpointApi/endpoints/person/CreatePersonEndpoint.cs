using FastEndpoints;

namespace FastEndpointApi.endpoints.person;

/// <summary>
/// Represents an endpoint for creating a person.
/// </summary>
public class CreatePersonEndpoint : Endpoint<CreatePersonRequest, CreatePersonResponse>
{
    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Post("/api/user/create");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the create person request asynchronously.
    /// </summary>
    /// <param name="req">The create person request.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        await SendAsync(new()
        {
            FullName = req.FirstName + " " + req.LastName,
            IsOver18 = req.Age > 18
        }, cancellation: ct);
    }
}
