using Business.Interfaces;
using Business.Models;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;
public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<ClientResult> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result.MapTo<ClientResult>();

    }

    public async Task<bool> AddClientAsync(AddClientForm form)
    {
        return true;
    }
}
