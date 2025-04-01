using System.Threading.Tasks;
using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using EasyHousingSolutionProjectAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this using directive

public class ImageController : ControllerBase
{
    private readonly IImageRepository imageRepository;
    private readonly ILogger<ImageController> _logger; // Add this field

    public ImageController(IImageRepository imageRepository, ILogger<ImageController> logger) // Modify constructor
    {
        this.imageRepository = imageRepository;
        _logger = logger; // Initialize the logger
    }

    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImage(IFormFileCollection files, int propertyId)
    {
        try
        {
            if (files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            var existingImages = await imageRepository.GetImagesByPropertyId(propertyId);

            if (existingImages.Count() + files.Count > 6)
            {
                return BadRequest("You can only upload a maximum of 6 images per property.");
            }

            foreach (var file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                    var image = new Image
                    {
                        PropertyId = propertyId,
                        ImageName = base64Image
                    };

                    var result = await imageRepository.AddImage(image);
                    if (!result)
                    {
                        _logger.LogError("Failed to add image to the repository.");
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add image to the repository.");
                    }
                }
            }

            return Ok("Images uploaded successfully!");
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error: {ex.Message}. Inner Exception: {ex.InnerException?.Message}";
            _logger.LogError(errorMessage);
            return BadRequest(errorMessage);
        }

    }
    [HttpDelete("DeleteImage/{imageId}")]
    public async Task<IActionResult> DeleteImage([FromRoute] int imageId) 
    { 
        bool ans = await imageRepository.DeleteImage(imageId);
        if (ans)
        {
            return Ok("Image Deleted Successfully");
        }
        return BadRequest("No Image Found");
    }

    [HttpGet("GetImagesByProperty/{propertyId}")]
    public async Task<IActionResult> GetImagesByProperty([FromRoute]int propertyId)
    {
        var data = await imageRepository.GetImagesByPropertyId(propertyId);

        if (data == null || !data.Any())
        {
            return BadRequest("No images exist for this property.");
        }

        return Ok(data);
    }



}
