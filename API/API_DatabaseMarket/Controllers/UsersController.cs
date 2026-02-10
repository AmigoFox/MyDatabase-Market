using API_DatabaseMarket.DTOs.Users;
using API_DatabaseMarket.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_DatabaseMarket.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized();

            if (!string.IsNullOrWhiteSpace(request.FullName) &&
                request.FullName != "string")
            {
                user.FullName = request.FullName;
            }

            if (!string.IsNullOrWhiteSpace(request.Phone) &&
                request.Phone != "string")
            {
                user.Phone = request.Phone;
            }

            if (!string.IsNullOrWhiteSpace(request.Email) &&
                request.Email != "string")
            {
                user.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.NewPassword) &&
                request.NewPassword != "string")
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
