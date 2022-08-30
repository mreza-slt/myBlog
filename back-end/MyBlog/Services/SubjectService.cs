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
    public class SubjectService
    {
        public SubjectService(BlogDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private BlogDbContext DbContext { get; }

        public async Task<ResponseMessageViewModel> AddDefaultSubject()
        {
            bool subject = this.IsExistSubject(1);
            if (!subject)
            {
                using var transaction = await this.DbContext.Database.BeginTransactionAsync();

                // تکنولوژی---------
                Subject technology = new(1, "1", "تکنولوژی", SubjectType.MajorSubject, null);
                await this.DbContext.Subjects.AddAsync(technology);
                await this.DbContext.SaveChangesAsync();
                // موبایل
                Subject moblie = new(1, this.GetFullCode(technology.FullCode, 1), "موبایل", SubjectType.ForumSubject, technology.Id);
                await this.DbContext.Subjects.AddAsync(moblie);
                // لپتاپ
                Subject lapTop = new(2, this.GetFullCode(technology.FullCode, 2), "لپتاپ", SubjectType.ForumSubject, technology.Id);
                await this.DbContext.Subjects.AddAsync(lapTop);
                // هوش مصنوعی
                Subject aI = new(3, this.GetFullCode(technology.FullCode, 3), "هوش_مصنوعی", SubjectType.ForumSubject, technology.Id);
                await this.DbContext.Subjects.AddAsync(aI);

                // سبک زندگی---------
                Subject lifeStyle = new(2, "2", "سبک زندگی", SubjectType.MajorSubject, null);
                await this.DbContext.Subjects.AddAsync(lifeStyle);
                await this.DbContext.SaveChangesAsync();
                // آرایش و زیبایی
                Subject makeUp = new(1, this.GetFullCode(lifeStyle.FullCode, 1), "آرایش و زیبایی", SubjectType.ForumSubject, lifeStyle.Id);
                await this.DbContext.Subjects.AddAsync(makeUp);
                // آشپزی
                Subject cooking = new(2, this.GetFullCode(lifeStyle.FullCode, 2), "آشپزی", SubjectType.ForumSubject, lifeStyle.Id);
                await this.DbContext.Subjects.AddAsync(cooking);
                // تناسب اندام
                Subject fitness = new(3, this.GetFullCode(lifeStyle.FullCode, 3), "تناسب اندام", SubjectType.ForumSubject, lifeStyle.Id);
                await this.DbContext.Subjects.AddAsync(fitness);

                // ادبیات، فرهنگ، هنر--------
                Subject artAndLiterature = new(3, "3", "ادبیات، فرهنگ، هنر", SubjectType.MajorSubject, null);
                await this.DbContext.Subjects.AddAsync(artAndLiterature);
                await this.DbContext.SaveChangesAsync();
                // کتاب
                Subject book = new(1, this.GetFullCode(artAndLiterature.FullCode, 1), "کتاب", SubjectType.ForumSubject, artAndLiterature.Id);
                await this.DbContext.Subjects.AddAsync(book);
                // سینما
                Subject cinema = new(2, this.GetFullCode(artAndLiterature.FullCode, 2), "سینما", SubjectType.ForumSubject, artAndLiterature.Id);
                await this.DbContext.Subjects.AddAsync(cinema);
                // موسیقی
                Subject music = new(3, this.GetFullCode(artAndLiterature.FullCode, 3), "موسیقی", SubjectType.ForumSubject, artAndLiterature.Id);
                await this.DbContext.Subjects.AddAsync(music);

                // متفرقه
                Subject other = new(4, "4", "متفرقه", SubjectType.MajorSubject, null);
                await this.DbContext.Subjects.AddAsync(other);
                await this.DbContext.SaveChangesAsync();
                // عمومی
                Subject general = new(1, this.GetFullCode(other.FullCode, 1), "عمومی", SubjectType.ForumSubject, other.Id);
                await this.DbContext.Subjects.AddAsync(general);
                await this.DbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseMessageViewModel(null, "موضوعات پیشفرض با موفقیت ثبت شد");
            }
            else
            {
                return new ResponseMessageViewModel(null, "موضوعات پیشفرض قبلا ثبت شده است");
            }
        }

        public SubjectMiniViewModel[] GetAll(long? parentId)
        {
            Subject[] subjects = this.FindSubjects(parentId);
            SubjectMiniViewModel[] subjectMiniModels = subjects.Select(x => new SubjectMiniViewModel { Id = x.Id, Name = x.Name, ParentId = x.ParentId, SubjectType = x.SubjectType, Code = x.Code, FullCode = x.FullCode }).ToArray();

            return subjectMiniModels;
        }

        public async Task<ResponseMessageViewModel> Register(SubjectViewModel subjectModel)
        {
            string? name = this.FindName(subjectModel.Name);
            if (name != null)
            {
                throw new HttpException($"نام {subjectModel.Name} قبلا برای یک موضوع ثبت شده است", nameof(SubjectViewModel.Name), HttpStatusCode.Conflict);
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

            Subject subject = new(code, this.GetFullCode(parentFullCode, code), subjectModel.Name, subjectModel.SubjectType, subjectModel.ParentId is null or 0 ? null : subjectModel.ParentId);
            await this.DbContext.AddAsync(subject);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "موضوع جدید با موفقیت ثبت شد");
        }

        public async Task<ResponseMessageViewModel> Edit(long id, string subjectName)
        {
            Subject? subject = this.FindSubject(id);
            if (subject == null)
            {
                throw new HttpException($"هیچ موضوعی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);
            }

            string? name = this.FindName(subjectName);
            if (name != null && name != subject.Name)
            {
                throw new HttpException($"نام {subjectName} قبلا برای یک موضوع ثبت شده است", nameof(subjectName), HttpStatusCode.Conflict);
            }

            Subject.Copy(subjectName, subject);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "ویرایش نام موضوع با موفقیت انجام شد");
        }

        public async Task<ResponseMessageViewModel> Delete(long id)
        {
            Subject? subject = this.FindSubject(id);
            if (subject == null)
            {
                throw new HttpException($"هیچ موضوعی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);
            }

            try
            {
                this.DbContext.Subjects.Remove(subject);
                await this.DbContext.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                if (ex.ErrorCode == -2146232060 && ex.Number == 547)
                {
                    throw new HttpException("خطا در حذف اطلاعات موضوع. از این اطلاعات در جای دیگر استفاده شده است", "", HttpStatusCode.NotAcceptable);
                }

                throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException)
                {
                    var exception = ex.InnerException as SqlException;
                    if (exception!.ErrorCode == -2146232060 && exception.Number == 547)
                    {
                        throw new HttpException("خطا در حذف اطلاعات موضوع. از این اطلاعات در جای دیگر استفاده شده است", "", HttpStatusCode.NotAcceptable);
                    }
                }

                throw;
            }

            return new ResponseMessageViewModel(null, "حذف موضوع با موفقیت انجام شد");
        }

        // Util Methods
        public int GetCode(long? parentId)
        {
            int? maxCode = this.FindMaxCode(parentId);

            int newCode = (maxCode is null or 0 ? 1 : maxCode + 1).Value;

            return newCode;
        }

        public string GetFullCode(string? parentFullCode, int code)
        {
            if (!string.IsNullOrEmpty(parentFullCode))
            {
                return parentFullCode + "/" + code;
            }

            return code.ToString();
        }

        // Datebase Methods
        public Subject[] FindSubjects(long? parentId)
        {
            return this.DbContext.Subjects.Where(x => x.ParentId == parentId).ToArray();
        }

        public int? FindMaxCode(long? parentId)
        {
            Expression<Func<Subject, bool>> pridicate = x => x.ParentId == parentId;
            if (!this.DbContext.Subjects.Where(pridicate).Any())
            {
                return null;
            }

            int code = this.DbContext.Subjects.Where(pridicate)
                .Select(x => x.Code).Max();

            return code;
        }

        public Subject? FindSubject(long id)
        {
            return this.DbContext.Subjects.FirstOrDefault(x => x.Id == id);
        }

        public Subject? FindSubject(long id, SubjectType subjectType)
        {
            return this.DbContext.Subjects.FirstOrDefault(x => x.Id == id && x.SubjectType == subjectType);
        }

        public string? FindFullCode(long id)
        {
            return this.DbContext.Subjects.Where(x => x.Id == id).Select(x => x.FullCode).FirstOrDefault();
        }

        public string? FindName(string name)
        {
            return this.DbContext.Subjects.Where(x => x.Name == name).Select(x => x.Name).FirstOrDefault();
        }

        public bool IsExistSubject(int code)
        {
            return this.DbContext.Subjects.Any(x => x.Code == code);
        }
    }
}
