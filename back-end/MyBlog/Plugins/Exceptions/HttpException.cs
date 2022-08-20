using System.Dynamic;
using System.Net;
using MyBlog.Plugins.Extentions;

namespace MyBlog.Plugins.Exceptions
{
    public class HttpException : Exception
    {
        private const string GeneralValue = "General";

        public dynamic? Messages { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public HttpException(string message, string field, HttpStatusCode httpStatusCode)
       : base()
        {
            dynamic errors = new ExpandoObject();
            ((IDictionary<string, object>)errors)[field.ReplaceWithValueIfIsNull(GeneralValue)] = new[] { message };
            this.Messages = errors;
            this.HttpStatusCode = httpStatusCode;
        }

        public HttpException(Dictionary<string, string> messages, HttpStatusCode httpStatusCode)
        : base()
        {
            dynamic errors = new ExpandoObject();
            foreach (var item in messages)
            {
                ((IDictionary<string, object>)errors)[item.Key.ReplaceWithValueIfIsNull(GeneralValue)] = new string[] { item.Value };
            }

            this.Messages = errors;
            this.HttpStatusCode = httpStatusCode;
        }
    }
}
