using UsersManagementApi.Models;
using UsersManagementApi.Models.DTOs;

namespace UsersManagementApi.Contracts
{
    public interface IMapperService
    {
        User MapToEntity(CreateUserDto dto);
        void MapToEntity(UpdateUserDto dto, User user);
        UserResponseDto MapToResponseDto(User user);
        List<UserResponseDto> MapToResponseDto(List<User> users);
    }
}