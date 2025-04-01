using System.Linq.Expressions;
using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EasyHousingSolutionProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        IPropertyRepository _propertyRepository;
        IImageRepository imageRepository;
        IWebHostEnvironment _webHostEnvironment;
        ISellerRepository sellerRepository;
        public PropertyController(IPropertyRepository propertyRepository, IImageRepository imageRepository, IWebHostEnvironment webHostEnvironment, ISellerRepository sellerRepository)
        {
            _propertyRepository = propertyRepository;
            this.imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
            this.sellerRepository = sellerRepository;
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("VerifyProperty/{propertyId}")]
        public async Task<IActionResult> VerifyProperty(int propertyId)
        {
            var result = await _propertyRepository.VerifyPropertyByAdmin(propertyId);
            if (result)
            {
                return Ok(new {message="Property Activated Successfully"});
            }
            return BadRequest("Property could not be verified(already verified or does not exist).");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeactivateProperty/{propertyId}/{reason}")]
        public async Task<IActionResult> DeactivateProperty(int propertyId, string reason)
        {
            var result = await _propertyRepository.DeactivatePropertyByAdmin(propertyId, reason);
            if (result)
            {
                return Ok(new {message= "Property Deactivated Successfully"});
            }
            return BadRequest("Property could not be deactivated(already deactivated or does not exist).");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("propertiesByRegion/{region}")]
        public async Task<IActionResult> GetPropertiesByRegion(string region)
        {
            var properties = await _propertyRepository.GetPropertyByRegion(region);
            if (properties != null)
            {
                return Ok(properties);
            }
            return BadRequest("No properties found in the region.");
        }

        [Authorize(Roles= "Admin")]
        [HttpGet("propertiesByOwner/{sellerId}")]
        public async Task<IActionResult> GetPropertiesByOwner(int sellerId)
        {
            var properties = await _propertyRepository.GetPropertiesBySeller(sellerId);
            if (properties != null || !properties.Any())
            {
                return Ok(properties);
            }
            return BadRequest("No properties found for the owner.");
        }

        [HttpGet("GetAllSellers")]
        public async Task<IActionResult> getAllSellers()
        {
            var data = await sellerRepository.GetAllSellersAsync();
            if(data != null)
            {
                var sellerList = data.Select(s => new { s.SellerId, s.FirstName, s.LastName });
                return Ok(sellerList);
            }
            return BadRequest("No Seller Exists");
        }

        [HttpGet("GetPropertyType/{type}")]
        public async Task<IActionResult> GetPropertyByType([FromRoute]string type)
        {
            var data = await _propertyRepository.GetAllVerifiedPropertyByType(type);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("No Property found");
        }

        [HttpGet("GetAllProperty")]
        public async Task<IActionResult> GetAllProperty()
        {
            var data = _propertyRepository.GetAllProperty();
            if(data != null)
            {
                return BadRequest("No Properties Exists");
            }
            else
            {
                return Ok(data);
            }
        }
    }
}
