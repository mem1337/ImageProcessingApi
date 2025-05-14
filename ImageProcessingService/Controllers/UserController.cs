using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageProcessingService.Context;
using ImageProcessingService.Misc;
using ImageProcessingService.Models;
using ImageProcessingService.Models.UserModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ImageProcessingService.Controllers;

[Route("[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJWT _jwt;
    private readonly IHash _hash;

    public UserController(AppDbContext context, IJWT jwt, IHash hash)
    {
        _context = context;
        _jwt = jwt;
        _hash = hash;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> users()
    {
        var userDtos = await _context.Users
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                UserEmail = u.UserEmail
            }).ToListAsync();

        return userDtos;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> users(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            UserId = user.UserId,
            UserEmail = user.UserEmail
        };
            
        return userDto;
    }
        
    [HttpPost]
    public async Task<ActionResult<User>> register([FromBody] UserRegisterLogin userRegister)
    {
        var salt = await _hash.GenerateSalt();
            
        var user = new User
        {
            UserEmail = userRegister.UserEmail,
            UserHash = await _hash.GenerateHash(userRegister.UserPassword,salt),
            UserSalt = salt
        };
            
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            UserId = user.UserId,
            UserEmail = user.UserEmail
        };
            
        return CreatedAtAction("Users", new { id = user.UserId }, userDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> login([FromBody] UserRegisterLogin userLogin)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserEmail == userLogin.UserEmail);
        if (user == null || user.UserHash != await _hash.GenerateHash(userLogin.UserPassword, user.UserSalt))
        {
            return NotFound();
        }

        var userResponse = new UserResponse
        {
            AuthResult = true,
            AuthToken = await _jwt.GenerateToken(user.UserId),
            ExpireDate = DateTime.UtcNow.AddMinutes(5)
        };
            
        return userResponse;
    }

    [Authorize]
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