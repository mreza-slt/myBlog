using System.Linq.Expressions;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums.Subject;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Subject;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class SubjectService(BlogDbContext dbContext)
    {
        private BlogDbContext DbContext { get; } = dbContext;

        //// افزودن موضوعات پیش‌فرض
        public async Task<ResponseMessageViewModel> AddDefaultSubject()
        {
            if (this.IsExistSubject(1))
                return new ResponseMessageViewModel(null, "موضوعات پیشفرض قبلا ثبت شده است");

            using var transaction = await this.DbContext.Database.BeginTransactionAsync();

            try
            {
                // تعریف دسته‌بندی‌ها و موضوعات مرتبط
                var defaultSubjects = new[]
                {
                    // تکنولوژی
                    new Subject(1, "1", "تکنولوژی", SubjectType.MajorSubject, null),
                    new Subject(1, "1/1", "موبایل", SubjectType.ForumSubject, 1),
                    new Subject(2, "1/2", "لپتاپ", SubjectType.ForumSubject, 1),
                    new Subject(3, "1/3", "هوش مصنوعی", SubjectType.ForumSubject, 1),

                    // سبک زندگی
                    new Subject(2, "2", "سبک زندگی", SubjectType.MajorSubject, null),
                    new Subject(1, "2/1", "آرایش و زیبایی", SubjectType.ForumSubject, 2),
                    new Subject(2, "2/2", "آشپزی", SubjectType.ForumSubject, 2),
                    new Subject(3, "2/3", "تناسب اندام", SubjectType.ForumSubject, 2),

                    // ادبیات، فرهنگ و هنر
                    new Subject(3, "3", "ادبیات، فرهنگ و هنر", SubjectType.MajorSubject, null),
                    new Subject(1, "3/1", "کتاب", SubjectType.ForumSubject, 3),
                    new Subject(2, "3/2", "سینما", SubjectType.ForumSubject, 3),
                    new Subject(3, "3/3", "موسیقی", SubjectType.ForumSubject, 3),

                    // متفرقه
                    new Subject(4, "4", "متفرقه", SubjectType.MajorSubject, null),
                    new Subject(1, "4/1", "عمومی", SubjectType.ForumSubject, 4),
                };

                await this.DbContext.Subjects.AddRangeAsync(defaultSubjects);
                await this.DbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseMessageViewModel(null, "موضوعات پیش‌فرض با موفقیت ثبت شدند");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public SubjectMiniViewModel[] GetAll(long? parentId) =>
            [.. this.FindSubjects(parentId)
            .Select(x => new SubjectMiniViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                SubjectType = x.SubjectType,
                Code = x.Code,
                FullCode = x.FullCode,
            })];

        public async Task<ResponseMessageViewModel> Register(SubjectViewModel subjectModel)
        {
            if (this.FindName(subjectModel.Name) is not null)
            {
                throw new HttpException($"نام {subjectModel.Name} قبلا ثبت شده است", nameof(SubjectViewModel.Name), HttpStatusCode.Conflict);
            }

            if (subjectModel.SubjectType == SubjectType.MajorSubject && !(subjectModel.ParentId is null or 0))
            {
                throw new HttpException("برای موضوع از نوع دسته بندی 'اصلی' نباید شناسه والد وارد کنید", nameof(SubjectViewModel.SubjectType), HttpStatusCode.NotAcceptable);
            }

            string? parentFullCode = null;
            if (subjectModel.SubjectType == SubjectType.ForumSubject && subjectModel.ParentId is null or 0)
            {
                throw new HttpException("بدلیل اینکه نوع دسته بندی زیر مجموعه را انتخاب کرده اید، شناسه والد را باید وارد کنید", nameof(SubjectViewModel.SubjectType), HttpStatusCode.NotAcceptable);
            }
            else if (subjectModel.SubjectType != SubjectType.MajorSubject)
            {
                parentFullCode = this.FindFullCode(subjectModel.ParentId!.Value);
                if (string.IsNullOrEmpty(parentFullCode))
                {
                    throw new HttpException($"هیچ موضوعی یا دسته بندی با شناسه والد {subjectModel.ParentId} پیدا نشد", nameof(SubjectViewModel.ParentId), HttpStatusCode.NotFound);
                }
            }

            int code = this.GetCode(subjectModel.ParentId);
            Subject newSubject = new(
                code,
                this.GetFullCode(parentFullCode, code),
                subjectModel.Name,
                subjectModel.SubjectType,
                subjectModel.ParentId is null or 0 ? null : subjectModel.ParentId
                );
            await this.DbContext.AddAsync(newSubject);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "موضوع با موفقیت ثبت شد");
        }

        public async Task<ResponseMessageViewModel> Edit(long id, string subjectName)
        {
            Subject? subject = this.FindSubject(id) ?? throw new HttpException($"هیچ موضوعی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);

            if (this.FindName(subjectName) is string existingName && existingName != subject.Name)
                throw new HttpException($"نام {subjectName} قبلا برای یک موضوع ثبت شده است", nameof(subjectName), HttpStatusCode.Conflict);

            Subject.Copy(subjectName, subject);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "ویرایش نام موضوع با موفقیت انجام شد");
        }

        public async Task<ResponseMessageViewModel> Delete(long id)
        {
            Subject? subject = this.FindSubject(id)
                ?? throw new HttpException($"هیچ موضوعی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);

            try
            {
                this.DbContext.Subjects.Remove(subject);
                await this.DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547, ErrorCode: -2146232060 })
            {
                throw new HttpException("خطا در حذف اطلاعات موضوع. از این اطلاعات در جای دیگر استفاده شده است", "", HttpStatusCode.NotAcceptable);
            }

            return new ResponseMessageViewModel(null, "موضوع با موفقیت حذف شد.");
        }

        // Util and Datebase Methods
        private string GetFullCode(string? parentFullCode, int code) => string.IsNullOrEmpty(parentFullCode) ? code.ToString() : $"{parentFullCode}/{code}";

        private int GetCode(long? parentId) => this.DbContext.Subjects.Where(x => x.ParentId == parentId).Select(x => x.Code).DefaultIfEmpty(0).Max() + 1;

        private bool IsExistSubject(int code) => this.DbContext.Subjects.Any(x => x.Code == code);

        public string? FindFullCode(long id)
        {
            return this.DbContext.Subjects.Where(x => x.Id == id).Select(x => x.FullCode).FirstOrDefault();
        }

        private Subject[] FindSubjects(long? parentId) => [.. this.DbContext.Subjects.Where(x => x.ParentId == parentId)];

        private Subject? FindSubject(long id) => this.DbContext.Subjects.FirstOrDefault(x => x.Id == id);

        public Subject? FindSubject(long id, SubjectType subjectType) => this.DbContext.Subjects.FirstOrDefault(x => x.Id == id && x.SubjectType == subjectType);

        private string? FindName(string name) => this.DbContext.Subjects.Where(x => x.Name == name).Select(x => x.Name).FirstOrDefault();
    }
}
