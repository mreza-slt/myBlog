using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Subject;
using MyBlog.Services;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        public SubjectController(SubjectService subjectService)
        {
            this.SubjectService = subjectService;
        }

        private SubjectService SubjectService { get; }

        /// <summary>
        /// اضافه کردن موضوعات پیشفرض
        /// </summary>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای اضافه کردن موضوعات پیشفرض از این متد استفاده می شود
        /// </remarks>
        [HttpPost]
        public async Task<ResponseMessageViewModel> AddDefaultAccountType()
        {
            return await this.SubjectService.AddDefaultSubject();
        }

        /// <summary>
        /// دریافت لیست موضوعات
        /// </summary>
        /// <returns>A <see cref="SubjectMiniViewModel"/> : return all Subjects list</returns>
        /// <remarks>
        /// برای  دریافت لیست موضوعات از این متد استفاده می شود
        /// </remarks>
        [HttpGet]
        public SubjectMiniViewModel[] GetAll()
        {
            return this.SubjectService.GetAll();
        }

        /// <summary>
        /// ثبت موضوع جدید
        /// </summary>
        /// <param name="subjectModel">اطلاعات موضوع</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای ایجاد موضوع جدید از این متد استفاده می شود
        /// </remarks>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه والد وارد شده پیدا نشود</response>
        /// <response code="406 NotAcceptable">در صورتی که درخواست ارسالی قابل قبول نباشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPost]
        public async Task<ResponseMessageViewModel> Register([FromBody] SubjectViewModel subjectModel)
        {
            return await this.SubjectService.Register(subjectModel);
        }

        /// <summary>
        /// ویرایش اطلاعات موضوع
        /// </summary>
        /// <param name="id">شناسه موضوع</param>
        /// <param name="name">نام موضوع برای ویرایش</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای ویرایش اطلاعات دسته بندی یا موضوع از این متد استفاده می شود
        /// </remarks>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه وارد شده پیدا نشود</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPut("{id}")]
        public async Task<ResponseMessageViewModel> Edit(long id, [Required(ErrorMessage = "لطفا نام را وارد کنید")] string name)
        {
            return await this.SubjectService.Edit(id, name);
        }

        /// <summary>
        /// حذف موضوع
        /// </summary>
        /// <param name="id">شناسه موضوع</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        /// <remarks>
        /// برای حذف دسته بندی یا موضوع از این متد استفاده می شود
        /// </remarks>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه وارد شده پیدا نشود</response>
        [HttpDelete("{id}")]
        public async Task<ResponseMessageViewModel> Delete(long id)
        {
            return await this.SubjectService.Delete(id);
        }
    }
}
