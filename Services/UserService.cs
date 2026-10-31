using UsersManagementApi.Contracts;
using UsersManagementApi.Helpers;
using UsersManagementApi.Models;

namespace UsersManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();
        private int _nextAddressId = 3;
        private readonly IEncryptionService _encryptionService;
        private readonly IValidationService _validationService;

        public UserService(IEncryptionService encryptionService, IValidationService validationService)
        {
            _encryptionService = encryptionService;
            _validationService = validationService;
            InitializeSampleUsers();
        }

        private void InitializeSampleUsers()
        {
            List<(string email, string name, string phone, string password, string taxId)> sampleUsers = new List<(string email, string name, string phone, string password, string taxId)>
            {
                ("galindotrejojoaquin@mail.com", "Joaquin Galindo Trejo", "1 55 555 555 55", "Password123", "AARR990101XXX"),
                ("cajadeapagadores@mail.com", "Miguel Galindo Padierna", "2 55 556 556 56", "Password456", "BBRR990101XXX"),
                ("trejoanaisabel@mail.com", "Ana Isabel Trejo Hernandez", "3 55 557 557 57", "Password789", "CCRR990101XXX")
            };

            int addressCounter = 1;
            string[] countryCodes = { "UK", "AU", "AG", "AM", "MX", "NZ" };

            foreach ((string email, string name, string phone, string password, string taxId) in sampleUsers)
            {
                string encryptedPassword = _encryptionService.Encrypt(password);

                _users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Name = name,
                    Phone = phone,
                    Password = encryptedPassword,
                    TaxId = taxId,
                    CreatedAt = GetMadagascarTime().ToString("dd-MM-yyyy HH:mm"),
                    Addresses = new List<Address>
                    {
                        new() { Id = addressCounter++, Name = "workaddress",
                                Street = $"street No. {addressCounter}",
                                CountryCode = countryCodes[(addressCounter-2) % countryCodes.Length] },
                        new() { Id = addressCounter++, Name = "homeaddress",
                                Street = $"street No. {addressCounter}",
                                CountryCode = countryCodes[(addressCounter-1) % countryCodes.Length] }
                    }
                });
            }
            _nextAddressId = addressCounter;
        }

        public List<User> GetAllUsers(string? filter = null, string? sortedBy = null)
        {
            List<User> users = _users.ToList();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                users = FilterHelper.FilterUsers(users, filter);
            }
            if (!string.IsNullOrWhiteSpace(sortedBy))
            {
                users = SortHelper.SortUsers(users, sortedBy);
            }
            return users;
        }

        public User? GetUserById(Guid id)
        {
            return _users.Find(u => u.Id == id);
        }

        public User CreateUser(User user)
        {
            if (_users.Any(u => u.TaxId.Equals(user.TaxId, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Tax ID {user.TaxId} already exists");
            }

            if (!_validationService.IsValidTaxId(user.TaxId))
            {
                throw new ArgumentException($"Invalid Tax ID format. Expected format: AARR990101XXX");
            }

            if (!_validationService.IsValidPhoneNumber(user.Phone))
            {
                throw new ArgumentException($"Invalid phone format. Expected 10 digits in AndresFormat");
            }

            user.Password = _encryptionService.Encrypt(user.Password);

            user.Id = Guid.NewGuid();
            user.CreatedAt = GetMadagascarTime().ToString("dd-MM-yyyy HH:mm");

            if (user.Addresses != null)
            {
                foreach (Address address in user.Addresses)
                {
                    address.Id = _nextAddressId++;
                }
            }

            _users.Add(user);
            return user;
        }

        public User? UpdateUser(Guid id, Action<User> updateAction, string? plainPassword = null)
        {
            User user = GetUserById(id);
            if (user == null) return null;

            string originalTaxId = user.TaxId;
            string originalPhone = user.Phone;
            updateAction(user);
            if (!string.IsNullOrWhiteSpace(user.TaxId) &&
                !user.TaxId.Equals(originalTaxId, StringComparison.OrdinalIgnoreCase))
            {
                if (_users.Any(u =>
                    u.Id != id &&
                    u.TaxId.Equals(user.TaxId, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Tax ID {user.TaxId} already exists");
                }
                if (!_validationService.IsValidTaxId(user.TaxId))
                {
                    throw new ArgumentException($"Invalid Tax ID format. Expected format: AARR990101XXX");
                }
            }

            if (!string.IsNullOrWhiteSpace(user.Phone) &&
                !user.Phone.Equals(originalPhone, StringComparison.OrdinalIgnoreCase))
            {
                if (!_validationService.IsValidPhoneNumber(user.Phone))
                {
                    throw new ArgumentException($"Invalid phone format. Expected 10 digits in AndresFormat");
                }
            }
            if (!string.IsNullOrWhiteSpace(plainPassword))
            {
                user.Password = _encryptionService.Encrypt(plainPassword);
            }

            return user;
        }

        public bool DeleteUser(Guid id)
        {
            User user = GetUserById(id);
            if (user == null) return false;

            return _users.Remove(user);
        }

        private static DateTime GetMadagascarTime()
        {
            return DateTime.UtcNow.AddHours(3);
        }
    }
}