using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.read;

/// <summary>
/// Read persons endpoint.
/// </summary>
/// <param name="personService"></param>
public class ReadPersonsEndpoint(IPersonService personService) : Endpoint<ReadPersonRequest, List<PersonResponse>>
{
    /// <summary>
    /// configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/api/person/");
        AllowAnonymous();
    }

    /// <summary>
    /// handles the read person request asynchronously.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public override Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        List<PersonResponse> personResponses = new();
        var personList = personService.ReadPersons();
        foreach (var person in personList)
        {
            personResponses.Add(new PersonResponse()
            {
                FullName = $"{person.FirstName} {person.LastName}",
                IsOver18 = person.Age > 18,
                PersonId = person.Id.ToString()
            }
            );
        }
        return SendAsync(personResponses, cancellation: ct);
    }
}
