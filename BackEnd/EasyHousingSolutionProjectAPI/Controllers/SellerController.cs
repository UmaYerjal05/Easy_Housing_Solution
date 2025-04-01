using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using EasyHousingSolutionProjectAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyHousingSolutionProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SellerController : ControllerBase
    {
        private readonly ISellerRepository sellerRepository;
        private readonly IStateRepository stateRepository;
        private readonly ICityRepository cityRepository;

        public SellerController(ISellerRepository sellerRepository, IStateRepository stateRepository, ICityRepository cityRepository)
        {
            this.sellerRepository = sellerRepository;
            this.stateRepository = stateRepository;
            this.cityRepository = cityRepository;
        }

        [HttpGet("GetSellerIdByUsername/{username}")]
        public async Task<IActionResult> GetSellerIdByUsername(string username)
        {
            var seller = await sellerRepository.GetSellerByName(username);
            if (seller == null)
            {
                return NotFound("Seller not found.");
            }
            return Ok(seller.SellerId);
        }

        //[Authorize(Roles = "Seller")]
        [HttpGet("GetImagesByPropertyId/{propertyId}")]
        public async Task<IActionResult> GetImagesByPropertyId(int propertyId)
        {
            var data = await sellerRepository.GetImagesByPropertyId(propertyId);
            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        [HttpGet("GetCitiesByStateId/{stateId}")]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await cityRepository.GetCities(stateId);
            if (cities != null && cities.Any())
            {
                return Ok(cities);
            }
            return BadRequest("No cities found.");
        }


        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var states = await stateRepository.GetStates();
            if (states != null && states.Any())
            {
                return Ok(states);
            }
            return BadRequest("No states found.");
        }
        //[Authorize(Roles = "Seller")]
        [HttpPost("add-property")]
        public async Task<IActionResult> AddProperty([FromBody] Property property)
        {
            if (property == null)
            {
                return BadRequest("Property data is required.");
            }

            var isStatus = await sellerRepository.AddProperty(property);
            if (isStatus)
            {
                return Ok(new {message= "Property Added Successfully" });
            }
            return BadRequest();
        }

        [HttpGet("GetPropertiesBySellerId/{sellerId}")]
        public async Task<IActionResult> GetPropertiesBySellerId(int sellerId)
        {
            var data = await sellerRepository.GetPropertiesBySellerId(sellerId);
            if(data != null)
            {
                return Ok(data);
                
            }
            return BadRequest("No Property exist please add the properties.");

        }

        [HttpGet("GetPropertiesBySellerName/{sellerName}")]
        public async Task<IActionResult> GetPropertiesBySellerName(string sellerName)
        {
            try
            {
                if (string.IsNullOrEmpty(sellerName))
                {
                    return BadRequest("Seller name cannot be empty.");
                }

                var seller = await sellerRepository.GetSellerByName(sellerName);
                if (seller == null)
                {
                    return NotFound($"No seller found with the name '{sellerName}'.");
                }

                var data = await sellerRepository.GetPropertiesBySellerId(seller.SellerId);
                if (data == null || !data.Any())
                {
                    return NotFound($"No properties found for seller '{sellerName}'.");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        //[Authorize(Roles = "Seller")]
        [HttpGet("GetAllProperties")]
        public async Task<IActionResult> GetAllProperties()
        {
            var data = await sellerRepository.GetAllProperties();
            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }


        //[Authorize(Roles = "Seller")]
        [HttpPut("edit-property")]
        public async Task<IActionResult> EditProperty(int propertyId,[FromBody] Property property)
        {
            if (property == null || propertyId == 0)
            {
                return BadRequest("Property data is invalid.");
            }
            if (await sellerRepository.EditProperty(propertyId,property))
            {
                return Ok(new {Message="Property edited successfully." });
            }
            else
            {
                return StatusCode(500, "Property can't be edited");
            }
        }

        //[Authorize(Roles = "Seller")]
        [HttpGet("verified-properties/{sellerId}")]
        public async Task<IActionResult> GetVerifiedPropertiesByOwner(int sellerId)
        {
            var properties = await sellerRepository.GetVerifiedPropertiesByOwner(sellerId);
            if (properties != null && properties.Any())
            {
                return Ok(properties);
            }
            return BadRequest("No verified properties found.");
        }

        //[Authorize(Roles = "Seller")]
        [HttpGet("deactivated-properties/{sellerId}")]
        public async Task<IActionResult> GetDeactivatedPropertiesByOwner(int sellerId)
        {
            var properties = await sellerRepository.GetDeactivatedPropertiesByOwner(sellerId);
            if (properties != null && properties.Any())
            {
                return Ok(properties);
            }
            return BadRequest("No deactivated properties found.");
        }

        //[Authorize(Roles = "Seller")]
        [HttpGet("property-details/{propertyId}")]
        public async Task<IActionResult> GetPropertyDetails([FromRoute]int propertyId)
        {
            var property = await sellerRepository.GetPropertyDetails(propertyId);
            if (property != null)
            {
                return Ok(property);
            }
            return BadRequest("Property not found.");
        }

        //[Authorize(Roles = "Seller")]
        [HttpPost("UploadImage/{propertyId}")]
        public async Task<IActionResult> UploadImage(IFormFileCollection files, int propertyId)
        {
            try
            {
                if (files.Count == 0)
                {
                    return BadRequest("No files uploaded.");
                }

                var existingImages = await sellerRepository.GetImagesByPropertyId(propertyId);

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

                        var result = await sellerRepository.UploadPropertyImages(image);
                        if (!result)
                        {
                           
                            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add image to the repository.");
                        }
                    }
                }

                return Ok("Images uploaded successfully!");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error: {ex.Message}. Inner Exception: {ex.InnerException?.Message}";
                return BadRequest(errorMessage);
            }

        }
    }
}
