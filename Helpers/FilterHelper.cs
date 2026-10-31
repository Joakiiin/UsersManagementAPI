using UsersManagementApi.Models;

namespace UsersManagementApi.Helpers
{
    public static class FilterHelper
    {
        public static List<User> FilterUsers(List<User> users, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return users;

            try
            {
                string[] parts = filter.Split('+');
                if (parts.Length != 3)
                    throw new ArgumentException("Invalid filter format");

                string field = parts[0].ToLower();
                string operation = parts[1].ToLower();
                string value = parts[2];

                return field switch
                {
                    "email" => ApplyFilter(users, u => u.Email, operation, value),
                    "name" => ApplyFilter(users, u => u.Name, operation, value),
                    "phone" => ApplyFilter(users, u => u.Phone, operation, value),
                    "tax_id" or "taxid" => ApplyFilter(users, u => u.TaxId, operation, value),
                    "created_at" or "createdat" => ApplyFilter(users, u => u.CreatedAt, operation, value),
                    "id" => ApplyFilter(users, u => u.Id.ToString(), operation, value),
                    _ => throw new ArgumentException($"Invalid field: {field}")
                };
            }
            catch
            {
                return new List<User>();
            }
        }

        private static List<User> ApplyFilter(
            List<User> users, 
            Func<User, string> selector, 
            string operation, 
            string value)
        {
            return operation.ToLower() switch
            {
                "co" => users.Where(u => selector(u).Contains(value, StringComparison.OrdinalIgnoreCase)).ToList(),
                "eq" => users.Where(u => selector(u).Equals(value, StringComparison.OrdinalIgnoreCase)).ToList(),
                "sw" => users.Where(u => selector(u).StartsWith(value, StringComparison.OrdinalIgnoreCase)).ToList(),
                "ew" => users.Where(u => selector(u).EndsWith(value, StringComparison.OrdinalIgnoreCase)).ToList(),
                _ => throw new ArgumentException($"Invalid operation: {operation}")
            };
        }
    }
}