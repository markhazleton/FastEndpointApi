namespace FastEndpointApi.endpoints.delete
{
    /// <summary>
    /// DeletePersonRequest is a class that represents the request to delete a person.
    /// </summary>
    public class DeletePersonRequest
    {
        /// <summary>
        /// The person id.
        /// </summary>
        public string? PersonId { get; set; } // To identify which person to delete.
    }
}
