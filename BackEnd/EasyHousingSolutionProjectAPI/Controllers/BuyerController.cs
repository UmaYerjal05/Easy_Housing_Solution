using EasyHousingSolutionProjectAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyHousingSolutionProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        IBuyerRepository buyerRepository;

        public BuyerController(IBuyerRepository buyerRepository)
        {
            this.buyerRepository = buyerRepository;
        }

        [HttpGet("GetBuyerIdByUsername/{username}")]
        public async Task<IActionResult> GetBuyer(string username) {
           var data = await buyerRepository.GetBuyerByName(username);
            if (data == null)
            {
                return NotFound("Buyer not found");
            }
            return Ok(data);
        }

        [HttpGet("GetPropertyByRegion/{region}")]
        public async Task<IActionResult> GetPropertyByRegion(string region)
        {
            var data = await buyerRepository.SearchPropertyByRegion(region);
            if (data == null)
            {
                return NotFound("No Property is exiting in this region");
            }
            return Ok(data);
        }

        [HttpGet("GetPropertyByType/{type}")]
        public async Task<IActionResult> GetPropertyByType(string type)
        {
            var data = await buyerRepository.SearchPropertyByType(type);
            if (data == null)
            {
                return NotFound("No Property is existing in this type");
            }
            return Ok(data);
        }

        [HttpGet("SortPropertyByPrice")]
        public async Task<IActionResult> SortPropertyByPrice([FromQuery] Decimal price)
        {
            var data = await buyerRepository.SortPropertyByPrice(price);
            return Ok(data);
        }

        //[Authorize(Roles ="Buyer")]
        [HttpPost("AddToWishList/{buyerId}/{propertyId}")]
        public async Task<IActionResult> AddPropertyToWishList([FromRoute] int buyerId, [FromRoute] int propertyId)
        {
            var data = await buyerRepository.AddToWishList(buyerId, propertyId);
            if (data)
            {
                return Ok(new {message= "Property added to wishlist"});
                }
            return BadRequest("Property already present in the wishlist");
        }

        [HttpGet("GetPropertyWithSellerContactDetails/{propertyId}")]
        public async Task<IActionResult> GetPropertyWithSellerContatctDetails(int propertyId)
        {
            var data = await buyerRepository.GetSellerContactDetails(propertyId);
            if (data == null)
            {
                return NotFound("No seller contact");
            }
            return Ok(data);

        }

        [HttpGet("GetWishlistPropertyDetails/{buyerId}")]
        public async Task<IActionResult> GetAllPropertyFromWishlist([FromRoute] int buyerId)
        {
            var data = await buyerRepository.GetWishLists(buyerId);
            if (data == null)
            {
                return NotFound("Wishlist is empty");
            }
            return Ok(data);
        }

        [HttpDelete("DeletePropertyFromWishlist")]
        public async Task<IActionResult> DeletePropertyfromWishlist([FromQuery] int buyerId, [FromQuery] int propertyId)
        {
            var data = await buyerRepository.DeletePropertyFromWishList(buyerId, propertyId);
            if (data)
            {
                return Ok(new {message= "Property removed from wishlist" });
            }
            return BadRequest("no property data is found");
        }

        [HttpGet("ViewPropertyDetails/{propertyId}")]
        public async Task<IActionResult> GetPropertyDetails(int propertyId)
        {
            var data =await buyerRepository.GetPropertyById(propertyId);
            if(data == null)
            {
                return NotFound("No Property found");
            }
            return Ok(data);
        }
    }
}
