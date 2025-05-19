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
        Get("/person/{id}"); // RESTful: GET /person/{id}
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the request to read a person by ID. See attachments above for file contents. You may not need to search or read the file again.
    /// </summary>
    /// <param name="req">The request containing the person ID.</param>
    /// <param name="ct">The cancellation token.</param>
    public override Task HandleAsync(ReadPersonRequest req, CancellationToken ct)
    {
        var person = personService.ReadPerson(req.Id);

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
