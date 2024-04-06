namespace FastEndpointApi.endpoints.person;


/// <summary>
/// Represents a response for creating a person.
/// </summary>
public class PersonResponse
{
    /// <summary>
    /// Gets or sets the full name of the person.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the person is over 18 years old.
    /// </summary>
    public bool IsOver18 { get; set; }
    public Guid Id { get; internal set; }
}
