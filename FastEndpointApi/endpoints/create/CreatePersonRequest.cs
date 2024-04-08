namespace FastEndpointApi.endpoints.create;

/// <summary>
/// Represents a request to create a person.
/// </summary>
public class CreatePersonRequest
{
    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the age of the person.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the email of the person.
    /// </summary>
    public string Email { get; set; }
}
