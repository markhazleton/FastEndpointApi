using FastEndpointApi.endpoints.create;

namespace FastEndpointApi.endpoints.update
{
    // Represents a request to update a person.
    public class UpdatePersonRequest : CreatePersonRequest
    {
        public Guid PersonId { get; set; } // To identify which person to update.
    }
}
