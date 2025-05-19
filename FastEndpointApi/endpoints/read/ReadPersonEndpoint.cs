using FastEndpointApi.services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Extensions;

namespace FastEndpointApi.endpoints.read;

/// <summary>
/// Endpoint for reading a person by ID.
/// </summary>
public class ReadPersonEndpoint(IPersonService personService) : Endpoint<ReadPersonRequest, PersonResponse>
{
    /// <inheritdoc/>
    public override void Configure()
    {
        Get("/person/{PersonId}"); // Using route parameter for PersonId.
        AllowAnonymous();
    }

    public override Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        var person = personService.ReadPerson(req.PersonId);

        if (person == null)
        {
            return SendNotFoundAsync(cancellation: ct);
        }
        var baseUrl = HttpContext.Request.GetDisplayUrl();

        Response = new PersonResponse
        {
            FullName = $"{person.FirstName} {person.LastName}",
            IsOver18 = person.Age > 18,
            PersonId = person.Id.ToString(),
            Links =
                [
                    new LinkResource { Rel = "self", Href = $"{baseUrl}/{person.Id}", Method = "GET" },
                    new LinkResource { Rel = "delete", Href = $"{baseUrl}/{person.Id}", Method = "DELETE" }
                ]
        };
        return SendAsync(Response, cancellation: ct);
    }
}
