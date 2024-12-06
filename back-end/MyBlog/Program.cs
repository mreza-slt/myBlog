using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBlog.Data;
using MyBlog.Models.DataModels;
using MyBlog.Plugins.Helpers;
using MyBlog.Plugins.Middlewares;
using MyBlog.Services;
using Swashbuckle.AspNetCore.Filters;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: myAllowSpecificOrigins,
        policy =>
                      {
                          policy.WithOrigins("http://localhost:3006")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

ConfigurationManager configuration = builder.Configuration;

// trim all input string
builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
     });

// Add IdentitContext and Change the default setting result variable in identity
builder.Services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<BlogDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = false;

    // User settings
    options.User.RequireUniqueEmail = false;
});

// Database connection configuration
builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ConfirmCodeService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<PostService>();

// Add services to the container.
builder.Services.AddControllers();

// set Custom setting Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "MyBlog";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.HttpOnly = false;
    options.SlidingExpiration = true;
    options.LoginPath = "/User/UnAuthorizedLogin";
    options.LogoutPath = "/User/Logout";
    options.AccessDeniedPath = "/api/User/UnAuthorized";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie.MaxAge = options.ExpireTimeSpan;
});

// swagger Settings----------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SoltaniWeblog Web API",
        Description = "لیست وب سرویس های وبلاگ",
    });

    // related to Documentions  /// <summary> /// </summary> in Controllers and Propertys and Configurated in MyBlog.Xml
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Show Auth on APIs
    options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(myAllowSpecificOrigins);

// middlewares Custom Exceptions
app.UseCustomExceptionHandler();

app.Run();
