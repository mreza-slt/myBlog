using System.Dynamic;
using System.Net;
using MyBlog.Models.ViewModels;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Plugins.Middlewares
{
    public static class ExceptionHandler
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // Do work that can write to the Response.
                try
                {
                    await next.Invoke();
                }
                catch (HttpException ex)
                {
                    dynamic errors = new ExpandoObject();
                    if (ex.Messages != null)
                    {
                        errors = ex.Messages;
                    }

                    // #if DEBUG
                    //                    errors.ExceptionMessage = ex.Message;
                    //                    errors.StackTrace = ex.StackTrace;
                    //                    errors.Source = ex.Source;
                    // #endif
                    var errorResponse = new ResponseMessageViewModel(errors, "");
                    context.Response.StatusCode = (int)ex.HttpStatusCode;
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
                catch (Exception)
                {
                    dynamic errors = new ExpandoObject();
                    // #if DEBUG
                    //                    errors.ExceptionMessage = ex.Message;
                    //                    errors.StackTrace = ex.StackTrace;
                    //                    errors.Source = ex.Source;
                    // #endif
                    errors.General = "خطا در اجرای درخواست شما.";

                    var errorResponse = new ResponseMessageViewModel(errors, "");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            });
        }
    }
}
