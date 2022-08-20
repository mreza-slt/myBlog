using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Article;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        public ArticleController(ArticleService articleService)
        {
            this.ArticleService = articleService;
        }

        private ArticleService ArticleService { get; }

        /// <summary>
        /// دریافت لیست مقاله ها
        /// </summary>
        /// <returns>A <see cref="ArticleMiniViewModel"/> : return all Articles list</returns>
        /// <remarks>
        /// برای  دریافت لیست مقاله ها از این متد استفاده می شود
        /// </remarks>
        [HttpGet]
        public ArticleMiniViewModel[] GetAll()
        {
            return this.ArticleService.GetAll();
        }

        /// <summary>
        /// ثبت مقاله جدید
        /// </summary>
        /// <param name="articleModel">اطلاعات مقاله</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای ایجاد مقاله جدید از این متد استفاده می شود
        /// </remarks>
        /// <response code="400 BadRequest">در صورتی که مقادیر ورودی نامعتبر باشد</response>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه و شناسه والد وارد شده پیدا نشود</response>
        /// <response code="406 NotAcceptable">در صورتی که درخواست ارسالی قابل قبول نباشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPost]
        public async Task<ResponseMessageViewModel> Register([FromBody] ArticleViewModel articleModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await this.ArticleService.Register(articleModel, Convert.ToInt64(userId));
        }
    }
}
