using FastEndpointApi.mappings.person;
using FastEndpointApi.services.person;
using FastEndpoints;

namespace FastEndpointApi.endpoints.person.create;

public class CreatePersonEndpoint(IPersonService personService) : PersonEndpointBase<CreatePersonRequest, PersonResponse>(personService)
{

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Post("/api/person/create");
        AllowAnonymous();
        Description(b => b.Accepts<CreatePersonRequest>());
    }


    /// <summary>
    /// Handles the create person request asynchronously.
    /// </summary>
    /// <param name="req">The create person request.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        var newPerson = personService.CreatePerson(new()
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Age = req.Age,
            Email = req.Email
        });
        Response = PersonMapper.ToPersonResponse(newPerson);
        await SendAsync(Response, cancellation: ct);
    }
}
