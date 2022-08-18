using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.User;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(UserService userService)
        {
            this.UserService = userService;
        }

        private UserService UserService { get; }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseMessageViewModel> Register([FromBody] RegisterUserViewModel userModel)
        {
            return await this.UserService.Register(userModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseMessageViewModel> Login([FromBody] LoginViewModel userModel)
        {
            return await this.UserService.Login(userModel);
        }
    }
}