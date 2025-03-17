using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;
using DAL.EFRepositories;
using DAL.AuthRepositories;
namespace DAL
{
     public static class ConfigurationExtensions
    {
        public static void ConfigureDAL(
            this IServiceCollection services, string connection)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
            services.AddScoped<IBookRepository, EFBookRepository>();
            services.AddScoped<IRepository<Author>, EFAuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
