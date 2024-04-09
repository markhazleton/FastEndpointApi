using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.create;

/// <summary>
/// Provides mapping functionality for the Person class.
/// </summary>
public class CreatePersonMapper : Mapper<CreatePersonRequest, PersonResponse, PersonEntity>
{
    /// <summary>
    /// maps the request to the entity.
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public override PersonEntity ToEntity(CreatePersonRequest r) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = r.FirstName,
        LastName = r.LastName,
        Age = r.Age,
        Email = r.Email
    };

    /// <summary>
    /// maps the entity to the response.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public override PersonResponse FromEntity(PersonEntity e) => new()
    {
        FullName = $"{e.FirstName} {e.LastName}",
        IsOver18 = e.Age >= 18,
        PersonId = e.Id.ToString()
    };
}
