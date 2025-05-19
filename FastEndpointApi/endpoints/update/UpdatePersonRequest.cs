using FastEndpointApi.endpoints.create;

namespace FastEndpointApi.endpoints.update;

/// <summary>
/// Model to update a person.
/// </summary>
public class UpdatePersonRequest : CreatePersonRequest
{
    /// <summary>
    /// Gets or sets the ID of the person to update.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    // Note: FirstName, LastName, and Age properties are inherited from CreatePersonRequest
}
