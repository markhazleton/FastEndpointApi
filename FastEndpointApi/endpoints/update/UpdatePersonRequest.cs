using FastEndpointApi.endpoints.create;

namespace FastEndpointApi.endpoints.update;

/// <summary>
/// Model to update a person.
/// </summary>
public class UpdatePersonRequest : CreatePersonRequest
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

    /// <summary>
    /// Gets or sets the ID of the person.
    /// </summary>
    public string PersonId { get; set; }
}
