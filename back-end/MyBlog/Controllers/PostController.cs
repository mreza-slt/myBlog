using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Post;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("[controller]/[Action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        public PostController(PostService PostService)
        {
            this.PostService = PostService;
        }

        private PostService PostService { get; }

        /// <summary>
        /// دریافت لیست پست ها
        /// </summary>
        /// <returns>A <see cref="PostMiniViewModel"/> : return all Posts list</returns>
        /// <remarks>
        /// برای  دریافت لیست پست ها از این متد استفاده می شود
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public PostMiniViewModel[] GetAll()
        {
            return this.PostService.GetAll();
        }

        /// <summary>
        /// ثبت پست جدید
        /// </summary>
        /// <param name="PostModel">اطلاعات پست</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای ایجاد پست جدید از این متد استفاده می شود
        /// </remarks>
        /// <response code="400 BadRequest">در صورتی که مقادیر ورودی نامعتبر باشد</response>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه و شناسه والد وارد شده پیدا نشود</response>
        /// <response code="406 NotAcceptable">در صورتی که درخواست ارسالی قابل قبول نباشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPost]
        public async Task<ResponseMessageViewModel> Register([FromBody] PostViewModel PostModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await this.PostService.Register(PostModel, Convert.ToInt64(userId));
        }
    }
}
