namespace FastEndpointApi.services;


/// <summary>
/// Represents a person service.
/// </summary>
public class PersonService : IPersonService
{
    private readonly List<PersonEntity> _people = [];

    /// <summary>
    /// Creates a new person.
    /// </summary>
    /// <param name="person">The person to create.</param>
    /// <returns>The created person.</returns>
    public PersonEntity CreatePerson(PersonEntity person)
    {
        _people.Add(person);
        return person;
    }

    /// <summary>
    /// Deletes a person by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the person to delete.</param>
    public bool DeletePerson(string? id)
    {
        if (id == null)
        {
            return false;
        }
        if (Guid.TryParse(id, out Guid guid))
        {
            var person = _people.FirstOrDefault(p => p.Id == guid);
            if (person != null)
            {
                _people.Remove(person);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Reads a person by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the person.</param>
    /// <returns>The person with the specified identifier, or null if not found.</returns>
    public PersonEntity ReadPerson(string id)
    {
        Guid guid = new(id);
        return _people.FirstOrDefault(p => p.Id == guid);
    }

    public List<PersonEntity> ReadPersons()
    {
        return [.. _people];
    }

    /// <summary>
    /// Updates a person by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the person to update.</param>
    /// <param name="updatedPerson">The updated person data.</param>
    /// <returns>The updated person, or null if not found.</returns>
    public PersonEntity? UpdatePerson(string id, PersonEntity updatedPerson)
    {
        if (id == null || updatedPerson == null)
        {
            return null;
        }
        Guid personId = Guid.Parse(id);
        var person = _people.FirstOrDefault(p => p.Id == personId);
        if (person != null)
        {
            _people.Remove(person);
            var newPerson = new PersonEntity
            {
                Id = personId,
                FirstName = updatedPerson.FirstName,
                LastName = updatedPerson.LastName,
                Age = updatedPerson.Age,
                Email = updatedPerson.Email
            };
            _people.Add(newPerson);
            return newPerson;
        }
        return null;
    }
}
