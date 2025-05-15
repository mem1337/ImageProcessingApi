using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageProcessingService.Context;
using ImageProcessingService.Misc;
using ImageProcessingService.Models.ImageModels;
using Microsoft.AspNetCore.Authorization;

namespace ImageProcessingService.Controllers
{
    [Authorize]
    [Route("[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITransform _transform;

        public ImageController(AppDbContext context, ITransform transform)
        {
            _context = context;
            _transform = transform;
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

        [HttpPost("{id}/transform")]
        public async Task<IActionResult> images(int id, [FromBody] Transformation transformation)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }
            
            if (transformation.Resize is { Width: > 0, Heigth: > 0 })
            {
                await _transform.Resize(image.ImageLocation, transformation.Resize.Width, transformation.Resize.Heigth);
            }

            if (transformation.Crop is { Width: > 0, Height: > 0 })
            {
                await _transform.Crop(image.ImageLocation, transformation.Resize.Width, transformation.Resize.Heigth);
            }
            
            if (transformation.Rotate.Number > 0)
            {
                await _transform.Rotate(image.ImageLocation, transformation.Rotate.Number);
            }

            if (transformation.Format.ImageFormat != 0)
            {
                await _transform.Format(image.ImageLocation, transformation.Format.ImageFormat);
            }
            
            if (transformation.Filter.Grayscale || transformation.Filter.Sepia)
            {
                await _transform.Filter(image.ImageLocation, transformation.Filter.Grayscale, transformation.Filter.Sepia);
            }
            
            return Ok();
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
