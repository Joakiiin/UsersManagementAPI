using UsersManagementApi.Models;
using UsersManagementApi.Models.DTOs;

namespace UsersManagementApi.Helpers
{
    public static class MapperResponse
    {
        public static UserResponseDto MapToResponseDto(User user)
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

        public static List<UserResponseDto> MapToResponseDto(List<User> users)
        {
            return users.Select(MapToResponseDto).ToList();
        }
    }
}