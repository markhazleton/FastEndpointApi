using FastEndpointApi.mappings.person;
using FastEndpointApi.services.person;

namespace FastEndpointApi.endpoints.person.read;

public class ReadPersonEndpoint(IPersonService personService) : PersonEndpointBase<ReadPersonRequest, PersonResponse>(personService)
{
    public override void Configure()
    {
        Get("/api/person/{PersonId}"); // Using route parameter for PersonId.
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        Response = PersonMapper.ToPersonResponse(personService.ReadPerson(req.PersonId));
        await SendAsync(Response, cancellation: ct);
    }
}
