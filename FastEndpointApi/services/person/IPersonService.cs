namespace FastEndpointApi.services.person
{
    public interface IPersonService
    {
        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to create.</param>
        /// <returns>The created person.</returns>
        PersonEntity CreatePerson(PersonEntity person);

        /// <summary>
        /// Deletes a person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person to delete.</param>
        void DeletePerson(Guid id);

        /// <summary>
        /// Reads a person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person.</param>
        /// <returns>The person with the specified identifier, or null if not found.</returns>
        PersonEntity ReadPerson(Guid id);
        List<PersonEntity> ReadPersons();

        /// <summary>
        /// Updates a person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person to update.</param>
        /// <param name="updatedPerson">The updated person data.</param>
        /// <returns>The updated person, or null if not found.</returns>
        PersonEntity UpdatePerson(Guid id, PersonEntity updatedPerson);
    }
}
