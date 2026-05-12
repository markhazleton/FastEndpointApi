namespace FastEndpointApi.endpoints;

/// <summary>
/// Represents a response for creating a person.
/// </summary>
public class PersonResponse
{
    /// <summary>
    /// Gets or sets the full name of the person.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the person is over 18 years old.
    /// </summary>
    public bool IsOver18 { get; set; }
    /// <summary>
    /// PersonId of the person as string.
    /// </summary>
    public string PersonId { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hypermedia links for the person resource.
    /// </summary>
    public List<LinkResource> Links { get; set; } = [];
}
