using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Weather_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {

            var result = await _userService.RegisterAsync(model);
            return Ok(result);
        }
        [HttpPost("Role")]
        public async Task<ResponseModel> AddUserToRoleAsync(ManageRoleModel model)
        {
            var result = await _userService.AddUserToRoleAsync(model);
            return result;
        }
        [HttpPost("login")]
        public Task<ResponseModel> LoginAsync(LoginModel model)
        {
            return _userService.LoginAsync(model);
        }
    }
}
