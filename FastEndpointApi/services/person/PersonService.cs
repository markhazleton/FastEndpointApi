namespace FastEndpointApi.services.person
{

    /// <summary>
    /// Represents a person service.
    /// </summary>
    public class PersonService : IPersonService
    {
        private readonly List<PersonEntity> _people = new();

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to create.</param>
        /// <returns>The created person.</returns>
        public PersonEntity CreatePerson(PersonEntity person)
        {
            person.Id = Guid.NewGuid();
            _people.Add(person);
            return person;
        }

        /// <summary>
        /// Reads a person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person.</param>
        /// <returns>The person with the specified identifier, or null if not found.</returns>
        public PersonEntity ReadPerson(Guid id)
        {
            return _people.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Updates a person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person to update.</param>
        /// <param name="updatedPerson">The updated person data.</param>
        /// <returns>The updated person, or null if not found.</returns>
        public PersonEntity UpdatePerson(Guid id, PersonEntity updatedPerson)
        {
            var person = _people.FirstOrDefault(p => p.Id == id);
            if (person != null)
            {
                person.FirstName = updatedPerson.FirstName;
                person.LastName = updatedPerson.LastName;
                person.Age = updatedPerson.Age;
                person.Email = updatedPerson.Email;
            }
            return person;
        }

        /// <summary>
        /// Deletes a person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person to delete.</param>
        public void DeletePerson(Guid id)
        {
            var person = _people.FirstOrDefault(p => p.Id == id);
            if (person != null)
            {
                _people.Remove(person);
            }
        }

        public List<PersonEntity> ReadPersons()
        {
            return [.. _people];
        }
    }
}
