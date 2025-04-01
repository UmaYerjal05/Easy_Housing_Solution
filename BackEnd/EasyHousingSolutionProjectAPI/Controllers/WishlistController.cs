using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyHousingSolutionProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        IWishlistRepository wishlistRepository;
        public WishlistController(IWishlistRepository wishlistRepository)
        {
            this.wishlistRepository = wishlistRepository;
        }

        [HttpPost("AddToWishlist")]
        public async Task<IActionResult> AddToWishlist(int buyerId,int propertyId)
        {
            var result = await wishlistRepository.AddToWishlist(buyerId, propertyId);
            if (result)
            {
                return Ok("Property added to wishlist successfully");
            }
            return BadRequest("Property could not be added to wishlist.");
        }

        [HttpGet("GetWishLists/{buyerId}")]
        public async Task<IActionResult> GetWishLists([FromRoute] int buyerId)
        {
            var wishlists = await wishlistRepository.GetPropertiesInWishListsByBuyerId(buyerId);
            if (wishlists != null)
            {
                return Ok(wishlists);
            }
            return BadRequest("No properties found in the wishlist.");
        }

        [HttpDelete("RemoveFromWishlist/{buyerId}/{propertyId}")]
        public async Task<IActionResult> RemoveFromWishlist(int buyerId, int propertyId)
        {
            var result = await wishlistRepository.RemoveFromWishlist(buyerId,propertyId);
            if (result)
            {
                return Ok("Property removed from wishlist successfully");
            }
            return BadRequest("Property could not be removed from wishlist.");
        }
    }
}
