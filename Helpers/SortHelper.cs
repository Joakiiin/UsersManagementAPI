using UsersManagementApi.Models;

namespace UsersManagementApi.Helpers
{
    public static class SortHelper
    {
        public static List<User> SortUsers(List<User> users, string sortedBy)
        {
            if (string.IsNullOrWhiteSpace(sortedBy))
                return users;

            return sortedBy.Trim().ToLower() switch
            {
                "email" => users.OrderBy(u => u.Email).ToList(),
                "name" => users.OrderBy(u => u.Name).ToList(),
                "phone" => users.OrderBy(u => u.Phone).ToList(),
                "tax_id" or "taxid" => users.OrderBy(u => u.TaxId).ToList(),
                "created_at" or "createdat" => users.OrderBy(u => u.CreatedAt).ToList(),
                "id" => users.OrderBy(u => u.Id).ToList(),
                _ => users.OrderBy(u => u.Id).ToList() // default
            };
        }
    }
}