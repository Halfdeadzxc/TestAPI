using BLL.ServicesInterfaces;
using Microsoft.Extensions.DependencyInjection;
using DAL;
using BLL.Profiles;
using BLL.ServicesInterfaces;
using BLL.DTO;
using BLL.Services;
using BLL.Validators;
using FluentValidation;
namespace BLL
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureBLL(this IServiceCollection services, string connection)
        {
            services.ConfigureDAL(connection);
            services.AddAutoMapper(
                typeof(AuthorProfile),
                typeof(BookProfile),
                typeof(UserProfile),
                typeof(RefreshTokenProfile)
            );
            services
                .AddScoped<IBookService, BookService>();
            services
                .AddScoped<IService<AuthorDTO>, AuthorService>();
            services
                .AddScoped<IUserService, UserService>();
            services
                .AddScoped<JwtService>();
            services.
                AddScoped<PasswordService>();
            services
                .AddTransient<IValidator<BookDTO>, BookDTOValidator>();
            services
                .AddTransient<IValidator<AuthorDTO>, AuthorDTOValidator>();
            services
                .AddTransient<IValidator<UserDTO>, UserDTOValidator>();
            services
                .AddTransient<IValidator<RefreshTokenDTO>, RefreshTokenDTOValidator>();
        }
    }
}
