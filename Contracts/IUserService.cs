using UsersManagementApi.Models;

namespace UsersManagementApi.Contracts
{
    public interface IUserService
    {
        List<User> GetAllUsers(string? filter = null, string? sortedBy = null);
        User? GetUserById(Guid id);
        User CreateUser(User user);
        User? UpdateUser(Guid id, Action<User> updateAction, string? plainPassword = null);
        bool DeleteUser(Guid id);
    }
}