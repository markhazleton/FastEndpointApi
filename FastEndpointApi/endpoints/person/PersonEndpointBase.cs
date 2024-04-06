using FastEndpointApi.services.person;
using FastEndpoints;

namespace FastEndpointApi.endpoints.person
{
    public abstract class PersonEndpointBase<TRequest, TResponse>(IPersonService personService) : Endpoint<TRequest, TResponse>
    {
    }
}
