using FastEndpointApi.services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Extensions;

namespace FastEndpointApi.endpoints.read;

/// <summary>
/// Read persons endpoint.
/// </summary>
public class ReadPersonsEndpoint(IPersonService personService) : EndpointWithoutRequest<List<PersonResponse>>
{

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/person/");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the read person request asynchronously.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var baseUrl = HttpContext.Request.GetDisplayUrl();

        var personResponses = personService.ReadPersons()
            .Select(person => new PersonResponse
            {
                FullName = $"{person.FirstName} {person.LastName}",
                IsOver18 = person.Age > 18,
                PersonId = person.Id.ToString(),
                Links = 
                [
                    new LinkResource { Rel = "self", Href = $"{baseUrl}/{person.Id}", Method = "GET" },
                    new LinkResource { Rel = "delete", Href = $"{baseUrl}/{person.Id}", Method = "DELETE" }
                ]
            })
            .ToList();

        await SendAsync(personResponses, cancellation: ct);
    }
}
