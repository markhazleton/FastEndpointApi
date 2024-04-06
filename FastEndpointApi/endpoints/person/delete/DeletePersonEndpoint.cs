using FastEndpoints;

namespace FastEndpointApi.endpoints.person.delete
{

    // Endpoint for deleting a person.
    public class DeletePersonEndpoint : Endpoint<DeletePersonRequest>
    {
        public override void Configure()
        {
            Delete("/api/person/delete/{PersonId}"); // Using route parameter for PersonId.
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeletePersonRequest req, CancellationToken ct)
        {
            // Logic to delete the person from the data source using req.PersonId.
            // Acknowledge the deletion:
            await SendOkAsync(ct);
        }
    }
}
