using System.ComponentModel.DataAnnotations;

namespace UsersManagementApi.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string TaxId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}