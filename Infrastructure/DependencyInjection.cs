using Application.Auth;
using Application.Files;
using Infrastructure.Auth;
using Infrastructure.Files;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IPasswordHasher), typeof(PasswordHasher));
            services.AddScoped(typeof(IJwtTokenService), typeof(JwtTokenService));
            services.AddScoped(typeof(ICurrentUser), typeof(CurrentUser));
            services.AddScoped(typeof(IFileStorage), typeof(LocalFileStorage));

            return services;
        }
    }
}
