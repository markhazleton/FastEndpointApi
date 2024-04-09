using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.update
{

    /// <summary>
    /// Endpoint for updating a person.
    /// </summary>
    /// <param name="personService"></param>
    public class UpdatePersonEndpoint(IPersonService personService) : Endpoint<UpdatePersonRequest, PersonResponse>
    {
        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        public override void Configure()
        {
            Put("/person/update/{PersonId}"); // Using route parameter for PersonId.
            AllowAnonymous();
        }

        /// <summary>
        /// handles the update person request asynchronously.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override Task HandleAsync(UpdatePersonRequest req, CancellationToken ct)
        {
            var person = personService.UpdatePerson(req.PersonId, new()
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Age = req.Age,
                Email = req.Email
            });
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
}
