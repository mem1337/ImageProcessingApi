using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageProcessingService.Context;
using ImageProcessingService.Misc;
using ImageProcessingService.Models.ImageModels;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace ImageProcessingService.Controllers
{
    [Authorize]
    [Route("[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<IActionResult> images(int id)
        {
            var bearer = HttpContext.Request.Headers.Authorization.ToString();
            var token = new JwtSecurityToken(bearer[7..]);
            var userId = Convert.ToInt32(token.Claims.FirstOrDefault(c => c.Type == "user_id").Value);
            
            var image = await _context.Images.FindAsync(id);
            
            if (image == null || image.ImageUserId != userId)
            {
                return BadRequest();
            }

            var stream = new FileStream(image.ImageLocation, FileMode.Open, FileAccess.Read);
            return File(stream, "application/octet-stream", image.ImageLocation[38..]);;
        }

        // PUT: api/Image/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image image)
        {
            if (id != image.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<ActionResult<ImageResponse>> images(IFormFile imageFile)
        {
            var bearer = HttpContext.Request.Headers.Authorization.ToString();
            var token = new JwtSecurityToken(bearer[7..]);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "user_id").Value;

            var sizeInKiloBytes = 2048 * 1024;
            if (imageFile.Length > sizeInKiloBytes)
            {
                return BadRequest();
            }
            
            var saveFilePath = Path.Combine("/home/anon/RiderProjects/ImageStorage/", imageFile.FileName);
            await using (var stream = new FileStream(saveFilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            
            var image = new Image
            {
                ImageLocation = saveFilePath,
                ImageExtension = Path.GetExtension(saveFilePath),
                ImageUserId = Convert.ToInt32(userId)
            };
            
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            var imageResponse = new ImageResponse
            {
                ImageId = image.ImageId,
                ImageLocation = image.ImageLocation
            };
            
            return CreatedAtAction("GetImage", new { id = image.ImageId }, imageResponse);
        }

        // DELETE: api/Image/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
