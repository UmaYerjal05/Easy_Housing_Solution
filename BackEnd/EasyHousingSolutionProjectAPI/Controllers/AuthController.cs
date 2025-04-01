using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using EasyHousingSolutionProjectAPI.Repository;

namespace EasyHousingSolutionProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly ISellerRepository _sellerRepository;
        private readonly IBuyerRepository _buyerRepository;
        
        public AuthController(IUserRepository userRepository, JwtHelper jwtHelper,ISellerRepository sellerRepository,IBuyerRepository buyerRepository)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _sellerRepository = sellerRepository;
            _buyerRepository = buyerRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserVM userVM)
        {
            if (userVM == null)
                return BadRequest("Invalid data.");

            // Check if the user already exists
            var existingUser = await _userRepository.GetByUsernameAsync(userVM.UserName);
            if (existingUser != null)
                return BadRequest("User already exists.");

            // Create the User object
            var user = new User
            {
                UserName = userVM.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(userVM.Password),
                UserType = userVM.UserType
            };

            // Save the user to the database
            var result = await _userRepository.CreateUserAsync(user);
            if (!result)
                return StatusCode(500, "Internal server error.");

            // Handle Buyer registration
            if (userVM.UserType == "Buyer")
            {
                var buyer = new Buyer
                {
                    UserId = user.UserId,  // Foreign Key reference to User
                    FirstName = userVM.FirstName,
                    LastName = userVM.LastName,
                    DateOfBirth = userVM.DateOfBirth,
                    PhoneNo = userVM.PhoneNo,
                    EmailId = userVM.EmailId
                };

                var buyerResult =await _buyerRepository.Add(buyer);
                if (!buyerResult)
                    return StatusCode(500, "Error registering Buyer.");
            }

            // Handle Seller registration
            if (userVM.UserType == "Seller")
            {
                var seller = new Seller
                {
                    UserId = user.UserId,  // Foreign Key reference to User
                    UserName = user.UserName,
                    FirstName = userVM.FirstName,
                    LastName = userVM.LastName,
                    DateOfBirth = userVM.DateOfBirth,
                    PhoneNo = userVM.PhoneNo,
                    Address = userVM.Address,
                    StateId = userVM.StateId,
                    CityId = userVM.CityId,
                    EmailId = userVM.EmailId
                };

                var sellerResult = await _sellerRepository.AddSeller(seller);
                if (!sellerResult)
                    return StatusCode(500, "Error registering Seller.");
            }


            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login/{Username}/{Password}/{UserType}")]
        public async Task<IActionResult> Login([FromRoute] string Username, [FromRoute] string Password, [FromRoute] string UserType)
        {
            // Retrieve the user by username
            var existingUser = await _userRepository.GetByUsernameAsync(Username);
            var existingUserType = existingUser.UserType;


            // Check if the user exists and if the password matches the stored hash
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(Password, existingUser.Password))
            {
                return Unauthorized();  // Unauthorized if the user doesn't exist or the password doesn't match
            }

            if(Username != existingUser.UserName)
            {
                return Unauthorized();
            }
            else if(UserType != existingUserType)
            {
                return Unauthorized();
            }


                // Generate JWT token
                var token = _jwtHelper.GenerateJwtToken(existingUser.UserName, existingUser.UserType);

            return Ok(new { Token = token });
        }

    }
}
