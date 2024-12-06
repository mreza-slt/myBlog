using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Subject;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    [Authorize]
    [Route("[controller]/[Action]")]
    [ApiController]
    public class SubjectController(SubjectService subjectService) : ControllerBase
    {
        private SubjectService SubjectService { get; } = subjectService;

        /// <summary>
        /// اضافه کردن موضوعات پیشفرض
        /// </summary>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : return Success Or Error Response</returns>
        [HttpPost]
        public async Task<ResponseMessageViewModel> AddDefaultAccountType()
            => await this.SubjectService.AddDefaultSubject();

        /// <summary>
        /// دریافت لیست موضوعات
        /// </summary>
        /// <param name="parentId">شناسه والد موضوع</param>
        /// <returns>A <see cref="SubjectMiniViewModel"/> : return all Subjects list</returns>
        [HttpGet]
        public SubjectMiniViewModel[] GetAll(long? parentId)
        => this.SubjectService.GetAll(parentId == 0 ? null : parentId);

        /// <summary>
        /// ثبت موضوع جدید
        /// </summary>
        /// <param name="subjectModel">اطلاعات موضوع</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : Create new subject </returns>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه والد وارد شده پیدا نشود</response>
        /// <response code="406 NotAcceptable">در صورتی که درخواست ارسالی قابل قبول نباشد</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPost]
        public async Task<ResponseMessageViewModel> Register([FromBody] SubjectViewModel subjectModel)
            => await this.SubjectService.Register(subjectModel);

        /// <summary>
        /// ویرایش اطلاعات موضوع
        /// </summary>
        /// <param name="id">شناسه موضوع</param>
        /// <param name="name">نام موضوع برای ویرایش</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : Edit subject information </returns>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه وارد شده پیدا نشود</response>
        /// <response code="409 Conflict">در صورتی که مقادیر ورودی تکراری باشد</response>
        [HttpPut("{id}")]
        public async Task<ResponseMessageViewModel> Edit(long id, [Required(ErrorMessage = "لطفا نام را وارد کنید")] string name)
            => await this.SubjectService.Edit(id, name);

        /// <summary>
        /// حذف موضوع
        /// </summary>
        /// <param name="id">شناسه موضوع</param>
        /// <returns>A <see cref="ResponseMessageViewModel"/> : Delete subject</returns>
        /// <response code="404 NotFound">در صورتی که هیچ موضوع یا دسته بندی با شناسه وارد شده پیدا نشود</response>
        [HttpDelete("{id}")]
        public async Task<ResponseMessageViewModel> Delete(long id)
            => await this.SubjectService.Delete(id);
    }
}
