using System.ComponentModel.DataAnnotations;

namespace UsersManagementApi.Models.DTOs
{
    public class UpdateUserDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        
        [MinLength(2)]
        public string? Name { get; set; }
        
        public string? Phone { get; set; }
        
        [MinLength(6)]
        public string? Password { get; set; }
        
        public string? TaxId { get; set; }
        
        public List<AddressDto>? Addresses { get; set; }
    }
}