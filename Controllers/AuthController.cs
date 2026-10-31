using Microsoft.AspNetCore.Mvc;
using UsersManagementApi.Contracts;
using UsersManagementApi.Helpers;
using UsersManagementApi.Models;
using UsersManagementApi.Models.DTOs;
namespace UsersManagementApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapperService _mapperService;
        public AuthController(IUserService userService, IEncryptionService encryptionService, IMapperService mapperService)
        {
            _userService = userService;
            _encryptionService = encryptionService;
            _mapperService = mapperService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                if (loginRequest == null || 
                    string.IsNullOrWhiteSpace(loginRequest.TaxId) || 
                    string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    return BadRequest(new { error = "Tax ID and password are required" });
                }
                List<User> allUsers = _userService.GetAllUsers();
                User user = allUsers.FirstOrDefault(u => 
                    u.TaxId.Equals(loginRequest.TaxId, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    return Unauthorized(new { error = "Invalid credentials" });
                }
                string decryptedPassword = _encryptionService.Decrypt(user.Password);
                
                if (decryptedPassword != loginRequest.Password)
                {
                    return Unauthorized(new { error = "Invalid credentials" });
                }
                UserResponseDto userResponse = _mapperService.MapToResponseDto(user);
                
                return Ok(new 
                { 
                    message = "Login successful",
                    user = userResponse 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}