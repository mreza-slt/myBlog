using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.User;
using MyBlog.Plugins.Exceptions;
using MyBlog.Services;
using System.Net;

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

        [HttpGet]
        public async Task<ResponseMessageViewModel> Logout()
        {
            return await this.UserService.Logout();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        public void UnAuthorized()
        {
            throw new HttpException("دسترسی غیر مجاز, شما مجوز لازم را ندارید", "", HttpStatusCode.Unauthorized);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        public void UnAuthorizedLogin()
        {
            throw new HttpException("شما وارد سیستم نشده اید یا برای مدتی از سیستم استفاده نکرده اید و یا از سیستم خارج شده اید", "", HttpStatusCode.Unauthorized);
        }
    }
}