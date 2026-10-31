using UsersManagementApi.Contracts;
using UsersManagementApi.Models;
using UsersManagementApi.Models.DTOs;

namespace UsersManagementApi.Services
{
    public class MapperService : IMapperService
    {
        private readonly IEncryptionService _encryptionService;

        public MapperService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public User MapToEntity(CreateUserDto dto)
        {
            return new User
            {
                Email = dto.Email,
                Name = dto.Name,
                Phone = dto.Phone,
                Password = dto.Password, // Se encriptará después en el UserService
                TaxId = dto.TaxId,
                Addresses = dto.Addresses?.Select(MapToAddressEntity).ToList() ?? new List<Address>()
            };
        }

        public void MapToEntity(UpdateUserDto dto, User user)
        {
            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                user.Phone = dto.Phone;

            // NOTA: El password debe manejarse diferente en PATCH
            // Si viene, se asume que ya está encriptado o se encriptará después
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // Aquí podríamos encriptar si es necesario
                // Pero mejor dejamos que UserService decida
                user.Password = dto.Password;
            }

            if (!string.IsNullOrWhiteSpace(dto.TaxId))
                user.TaxId = dto.TaxId;

            if (dto.Addresses != null)
            {
                user.Addresses.Clear();
                foreach (AddressDto addressDto in dto.Addresses)
                {
                    user.Addresses.Add(MapToAddressEntity(addressDto));
                }
            }
        }

        public UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                TaxId = user.TaxId,
                CreatedAt = user.CreatedAt,
                Addresses = user.Addresses
            };
        }

        public List<UserResponseDto> MapToResponseDto(List<User> users)
        {
            return users.Select(MapToResponseDto).ToList();
        }

        private Address MapToAddressEntity(AddressDto dto)
        {
            return new Address
            {
                Id = dto.Id,
                Name = dto.Name,
                Street = dto.Street,
                CountryCode = dto.CountryCode
            };
        }
    }
}