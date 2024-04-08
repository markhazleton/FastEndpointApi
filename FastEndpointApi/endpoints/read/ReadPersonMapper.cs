using FastEndpointApi.services;
using FastEndpoints;

namespace FastEndpointApi.endpoints.read
{

    /// <summary>
    /// Provides mapping functionality for the Person class.
    /// </summary>
    public class ReadPersonMapper : Mapper<ReadPersonRequest, PersonResponse, PersonEntity>
    {
        /// <summary>
        /// maps the request to the entity.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override PersonEntity ToEntity(ReadPersonRequest r) => new()
        {
            Id = Guid.Parse(r.PersonId)
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
}
