namespace UsersManagementApi.Models.DTOs
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string TaxId { get; set; }
        public string CreatedAt { get; set; }
        public List<Address> Addresses { get; set; }
    }
}