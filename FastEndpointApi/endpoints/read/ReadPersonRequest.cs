namespace FastEndpointApi.endpoints.read;

/// <summary>
/// Request object for reading a person.
/// </summary>
public class ReadPersonRequest
{
    /// <summary>
    /// The person identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}
