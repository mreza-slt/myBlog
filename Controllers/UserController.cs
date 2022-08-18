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

        /// <summary>
        /// ایجاد حساب کاربری
        /// </summary>
        /// <param name="userModel">اطلاعات کاربر</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای ایجاد حساب کاربری جدید از این متد استفاده می شود
        /// </remarks>
        /// <response code="400 BadRequest">در صورتی که مقادیر ورودی نامعتبر باشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseMessageViewModel> Register([FromBody] RegisterUserViewModel userModel)
        {
            return await this.UserService.Register(userModel);
        }
    }
}