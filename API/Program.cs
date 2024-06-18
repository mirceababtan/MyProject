using API.Infrastructure.Database;
using API.Manager.Authentication;
using API.Manager.Authentication.Contract;
using API.Manager.User;
using API.Manager.User.Contract;
using API.Resource.Authentication;
using API.Resource.Authentication.Contract;
using API.Resource.User;
using API.Resource.User.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using API.Manager.Course.Contract;
using API.Manager.Course;
using API.Resource.Course.Contract;
using API.Resource.Course;
using API.Manager.Course.Lesson.Contract;
using API.Manager.Course.Lesson;
using API.Resource.Course.Lesson;
using API.Resource.Course.Lesson.Contract;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                    };
                });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddScoped<ICourseManager, CourseManager>();
            builder.Services.AddScoped<ILessonManager,LessonManager>();
            
            builder.Services.AddScoped<IUserResource, UserResource>();
            builder.Services.AddScoped<IAuthResource, AuthResource>();
            builder.Services.AddScoped<ICourseResource,CourseResource>();
            builder.Services.AddScoped<ILessonResource, LessonResource>();

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
