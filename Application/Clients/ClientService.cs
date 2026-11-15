using Application.Auth;
using Application.Clients.DTOs;
using Application.DTOs;
using Application.Repositories;
using Domain.Entities;

namespace Application.Clients
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        public ClientService(IClientRepository clientRepository, IUserRepository userRepository, ICurrentUser currentUser)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task CreateClient(SaveClientInput input)
        {
            await ValidateClient(input);

            var client = new Client
            {
                Name = input.Name,
                Description = input.Description,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber
            };

            await _clientRepository.Insert(client);
            await _clientRepository.SaveChanges();
        }

        public async Task UpdateClient(int id, SaveClientInput input)
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
            {
                throw new Exception("Client not found");
            }

            await ValidateClient(input, id);

            client.Name = input.Name;
            client.Description = input.Description;
            client.Email = input.Email;
            client.PhoneNumber = input.PhoneNumber;

            _clientRepository.Update(client);
            await _clientRepository.SaveChanges();
        }

        public async Task<PaginationOutput<GetClientOutput>> GetClients(ClientFilterInput input)
        {
            var clients = await _clientRepository.GetClientPagination(input);

            var result = new PaginationOutput<GetClientOutput>
            {
                Items = clients.Items.Select(MapToOutput).ToList(),
                TotalCount = clients.TotalCount
            };

            return result;
        }

        public async Task<PaginationOutput<GetClientOutput>> GetMyClients(ClientFilterInput input)
        {
            input.AssignedEmployeeId = _currentUser.UserId;
            return await GetClients(input);
        }

        public async Task<GetClientOutput> GetClientById(int id)
        {
            var client = await _clientRepository.GetByIdWithEmployee(id);

            if (client == null)
            {
                throw new Exception("Client not found");
            }

            return MapToOutput(client);
        }

        public async Task DeleteClient(int id)
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
            {
                throw new Exception("Client not found");
            }

            _clientRepository.Delete(client);
            await _clientRepository.SaveChanges();
        }

        public async Task AssignEmployee(int id, AssignEmployeeInput input)
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
            {
                throw new Exception("Client not found");
            }

            if (input.EmployeeId.HasValue)
            {
                var isEmployeeExists = await _userRepository
                    .Any(u => u.Id == input.EmployeeId.Value && u.Role.Name == RoleNames.Employee);
                if (!isEmployeeExists)
                {
                    throw new Exception("Employee not found");
                }
            }

            client.AssignedEmployeeId = input.EmployeeId;

            _clientRepository.Update(client);
            await _clientRepository.SaveChanges();
        }

        private async Task ValidateClient(SaveClientInput input, int? id = null)
        {
            var isPhoneNumberExists = await _clientRepository
                .Any(c => c.PhoneNumber.ToLower().Trim() == input.PhoneNumber.ToLower().Trim() && c.Id != id);
            if (isPhoneNumberExists)
            {
                throw new Exception("Phone number already exists");
            }

            if (!string.IsNullOrEmpty(input.Email))
            {
                var isEmailExists = await _clientRepository
                    .Any(c => c.Email.ToLower().Trim() == input.Email.ToLower().Trim() && c.Id != id);
                if (isEmailExists)
                {
                    throw new Exception("Email already exists");
                }
            }
        }

        private static GetClientOutput MapToOutput(Client c)
        {
            return new GetClientOutput
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                AssignedEmployeeId = c.AssignedEmployeeId,
                AssignedEmployeeName = c.AssignedEmployee?.Name
            };
        }
    }
}
