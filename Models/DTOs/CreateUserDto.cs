using System.ComponentModel.DataAnnotations;

namespace UsersManagementApi.Models.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        
        [Required]
        public string Phone { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        public string TaxId { get; set; }
        
        public List<AddressDto> Addresses { get; set; }
    }
}