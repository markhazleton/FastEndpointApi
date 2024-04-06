using FastEndpoints;

namespace FastEndpointApi.endpoints.person.update
{

    // Endpoint for updating a person.
    public class UpdatePersonEndpoint : Endpoint<UpdatePersonRequest, PersonResponse>
    {
        public override void Configure()
        {
            Put("/api/person/update/{PersonId}"); // Using route parameter for PersonId.
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdatePersonRequest req, CancellationToken ct)
        {
            // Logic to update the person in the data source using req.
            // Simulated response:
            await SendAsync(new PersonResponse { /* Updated data */ }, cancellation: ct);
        }
    }
}
