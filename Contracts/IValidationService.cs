namespace UsersManagementApi.Contracts
{
    public interface IValidationService
    {
        bool IsValidTaxId(string taxId);
        bool IsValidPhoneNumber(string phone);
    }
}