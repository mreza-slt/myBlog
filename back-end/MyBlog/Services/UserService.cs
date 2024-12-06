using System.Net;
using Microsoft.AspNetCore.Identity;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.User;
using MyBlog.Plugins.Exceptions;
using MyBlog.Plugins.Extentions;

namespace MyBlog.Services
{
    public class UserService(
        BlogDbContext dbContext,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ImageService imageService,
        EmailService emailService,
        ConfirmCodeService confirmCodeService)
    {
        private BlogDbContext DbContext { get; } = dbContext;

        private ImageService ImageService { get; } = imageService;

        private EmailService EmailService { get; } = emailService;

        private ConfirmCodeService ConfirmCodeService { get; } = confirmCodeService;

        private readonly UserManager<User> userManager = userManager;

        private readonly SignInManager<User> signInManager = signInManager;

        /// <summary>
        /// مدیریت اطلاعات ورودی کاربر
        /// </summary>
        private Dictionary<string, string> ValidateUserInput(RegisterUserViewModel userModel)
        {
            Dictionary<string, string> errors = [];

            string? userName = this.FindUsername(userModel.UserName);
            if (!string.IsNullOrEmpty(userName))
            {
                errors.Add(nameof(RegisterUserViewModel.UserName), $"نام کاربری {userName} قبلا برای یک کاربر ثبت شده است");
            }

            string? email = this.FindEmail(userModel.Email);

            if (!string.IsNullOrEmpty(email))
            {
                email.IsValidEmail();
                errors.Add(nameof(RegisterUserViewModel.Email), $"ایمیل {email} قبلا برای یک کاربر ثبت شده است");
            }

            string? phoneNumber = this.FindPhoneNumber(userModel.PhoneNumber);
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                errors.Add(nameof(RegisterUserViewModel.PhoneNumber), $"شماره موبایل {phoneNumber} قبلا برای یک کاربر ثبت شده است");
            }

            return errors;
        }

        /// <summary>
        /// متد برای مدیریت خطاهای IdentityResult
        /// </summary>
        private void HandleIdentityErrors(IEnumerable<IdentityError> errors, string? phoneNumber)
        {
            foreach (var error in errors)
            {
                throw error.Code switch
                {
                    "InvalidUserName" => new HttpException(
                                                "نام کاربری را فقط با حروف و اعداد انگلیسی تکمیل کنید و از حروف و اعداد فارسی و علامت‌های نگارشی استفاده نکنید",
                                                nameof(RegisterUserViewModel.UserName),
                                                HttpStatusCode.BadRequest),
                    "PasswordTooShort" => new HttpException(
                                                "مقدار پسورد شما نباید کمتر از 6 کاراکتر باشد",
                                                nameof(RegisterUserViewModel.Password),
                                                HttpStatusCode.BadRequest),
                    "DuplicateUserName" => new HttpException(
                                                $"شماره موبایل {phoneNumber} که وارد کرده‌اید قبلا به عنوان نام کاربری یک کاربر ثبت شده است. لطفاً نام کاربری متفاوتی وارد کنید.",
                                                nameof(RegisterUserViewModel.UserName),
                                                HttpStatusCode.BadRequest),
                    _ => new Exception($"خطای ناشناخته: {error.Description}"),
                };
            }
        }

        public async Task<ResponseMessageViewModel> Register(RegisterUserViewModel userModel)
        {
            // جمع‌آوری خطاها
            Dictionary<string, string> errors = this.ValidateUserInput(userModel);
            if (errors.Count > 0)
                throw new HttpException(errors, HttpStatusCode.Conflict);

            using var transaction = await this.DbContext.Database.BeginTransactionAsync();

            User user = new(
                userModel.Title,
                userModel.Name,
                userModel.Surname,
                !string.IsNullOrEmpty(userModel.UserName) ? userModel.UserName : userModel.PhoneNumber,
                userModel.Email,
                userModel.PhoneNumber,
                userModel.Password);

            IdentityResult result = await this.userManager.CreateAsync(user, userModel.Password);

            // مدیریت خطاهای IdentityResult
            if (!result.Succeeded)
            {
                this.HandleIdentityErrors(result.Errors, user.PhoneNumber);
            }

            await transaction.CommitAsync();

            return new ResponseMessageViewModel(null, "ثبت نام با موفقیت انجام شد");
        }

        /// <summary>
        /// بررسی ورودی‌های ویرایش شده کاربر
        /// </summary>
        private void ValidateProfileInput(User user, ProfileUserViewModel userModel)
        {
            Dictionary<string, string> errors = [];

            if (string.IsNullOrEmpty(userModel.UserName) && string.IsNullOrEmpty(userModel.Email) && string.IsNullOrEmpty(userModel.PhoneNumber))
            {
                throw new HttpException(
                    "باید حداقل یکی از فیلدهای نام کاربری یا ایمیل یا شماره موبایل را وارد کنید",
                    $"{nameof(ProfileUserViewModel.UserName)},{nameof(ProfileUserViewModel.Email)},{nameof(ProfileUserViewModel.PhoneNumber)}",
                    HttpStatusCode.BadRequest);
            }

            if (!string.IsNullOrEmpty(userModel.UserName))
            {
                string? userName = this.FindUsername(userModel.UserName);
                if (!string.IsNullOrEmpty(userName) && userName != user.UserName)
                {
                    errors.Add(nameof(ProfileUserViewModel.UserName), $"نام کاربری {userName} قبلا برای یک کاربر ثبت شده است");
                }
            }

            if (!string.IsNullOrEmpty(userModel.Email))
            {
                string? email = this.FindEmail(userModel.Email);
                if (!string.IsNullOrEmpty(email) && email != user.Email)
                {
                    email.IsValidEmail();
                    errors.Add(nameof(ProfileUserViewModel.Email), $"ایمیل {email} قبلا برای یک کاربر ثبت شده است");
                }
            }

            if (!string.IsNullOrEmpty(userModel.PhoneNumber))
            {
                string? phoneNumber = this.FindPhoneNumber(userModel.PhoneNumber);
                if (phoneNumber != null && phoneNumber != user.PhoneNumber)
                {
                    errors.Add(nameof(ProfileUserViewModel.PhoneNumber), $"شماره موبایل {phoneNumber} قبلا برای یک کاربر ثبت شده است");
                }
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.Conflict);
            }
        }

        /// <summary>
        /// متد برای بررسی فرمت و تغییر اندازه تصویر
        /// </summary>
        private void ValidateImageFormat(string? avatar)
        {
            try
            {
                if (avatar != null)
                {
                    this.ImageService.FixImageSize(avatar, 300);
                }
            }
            catch (ArgumentException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(ProfileUserViewModel.Avatar), HttpStatusCode.BadRequest);
            }
            catch (FormatException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(ProfileUserViewModel.Avatar), HttpStatusCode.BadRequest);
            }
        }

        public async Task<ResponseMessageViewModel> Profile(long userId, ProfileUserViewModel userModel)
        {
            User user = this.FindUser(userId)!;

            this.ValidateProfileInput(user, userModel);

            this.ValidateImageFormat(userModel.Avatar);

            User.Copy(userModel, user);
            IdentityResult result = await this.userManager.UpdateAsync(user);

            this.HandleIdentityErrors(result.Errors, user.PhoneNumber);

            return new ResponseMessageViewModel(null, "اطلاعات با موفقیت ویرایش شد");
        }

        /// <summary>
        /// متد برای به‌روزرسانی اطلاعات ورود کاربر
        /// </summary>
        private void UpdateUserLoginInfo(User user)
        {
            user.LastLoginDateTime = user.LoginDateTime;
            user.LoginDateTime = DateTime.Now;
            user.AccessFailedCount = 0; // بازنشانی شمارنده تلاش‌های ناموفق
            user.LockoutEnabled = false; // غیرفعال کردن قفل حساب
        }

        /// <summary>
        ///   متد برای مدیریت تلاش‌های ورود ناموفق
        /// </summary>
        private async Task HandleFailedLoginAttempt(User user)
        {
            // اگر شمارنده تلاش‌های ناموفق 4 یا بیشتر باشد، حساب کاربر قفل می‌شود
            if (user.AccessFailedCount >= 4)
            {
                user.LockoutEnd = DateTime.Now.AddMinutes(5); // تنظیم زمان پایان قفل
                user.AccessFailedCount = 0; // بازنشانی شمارنده تلاش‌های ناموفق
                user.LockoutEnabled = true; // فعال کردن قفل حساب
            }
            else
            {
                user.AccessFailedCount++;
            }

            await this.DbContext.SaveChangesAsync();
        }

        public async Task<ProfileUserViewModel> Login(LoginViewModel userModel)
        {
            User? user = this.FindByUserNameOrEmailOrPhoneNumber(userModel.UserNameEmailPhone);
            if (user == null)
            {
                throw new HttpException(
                    $"هیچ کاربری با نام کاربری یا ایمیل یا شماره موبایل {userModel.UserNameEmailPhone} پیدا نشد",
                    nameof(LoginViewModel.UserNameEmailPhone),
                    HttpStatusCode.NotFound);
            }

            if (user.LockoutEnd >= DateTime.Now)
            {
                throw new HttpException("پنج بار تلاش ورود ناموفق. پنج دقیقه دیگر مجددا امتحان کنید", "", HttpStatusCode.Forbidden);
            }

            if (!await this.userManager.CheckPasswordAsync(user, userModel.Password))
            {
                await this.HandleFailedLoginAttempt(user);
                throw new HttpException("رمز عبور نادرست است", nameof(LoginViewModel.Password), HttpStatusCode.Unauthorized);
            }

            if (!user.LockoutEnabled || (user.LockoutEnabled && user.LockoutEnd.HasValue))
            {
                this.UpdateUserLoginInfo(user);
                this.DbContext.SaveChanges();
            }

            await this.signInManager.SignInAsync(user, true);

            return new ProfileUserViewModel
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber!,
                Title = user.Title,
                Avatar = user.Avatar,
            };
        }

        public async Task<ResponseMessageViewModel> Logout()
        {
            await this.signInManager.SignOutAsync();

            return new ResponseMessageViewModel(null, "شما از حساب کاربری خود خارج شدید");
        }

        public async Task<ResponseMessageViewModel> SendEmailConfirmCode(long userId)
        {
            User user = this.FindUser(userId)!;
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new HttpException("شما هنوز ایمیلی ثبت نکرده‌اید. لطفاً یک ایمیل جدید ثبت کنید", "", HttpStatusCode.BadRequest);
            }
            else
            {
                user.Email.IsValidEmail();
            }

            DateTime? lastCodeDate = this.ConfirmCodeService.FindLastConfirmCodeCreateDate(userId, CodeType.EmailConfirm);
            if (lastCodeDate != null && DateTime.Now < lastCodeDate.Value.AddMinutes(2))
            {
                throw new HttpException("لطفا بعد از دو دقیقه از دریافت کد قبلی مجددا تلاش کنید", "", HttpStatusCode.Forbidden);
            }

            int randomCode = new Random().Next(100000, 999999);

            using var transaction = await this.DbContext.Database.BeginTransactionAsync();

            ConfirmCode newConfirmCode = new(userId, randomCode, CodeType.EmailConfirm, DateTime.Now, DateTime.Now.AddHours(1));

            await this.DbContext.AddAsync(newConfirmCode);

            await this.DbContext.SaveChangesAsync();

            // ازسال ایمیل به کاربر
            string subject = "کد تایید ایمیل وبلاگ";
            string body = $"کد تایید : {randomCode}";

            await this.EmailService.SendEmail(user.Email, subject, body);

            await transaction.CommitAsync();

            return new ResponseMessageViewModel(null, "کد تایید به ایمیل ارسال شد");
        }

        public async Task<ResponseMessageViewModel> EmailConfirm(long userId, int receivedCode)
        {
            ConfirmCode? confirmCode = this.ConfirmCodeService.FindConfirmCode(userId, receivedCode, CodeType.EmailConfirm);

            if (confirmCode != null && confirmCode.ExpireDate > DateTime.Now)
            {
                User user = this.FindUser(userId)!;

                user.EmailConfirmed = true;

                await this.DbContext.SaveChangesAsync();

                return new ResponseMessageViewModel(null, "ایمیل شما تایید شد");
            }
            else
            {
                throw new HttpException(
                    "کد وارد شده نامعتبر یا اعتبار کد به پایان رسیده است. لطفا دوباره کد را ارسال کنید",
                    "",
                    HttpStatusCode.BadRequest);
            }
        }

        // datebase Methods
        private User? FindUser(long id) => this.DbContext.Users.FirstOrDefault(x => x.Id == id);

        private string? FindUsername(string? username) =>
            this.DbContext.Users.Where(x => !string.IsNullOrEmpty(x.UserName) && x.UserName == username)
                .Select(x => x.UserName).FirstOrDefault();

        private string? FindEmail(string? email) =>
            this.DbContext.Users.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email == email)
                .Select(x => x.Email).FirstOrDefault();

        private string? FindPhoneNumber(string phoneNumber) =>
             this.DbContext.Users.Where(x => x.PhoneNumber == phoneNumber)
                .Select(x => x.PhoneNumber).FirstOrDefault();

        private User? FindByUserNameOrEmailOrPhoneNumber(string userName)
        {
            if (!userName.StartsWith("09"))
            {
                if (userName.Contains('@'))
                {
                    return this.DbContext.Users.FirstOrDefault(x => x.Email == userName);
                }

                return this.DbContext.Users.FirstOrDefault(x => x.UserName == userName);
            }

            return this.DbContext.Users.FirstOrDefault(x => x.PhoneNumber == userName);
        }
    }
}
