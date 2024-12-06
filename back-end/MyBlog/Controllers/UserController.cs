using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.User;
using MyBlog.Plugins.Exceptions;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("[controller]/[Action]")]
    [ApiController]
    public class UserController(UserService userService) : ControllerBase
    {
        private UserService UserService { get; } = userService;

        /// <summary>
        /// ورود به حساب کاربری
        /// </summary>
        /// <param name="userModel">اطلاعات ورود کاربر</param>
        /// <returns>A <see cref="ProfileUserViewModel"/> : return user info</returns>
        /// <response code="401 Unauthorized">در صورتی که رمز عبور اشتباه وارد شود</response>
        /// <response code="403 Forbidden">در صورتی که امکان ورود به سیستم در این بازه زمانی وجود نداشته باشد</response>
        /// <response code="404 NotFound">در صورتی که هیچ کاربری با نام کاربری یا ایمیل یا شماره تلفن پیدا نشود</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ProfileUserViewModel> Login([FromBody] LoginViewModel userModel)
            => await this.UserService.Login(userModel);

        /// <summary>
        /// ایجاد حساب کاربری
        /// </summary>
        /// <param name="userModel">اطلاعات کاربر</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : Register user and return user information</returns>
        /// <response code="400 BadRequest">در صورتی که مقادیر ورودی نامعتبر باشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseMessageViewModel> Register([FromBody] RegisterUserViewModel userModel)
            => await this.UserService.Register(userModel);

        /// <summary>
        /// ویرایش حساب کاربری
        /// </summary>
        /// <param name="userModel">اطلاعات کاربر</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> :Edit User Profile </returns>
        /// <response code="400 BadRequest">در صورتی که مقادیر ورودی نامعتبر باشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPut]
        public async Task<ResponseMessageViewModel> Profile([FromBody] ProfileUserViewModel userModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await this.UserService.Profile(Convert.ToInt64(userId), userModel);
        }

        /// <summary>
        /// خروج از حساب کاربری
        /// </summary>
        /// <returns>A <see cref="ResponseMessageViewModel"/>Logout Account</returns>
        [HttpGet]
        public async Task<ResponseMessageViewModel> Logout()
        {
            return await this.UserService.Logout();
        }

        /// <summary>
        ///  ارسال کد تایید به ایمیل
        /// </summary>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : Send code to email</returns>
        /// <response code="403 Forbidden">در صورتی که امکان ارسال کد تایید در این بازه زمانی وجود نداشته باشد</response>
        [HttpGet]
        public async Task<ResponseMessageViewModel> SendEmailConfirmCode()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await this.UserService.SendEmailConfirmCode(Convert.ToInt64(userId));
        }

        /// <summary>
        /// تایید ایمیل کاربر
        /// </summary>
        /// <param name="confirmCode">دریافت کد تایید ارسال شده به ایمیل کاربر</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : Confirm EmailCode </returns>
        /// <response code="400 BadRequest">در صورتی که مقدار وارد شده نامعتبر باشد</response>
        [HttpGet]
        public async Task<ResponseMessageViewModel> EmailConfirm([Required(ErrorMessage = "کد تایید ارسال شده به ایمیل را وارد کنید")] int confirmCode)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await this.UserService.EmailConfirm(Convert.ToInt64(userId), confirmCode);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpGet, HttpPost, HttpPut, HttpDelete]
        public void UnAuthorized()
           => throw new HttpException("دسترسی غیر مجاز, شما مجوز لازم را ندارید", "", HttpStatusCode.Unauthorized);

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpGet, HttpPost, HttpPut, HttpDelete]
        public void UnAuthorizedLogin()
         => throw new HttpException("شما وارد سیستم نشده اید یا برای مدتی از سیستم استفاده نکرده اید و یا از سیستم خارج شده اید", "", HttpStatusCode.Unauthorized);
    }
}