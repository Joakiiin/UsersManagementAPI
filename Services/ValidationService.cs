using System.Text.RegularExpressions;
using UsersManagementApi.Contracts;
namespace UsersManagementApi.Services
{
    public class ValidationService : IValidationService
    {
        private static readonly Regex TaxIdRegex =
            new Regex(@"^[A-ZÑ&]{4}[0-9]{6}[A-Z0-9]{3}$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex =
            new Regex(@"^(\d\s*){10}$", RegexOptions.Compiled);
        public bool IsValidTaxId(string taxId)
        {
            if (string.IsNullOrWhiteSpace(taxId))
                return false;

            return TaxIdRegex.IsMatch(taxId);
        }
        public bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            string digitsOnly = Regex.Replace(phone, @"\s+", "");
            return PhoneRegex.IsMatch(digitsOnly);
        }
    }
}