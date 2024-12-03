using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels.Post;
using MyBlog.Plugins.Exceptions;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("[controller]/[Action]")]
    [ApiController]
    public class PostController(PostService postService) : ControllerBase
    {
        private PostService PostService { get; } = postService;

        /// <summary>
        /// دریافت لیست پست ها
        /// </summary>
        /// <returns> <see cref="PostMiniViewModel"/> : return all Posts list</returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public PostMiniViewModel[] GetAll()
        {
            return this.PostService.GetAll();
        }

        /// <summary>
        /// دریافت پست
        /// </summary>
        /// <param name="id">شناسه پست</param>
        /// <returns> <see cref="PostMiniViewModel"/> : return Post by id</returns>
        /// <response code="404 NotFound">در صورتی که هیچ پستی با شناسه وارد شده پیدا نشود</response>
        [AllowAnonymous]
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public PostMiniViewModel Get([Required(ErrorMessage = "شناسه پست را وارد کنید")] long id)
        {
            var post = this.PostService.Get(id);
            return post == null ? throw new HttpException("پست مورد نظر یافت نشد.", "", HttpStatusCode.NotFound) : this.PostService.Get(id);
        }

        /// <summary>
        /// ثبت پست جدید
        /// </summary>
        /// <param name="postModel">اطلاعات پست</param>
        /// <returns> <see cref="long"/> : Add new post</returns>
        /// <response code="400 BadRequest">در صورتی که مقادیر ورودی نامعتبر باشد</response>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه و شناسه والد وارد شده پیدا نشود</response>
        /// <response code="406 NotAcceptable">در صورتی که درخواست ارسالی قابل قبول نباشد</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<long> Register([FromBody] PostViewModel postModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new HttpException("کاربر احراز هویت نشده است.", "", HttpStatusCode.Unauthorized);

            return await this.PostService.Register(postModel, Convert.ToInt64(userId));
        }
    }
}
