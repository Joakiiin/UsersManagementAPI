using Microsoft.AspNetCore.Mvc;
using UsersManagementApi.Contracts;
using UsersManagementApi.Models;
using UsersManagementApi.Models.DTOs;

namespace UsersManagementApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapperService _mapperService;

        public UsersController(IUserService userService, IMapperService mapperService)
        {
            _userService = userService;
            _mapperService = mapperService;
        }

        [HttpGet]
        public IActionResult GetUsers(
            [FromQuery] string? filter,
            [FromQuery] string? sortedBy)
        {
            try
            {
                List<User> users = _userService.GetAllUsers(filter, sortedBy);
                List<UserResponseDto> response = _mapperService.MapToResponseDto(users);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetUser(Guid id)
        {
            try
            {
                User user = _userService.GetUserById(id);
                if (user == null)
                    return NotFound(new { error = "User not found" });

                UserResponseDto response = _mapperService.MapToResponseDto(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Mapeo limpio usando el servicio
                User user = _mapperService.MapToEntity(userDto);
                User createdUser = _userService.CreateUser(user);
                UserResponseDto response = _mapperService.MapToResponseDto(createdUser);
                
                return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPatch("{id:guid}")]
        public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto updateDto)
        {
            try
            {
                User existingUser = _userService.GetUserById(id);
                if (existingUser == null)
                    return NotFound(new { error = "User not found" });

                // Guardar password en texto plano si viene
                string? plainPassword = updateDto.Password;
                
                // Usar mapper para actualización
                User updatedUser = _userService.UpdateUser(id, user =>
                {
                    _mapperService.MapToEntity(updateDto, user);
                }, plainPassword);

                if (updatedUser == null)
                    return NotFound(new { error = "User not found" });

                UserResponseDto response = _mapperService.MapToResponseDto(updatedUser);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                User user = _userService.GetUserById(id);
                if (user == null)
                    return NotFound(new { error = "User not found" });

                bool deleted = _userService.DeleteUser(id);
                if (!deleted)
                    return StatusCode(500, new { error = "Failed to delete user" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}