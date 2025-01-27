using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;
using Hauna.Urooj.Hauna.Urooj.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hauna.Urooj.Hauna.Urooj.Controllers
{
    [Route("api/Urooj")]
    [ApiController]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var response = _userService.ValidateUser(loginModel);

            if (response == null) {
                return BadRequest(new { message = "Bad Username/Password. Please check your credentials and try again." });
            }

            return Ok(response);
        }

        [HttpGet("Verify")]
        public IActionResult Verify()
        {
            if (Request.Headers["Authorization"].ToString() == null)
            {
                return Unauthorized(new { message = "Token is missing" });
            }
            var token = Request.Headers["Authorization"].ToString().Split(" ").Last();
            var extractedValues = _userService.VerifyToken(token);

            if (extractedValues != null) {
                return Ok(extractedValues);
            }
            return BadRequest();
        }

        [HttpPut("addAdmin")]
        public async Task<IActionResult> AddUser(UserInfoModel userInfoModel,int isAdmin)
        {
            var existingEmail = await _userService.AddAdmin(userInfoModel, isAdmin);
            if (existingEmail != null) {
                return BadRequest("This email already exists");
            }
            return Ok();
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var response = _userService.ChangePassword(changePasswordModel);
            if (response != null)
            {
                return BadRequest("Incorrect Password");
            }
            return Ok();
        }

        [HttpPost("RemoveUser/{userName}")]
        public IActionResult DeleteUser(string userName)
        {
            _userService.RemoveUser(userName);
            return Ok();
        }

        [HttpGet("UserDetails/{userName}")]
        public IActionResult UserDetails(string userName)
        {
            var userDetails = _userService.GetUserDetails(userName);
            if (userDetails != null)
            {
                return Ok(userDetails);
            }
            return NotFound();
        }

        [HttpPost("Subscription")]
        public IActionResult Subscription(SubscriptionModel subscriptionModel)
        {
            _userService.Subscription(subscriptionModel);
            return Ok();
        }
    }
}
