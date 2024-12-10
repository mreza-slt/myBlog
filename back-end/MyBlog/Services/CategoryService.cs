using System.Linq.Expressions;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums.Category;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.Category;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class CategoryService(BlogDbContext dbContext)
    {
        private BlogDbContext DbContext { get; } = dbContext;

        //// افزودن موضوعات پیش‌فرض
        public async Task<ResponseMessageViewModel> AddDefaultCategory()
        {
            if (this.IsExistCategory(1))
                return new ResponseMessageViewModel(null, "موضوعات پیشفرض قبلا ثبت شده است");

            using var transaction = await this.DbContext.Database.BeginTransactionAsync();

            try
            {
                // تعریف دسته‌بندی‌ها و موضوعات مرتبط
                var defaultCategorys = new[]
                {
                    // تکنولوژی
                    new Category(1, "1", "تکنولوژی", CategoryType.MajorCategory, null),
                    new Category(1, "1/1", "موبایل", CategoryType.ForumCategory, 1),
                    new Category(2, "1/2", "لپتاپ", CategoryType.ForumCategory, 1),
                    new Category(3, "1/3", "هوش مصنوعی", CategoryType.ForumCategory, 1),

                    // سبک زندگی
                    new Category(2, "2", "سبک زندگی", CategoryType.MajorCategory, null),
                    new Category(1, "2/1", "آرایش و زیبایی", CategoryType.ForumCategory, 2),
                    new Category(2, "2/2", "آشپزی", CategoryType.ForumCategory, 2),
                    new Category(3, "2/3", "تناسب اندام", CategoryType.ForumCategory, 2),

                    // ادبیات، فرهنگ و هنر
                    new Category(3, "3", "ادبیات، فرهنگ و هنر", CategoryType.MajorCategory, null),
                    new Category(1, "3/1", "کتاب", CategoryType.ForumCategory, 3),
                    new Category(2, "3/2", "سینما", CategoryType.ForumCategory, 3),
                    new Category(3, "3/3", "موسیقی", CategoryType.ForumCategory, 3),

                    // متفرقه
                    new Category(4, "4", "متفرقه", CategoryType.MajorCategory, null),
                    new Category(1, "4/1", "عمومی", CategoryType.ForumCategory, 4),
                };

                await this.DbContext.Categorys.AddRangeAsync(defaultCategorys);
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

        public CategoryMiniViewModel[] GetAll(long? parentId) =>
            [.. this.FindCategorys(parentId)
            .Select(x => new CategoryMiniViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                CategoryType = x.CategoryType,
                Code = x.Code,
                FullCode = x.FullCode,
            })];

        public async Task<ResponseMessageViewModel> Register(CategoryViewModel categoryModel)
        {
            if (this.FindName(categoryModel.Name) is not null)
            {
                throw new HttpException($"نام {categoryModel.Name} قبلا ثبت شده است", nameof(CategoryViewModel.Name), HttpStatusCode.Conflict);
            }

            if (categoryModel.CategoryType == CategoryType.MajorCategory && !(categoryModel.ParentId is null or 0))
            {
                throw new HttpException("برای موضوع از نوع دسته بندی 'اصلی' نباید شناسه والد وارد کنید", nameof(CategoryViewModel.CategoryType), HttpStatusCode.NotAcceptable);
            }

            string? parentFullCode = null;
            if (categoryModel.CategoryType == CategoryType.ForumCategory && categoryModel.ParentId is null or 0)
            {
                throw new HttpException("بدلیل اینکه نوع دسته بندی زیر مجموعه را انتخاب کرده اید، شناسه والد را باید وارد کنید", nameof(CategoryViewModel.CategoryType), HttpStatusCode.NotAcceptable);
            }
            else if (categoryModel.CategoryType != CategoryType.MajorCategory)
            {
                parentFullCode = this.FindFullCode(categoryModel.ParentId!.Value);
                if (string.IsNullOrEmpty(parentFullCode))
                {
                    throw new HttpException($"هیچ موضوعی یا دسته بندی با شناسه والد {categoryModel.ParentId} پیدا نشد", nameof(CategoryViewModel.ParentId), HttpStatusCode.NotFound);
                }
            }

            int code = this.GetCode(categoryModel.ParentId);
            Category newCategory = new(
                code,
                this.GetFullCode(parentFullCode, code),
                categoryModel.Name,
                categoryModel.CategoryType,
                categoryModel.ParentId is null or 0 ? null : categoryModel.ParentId
                );
            await this.DbContext.AddAsync(newCategory);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "موضوع با موفقیت ثبت شد");
        }

        public async Task<ResponseMessageViewModel> Edit(long id, string categoryName)
        {
            Category? category = this.FindCategory(id) ?? throw new HttpException($"هیچ موضوعی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);

            if (this.FindName(categoryName) is string existingName && existingName != category.Name)
                throw new HttpException($"نام {categoryName} قبلا برای یک موضوع ثبت شده است", nameof(categoryName), HttpStatusCode.Conflict);

            Category.Copy(categoryName, category);
            await this.DbContext.SaveChangesAsync();

            return new ResponseMessageViewModel(null, "ویرایش نام موضوع با موفقیت انجام شد");
        }

        public async Task<ResponseMessageViewModel> Delete(long id)
        {
            Category? category = this.FindCategory(id)
                ?? throw new HttpException($"هیچ موضوعی با شناسه {id} پیدا نشد", nameof(id), HttpStatusCode.NotFound);

            try
            {
                this.DbContext.Categorys.Remove(category);
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

        private int GetCode(long? parentId) => this.DbContext.Categorys.Where(x => x.ParentId == parentId).Select(x => x.Code).DefaultIfEmpty(0).Max() + 1;

        private bool IsExistCategory(int code) => this.DbContext.Categorys.Any(x => x.Code == code);

        public string? FindFullCode(long id)
        {
            return this.DbContext.Categorys.Where(x => x.Id == id).Select(x => x.FullCode).FirstOrDefault();
        }

        private Category[] FindCategorys(long? parentId) => [.. this.DbContext.Categorys.Where(x => x.ParentId == parentId)];

        private Category? FindCategory(long id) => this.DbContext.Categorys.FirstOrDefault(x => x.Id == id);

        public Category? FindCategory(long id, CategoryType categoryType) => this.DbContext.Categorys.FirstOrDefault(x => x.Id == id && x.CategoryType == categoryType);

        private string? FindName(string name) => this.DbContext.Categorys.Where(x => x.Name == name).Select(x => x.Name).FirstOrDefault();
    }
}
