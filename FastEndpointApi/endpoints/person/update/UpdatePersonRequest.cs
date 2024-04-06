using FastEndpointApi.endpoints.person.create;

namespace FastEndpointApi.endpoints.person.update
{
    // Represents a request to update a person.
    public class UpdatePersonRequest : CreatePersonRequest
    {
        public Guid PersonId { get; set; } // To identify which person to update.
    }
}
