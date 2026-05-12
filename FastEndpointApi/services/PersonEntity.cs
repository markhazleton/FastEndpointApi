namespace FastEndpointApi.services;

/// <summary>
/// Represents a person entity.
/// </summary>
public record PersonEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the person.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the age of the person.
    /// </summary>
    public int Age { get; init; }

    /// <summary>
    /// Gets or sets the email address of the person.
    /// </summary>
    public string Email { get; init; } = string.Empty;
}
