namespace FastEndpointApi.services.person;

/// <summary>
/// Represents a person entity.
/// </summary>
public class PersonEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the person.
    /// </summary>
    public Guid Id { get; set; } // Unique identifier for the person.

    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    public string FirstName { get; set; } // First name of the person.

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    public string LastName { get; set; } // Last name of the person.

    /// <summary>
    /// Gets or sets the age of the person.
    /// </summary>
    public int Age { get; set; } // Age of the person.

    /// <summary>
    /// Gets or sets the email address of the person.
    /// </summary>
    public string Email { get; set; } // Email address of the person.
}
