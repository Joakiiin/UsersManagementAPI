using System.ComponentModel.DataAnnotations;
namespace UsersManagementApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Phone { get; set; }
        
        [Required]
        public string Password { get; set; } 
        
        [Required]
        public string TaxId { get; set; }
        
        public string CreatedAt { get; set; }
        
        public List<Address> Addresses { get; set; }
        
        public User()
        {
            Addresses = new List<Address>();
        }
    }
}