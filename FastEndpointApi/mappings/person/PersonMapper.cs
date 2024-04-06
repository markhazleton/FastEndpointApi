using FastEndpointApi.endpoints.person;
using FastEndpointApi.endpoints.person.create;
using FastEndpointApi.endpoints.person.update;
using FastEndpointApi.services.person;

namespace FastEndpointApi.mappings.person;

/// <summary>
/// Provides mapping functionality for the Person class.
/// </summary>
public static class PersonMapper
{
    /// <summary>
    /// Converts a CreatePersonRequest object to a PersonEntity object.
    /// </summary>
    /// <param name="request">The CreatePersonRequest object.</param>
    /// <returns>The converted PersonEntity object.</returns>
    public static PersonEntity ToEntity(CreatePersonRequest request)
    {
        return new PersonEntity
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            Email = request.Email
        };
    }

    /// <summary>
    /// Converts a PersonEntity object to a PersonResponse object.
    /// </summary>
    /// <param name="entity">The PersonEntity object.</param>
    /// <returns>The converted PersonResponse object.</returns>
    public static PersonResponse ToPersonResponse(PersonEntity entity)
    {
        return new PersonResponse
        {
            Id = entity.Id,
            FullName = $"{entity.FirstName} {entity.LastName}",
            IsOver18 = entity.Age > 18
        };
    }

    /// <summary>
    /// Updates a PersonEntity object from an UpdatePersonRequest object.
    /// </summary>
    /// <param name="request">The UpdatePersonRequest object.</param>
    /// <param name="entity">The PersonEntity object to update.</param>
    /// <returns>The updated PersonEntity object.</returns>
    public static PersonEntity UpdateEntityFromRequest(UpdatePersonRequest request, PersonEntity entity)
    {
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Age = request.Age;
        entity.Email = request.Email;
        return entity;
    }
}

