using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageProcessingService.Context;
using ImageProcessingService.Misc;
using ImageProcessingService.Models;
using ImageProcessingService.Models.UserModels;

namespace ImageProcessingService.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> users()
        {
            var userDtos = await _context.Users
                .Select(u => new UserDto()
                {
                    UserId = u.UserId,
                    UserEmail = u.UserEmail
                }).ToListAsync();

            return userDtos;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserJwtResponse>> users(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userResponse = new UserJwtResponse
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserJwt = "placeholder"
            };
            
            return userResponse;
        }
        
        [HttpPost]
        public async Task<ActionResult<User>> register([FromBody] UserRegisterLogin userRegister)
        {
            var salt = await Hash.GenerateSalt();
            
            var user = new User
            {
                UserEmail = userRegister.UserEmail,
                UserHash = await Hash.GenerateHash(userRegister.UserPassword,salt),
                UserSalt = salt
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("Users", new { id = user.UserId }, user);
        }

        [HttpPost]
        public async Task<ActionResult<UserJwtResponse>> login([FromBody] UserRegisterLogin userLogin)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserEmail == userLogin.UserEmail);
            if (user?.UserHash != await Hash.GenerateHash(userLogin.UserPassword, user.UserSalt))
            {
                return NotFound();
            }

            var userResponse = new UserJwtResponse
            {
                UserId = user.UserId,
                UserEmail = user.UserEmail,
                UserJwt = "placeholder"
            };

            return userResponse;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
