using FastEndpointApi.endpoints.person.create;
using FastEndpointApi.mappings.person;
using FastEndpointApi.services.person;

namespace FastEndpointApi.endpoints.person.read;

public class ReadPersonsEndpoint(IPersonService personService) : PersonEndpointBase<CreatePersonRequest, List<PersonResponse>>(personService)
{
    public override void Configure()
    {
        Get("/api/user/"); // Using route parameter for PersonId.
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        List<PersonResponse> personResponses = new();
        var personList = personService.ReadPersons();
        foreach (var person in personList)
        {
            personResponses.Add(PersonMapper.ToPersonResponse(person));
        }
        await SendAsync(personResponses);
    }
}
