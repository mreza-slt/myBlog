using System.Net;
using Microsoft.AspNetCore.Identity;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Models.Enums;
using MyBlog.Models.ViewModels;
using MyBlog.Models.ViewModels.User;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class UserService
    {
        public UserService(
            BlogDbContext dbContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ImageService imageService,
            EmailService emailService,
            ConfirmCodeService confirmCodeService)
        {
            this.DbContext = dbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.ImageService = imageService;
            this.EmailService = emailService;
            this.ConfirmCodeService = confirmCodeService;
        }

        private BlogDbContext DbContext { get; }

        private ImageService ImageService { get; }

        private EmailService EmailService { get; }

        private ConfirmCodeService ConfirmCodeService { get; }

        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        public async Task<ResponseMessageViewModel> Register(RegisterUserViewModel userModel)
        {
            Dictionary<string, string> errors = new();

            string? userName = this.FindUsername(userModel.UserName);
            if (!string.IsNullOrEmpty(userName))
            {
                errors.Add(nameof(RegisterUserViewModel.UserName), $"نام کاربری {userName} قبلا برای یک کاربر ثبت شده است");
            }

            string? email = this.FindEmail(userModel.Email);
            if (!string.IsNullOrEmpty(email))
            {
                errors.Add(nameof(RegisterUserViewModel.Email), $"ایمیل {email} قبلا برای یک کاربر ثبت شده است");
            }

            string? phoneNumber = this.FindPhoneNumber(userModel.PhoneNumber);
            if (phoneNumber != null)
            {
                errors.Add(nameof(RegisterUserViewModel.PhoneNumber), $"شماره موبایل {phoneNumber} قبلا برای یک کاربر ثبت شده است");
            }

            if (errors.Count > 0)
            {
                throw new HttpException(errors, HttpStatusCode.Conflict);
            }

            var transaction = await this.DbContext.Database.BeginTransactionAsync();

            User user = new(
                userModel.Title,
                userModel.Name,
                userModel.Surname,
                !string.IsNullOrEmpty(userModel.UserName) ? userModel.UserName : userModel.PhoneNumber,
                userModel.Email,
                userModel.PhoneNumber,
                userModel.Password);

            IdentityResult result = await this.userManager.CreateAsync(user, userModel.Password);
            foreach (var error in result.Errors)
            {
                if (error.Code == "InvalidUserName")
                {
                    throw new HttpException("نام کاربری را فقط با حروف و اعداد انگلیسی تکمیل کنید و از حروف و اعداد فارسی و علامت های نگارشی استفاده نکنید", nameof(RegisterUserViewModel.UserName), HttpStatusCode.BadRequest);
                }
                else if (error.Code == "PasswordTooShort")
                {
                    throw new HttpException("مقدار پسورد شما نباید کمتر از 6 کاراکتر باشد", nameof(RegisterUserViewModel.Password), HttpStatusCode.BadRequest);
                }
                else if (error.Code == "DuplicateUserName")
                {
                    throw new HttpException($"شماره موبایل {user.PhoneNumber} که وارد کرده اید قبلا به عنوان نام کاربری یک کاربر ثبت شده است، و به همین دلیل باید مقدار نام کاربری را وارد کنید", nameof(RegisterUserViewModel.UserName), HttpStatusCode.BadRequest);
                }

                throw new Exception();
            }

            await transaction.CommitAsync();

            return new ResponseMessageViewModel(null, "ثبت نام با موفقیت انجام شد");
        }

        public async Task<ResponseMessageViewModel> Profile(long userId, ProfileUserViewModel userModel)
        {
            User user = this.FindUser(userId)!;

            Dictionary<string, string> errors = new();

            if (!string.IsNullOrEmpty(userModel.UserName) || !string.IsNullOrEmpty(userModel.Email) || !string.IsNullOrEmpty(userModel.PhoneNumber))
            {
                string? userName = this.FindUsername(userModel.UserName);
                if (!string.IsNullOrEmpty(userName) && userName != user.UserName)
                {
                    errors.Add(nameof(ProfileUserViewModel.UserName), $"نام کاربری {userName} قبلا برای یک کاربر ثبت شده است");
                }

                string? email = this.FindEmail(userModel.Email);
                if (!string.IsNullOrEmpty(email) && email != user.Email)
                {
                    errors.Add(nameof(ProfileUserViewModel.Email), $"ایمیل {email} قبلا برای یک کاربر ثبت شده است");
                }

                string? phoneNumber = this.FindPhoneNumber(userModel.PhoneNumber);
                if (phoneNumber != null && phoneNumber != user.PhoneNumber)
                {
                    errors.Add(nameof(ProfileUserViewModel.PhoneNumber), $"شماره موبایل {phoneNumber} قبلا برای یک کاربر ثبت شده است");
                }

                if (errors.Count > 0)
                {
                    throw new HttpException(errors, HttpStatusCode.Conflict);
                }
            }
            else
            {
                throw new HttpException("باید حداقل یکی از فیلدهای نام کاربری یا ایمیل یا شماره موبایل را وارد کنید", $"{nameof(ProfileUserViewModel.UserName)},{nameof(ProfileUserViewModel.Email)},{nameof(ProfileUserViewModel.PhoneNumber)}", HttpStatusCode.BadRequest);
            }

            // Check format and fix size Image
            try
            {
                this.ImageService.FixImageSize(userModel.Avatar, 300);
            }
            catch (ArgumentException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(ProfileUserViewModel.Avatar), HttpStatusCode.BadRequest);
            }
            catch (FormatException)
            {
                throw new HttpException("فرمت عکس صحیح نیست", nameof(ProfileUserViewModel.Avatar), HttpStatusCode.BadRequest);
            }

            // Update user with Internal in model
            User.Copy(userModel, user);

            IdentityResult result = await this.userManager.UpdateAsync(user);
            foreach (var error in result.Errors)
            {
                if (error.Code == "InvalidUserName")
                {
                    throw new HttpException("نام کاربری را فقط با حروف و اعداد انگلیسی تکمیل کنید و از حروف و اعداد فارسی و علامت های نگارشی استفاده نکنید", nameof(ProfileUserViewModel.UserName), HttpStatusCode.BadRequest);
                }
                else if (error.Code == "DuplicateUserName")
                {
                    throw new HttpException($"شماره موبایل {user.PhoneNumber} که وارد کرده اید قبلا به عنوان نام کاربری یک کاربر ثبت شده است، و به همین دلیل باید مقدار نام کاربری را وارد کنید", nameof(RegisterUserViewModel.UserName), HttpStatusCode.BadRequest);
                }

                throw new Exception();
            }

            return new ResponseMessageViewModel(null, "اطلاعات با موفقیت ویرایش شد");
        }

        public async Task<ProfileUserViewModel> Login(LoginViewModel userModel)
        {
            User? user = this.FindByUserNameOrEmailOrPhoneNumber(userModel.UserNameEmailPhone);

            try
            {
                if (user == null)
                {
                    throw new HttpException($"هیچ کاربری با نام کاربری یا ایمیل یا شماره موبایل {userModel.UserNameEmailPhone} پیدا نشد", nameof(LoginViewModel.UserNameEmailPhone), HttpStatusCode.NotFound);
                }

                if (user.LockoutEnd >= DateTime.Now)
                {
                    throw new HttpException("پنج بار تلاش ورود ناموفق. پنج دقیقه دیگر مجددا امتحان کنید", "", HttpStatusCode.Forbidden);
                }

                if (!await this.userManager.CheckPasswordAsync(user, userModel.Password))
                {
                    throw new HttpException("رمز عبور نادرست است", nameof(LoginViewModel.Password), HttpStatusCode.Unauthorized);
                }

                if (!user.LockoutEnabled || (user.LockoutEnabled && user.LockoutEnd.HasValue))
                {
                    // save loginDatetime and last loginDateTime
                    user.LastLoginDateTime = user.LoginDateTime;
                    user.LoginDateTime = DateTime.Now;

                    // reset accessFailedCount
                    user.AccessFailedCount = 0;
                    user.LockoutEnabled = false;

                    this.DbContext.SaveChanges();
                }
            }
            catch (HttpException error)
            {
                if (user != null && error.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    if (user.AccessFailedCount >= 4)
                    {
                        DateTime currentTime = DateTime.Now;
                        user.LockoutEnd = currentTime.AddMinutes(5);
                        user.AccessFailedCount = 0;
                        user.LockoutEnabled = true;
                    }
                    else
                    {
                        user.AccessFailedCount += 1;
                    }

                    this.DbContext.SaveChanges();
                }

                throw error;
            }
            catch (Exception)
            {
                throw;
            }

            await this.signInManager.SignInAsync(user, true);

            return new ProfileUserViewModel() { UserName = user.UserName, Name = user.Name, Surname = user.Surname, Email = user.Email, PhoneNumber = user.PhoneNumber, Title = user.Title, Avatar = user.Avatar };
        }

        public async Task<ResponseMessageViewModel> Logout()
        {
            await this.signInManager.SignOutAsync();

            return new ResponseMessageViewModel(null, "شما از حساب کاربری خود خارج شدید");
        }

        public async Task<ResponseMessageViewModel> SendEmailConfirmCode(long userId)
        {
            DateTime? createDate = this.ConfirmCodeService.FindLastConfirmCodeCreateDate(userId, CodeType.EmailConfirm);
            if (createDate != null && DateTime.Now < createDate.Value.AddMinutes(2))
            {
                throw new HttpException("لطفا بعد از دو دقیقه از دریافت کد قبلی مجددا تلاش کنید", "", HttpStatusCode.Forbidden);
            }

            User user = this.FindUser(userId)!;

            Random rd = new();
            int randomCode = rd.Next(100000, 999999);

            using var transaction = await this.DbContext.Database.BeginTransactionAsync();

            ConfirmCode newConfirmCode = new(userId, randomCode, CodeType.EmailConfirm, DateTime.Now, DateTime.Now.AddHours(1));

            await this.DbContext.AddAsync(newConfirmCode);

            await this.DbContext.SaveChangesAsync();

            // For text and email subject
            string subject = "کد تایید ایمیل وبلاگ";
            string body = $"کد تایید : {randomCode}";

            await this.EmailService.SendEmail(user.Email, subject, body);

            await transaction.CommitAsync();

            return new ResponseMessageViewModel(null, "کد تایید به ایمیل ارسال شد");
        }

        public async Task<ResponseMessageViewModel> EmailConfirm(long userId, int recivedCode)
        {
            ConfirmCode? confirmCode = this.ConfirmCodeService.FindConfirmCode(userId, recivedCode, CodeType.EmailConfirm);

            if (confirmCode != null && confirmCode.ExpireDate > DateTime.Now)
            {
                User? updateUser = this.FindUser(userId)!;

                updateUser.EmailConfirmed = true;

                await this.DbContext.SaveChangesAsync();

                return new ResponseMessageViewModel(null, "ایمیل شما تایید شد");
            }
            else
            {
                throw new HttpException("کد وارد شده نامعتبر یا اعتبار کد به پایان رسیده است. لطفا دوباره کد را ارسال کنید", "", HttpStatusCode.BadRequest);
            }
        }

        // datebase Methods
        public User? FindUser(long id)
        {
            return this.DbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public string? FindUsername(string? username)
        {
            return this.DbContext.Users.Where(x => !string.IsNullOrEmpty(x.UserName) && x.UserName == username)
                .Select(x => x.UserName).FirstOrDefault();
        }

        public string? FindEmail(string? email)
        {
            return this.DbContext.Users.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email == email)
                .Select(x => x.Email).FirstOrDefault();
        }

        public string? FindPhoneNumber(string phoneNumber)
        {
            return this.DbContext.Users.Where(x => x.PhoneNumber == phoneNumber)
                .Select(x => x.PhoneNumber).FirstOrDefault();
        }

        public User? FindByUserNameOrEmailOrPhoneNumber(string userName)
        {
            if (userName.StartsWith("09"))
            {
                return this.DbContext.Users.FirstOrDefault(x => x.PhoneNumber == userName);
            }

            if (userName.Contains('@'))
            {
                return this.DbContext.Users.FirstOrDefault(x => x.Email == userName);
            }

            return this.DbContext.Users.FirstOrDefault(x => x.UserName == userName);
        }
    }
}
