using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.read;

public class ReadPersonEndpoint(IPersonService personService) : Endpoint<ReadPersonRequest, PersonResponse>
{
    public override void Configure()
    {
        Get("/api/person/{PersonId}"); // Using route parameter for PersonId.
        AllowAnonymous();
    }

    public override Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        var person = personService.ReadPerson(req.PersonId);

        if (person == null)
        {
            return SendNotFoundAsync(cancellation: ct);
        }

        Response = new PersonResponse
        {
            FullName = $"{person.FirstName} {person.LastName}",
            IsOver18 = person.Age > 18,
            PersonId = person.Id.ToString()
        };
        return SendAsync(Response, cancellation: ct);
    }
}
