namespace FastEndpointApi.endpoints.delete
{
    // Represents a request to delete a person by ID.
    public class DeletePersonRequest
    {
        public Guid PersonId { get; set; } // To identify which person to delete.
    }
}
