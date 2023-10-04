using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PatientDoctor.domain.Entities;
using System.Reflection;
using PatientDoctor.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Infrastructure.Repositories.Identity;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Infrastructure.Repositories.Patient;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DocterPatiendDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("PatientDoctorDbConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("PatientDoctor.Migrations")
                );
            });
            //services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            //{
            //    Options.Password.RequiredLength = 4;
            //    Options.Password.RequireNonAlphanumeric = false;
            //    Options.Password.RequiredUniqueChars = 3;
            //    Options.Password.RequireDigit = false;
            //    Options.Password.RequireLowercase = false;
            //    Options.Password.RequireUppercase = false;

            //    //Options.SignIn.RequireConfirmedEmail = true;
            //}).AddEntityFrameworkStores<DocterPatiendDbContext>()
            //.AddDefaultTokenProviders();
            // services.AddCors(options => options.AddPolicy("CorsPolicy",
            //builder =>
            //{
            //    builder.AllowAnyHeader()
            //           .AllowAnyMethod()
            //           .SetIsOriginAllowed((host) => true)
            //           .AllowCredentials();
            //}));
            services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             //Adding JWT Bearer 
             .AddJwtBearer(options =>
             {
                 //options.SaveToken = false;
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     //ValidateIssuer = false,
                     //ValidateAudience = false, 
                     ValidAudience = configuration["JWT:ValidAudience"],
                     ValidIssuer = configuration["JWT:ValidIssuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                 };
             })
             .AddCookie();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IResponse, Response>();
            return services;
        }

    }
}
