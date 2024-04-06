namespace FastEndpointApi.endpoints.person.read
{
    // Represents a request to read a person by ID.
    public class ReadPersonRequest
    {
        public Guid PersonId { get; set; } // Assumes each person has a unique identifier.
    }
}
