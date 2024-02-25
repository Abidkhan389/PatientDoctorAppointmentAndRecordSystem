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
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Infrastructure.Repositories.CryptoService;
using System.Security.Cryptography;
using PatientDoctor.Application.Contracts.Persistance.Dashboard;
using PatientDoctor.Infrastructure.Repositories.Dashboard;
using PatientDoctor.Application.Contracts.Persistance.ISecurity;
using PatientDoctor.Infrastructure.Repositories.SecurityRepository;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Infrastructure.Repositories.Administrator;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Infrastructure.Repositories.MedicineType;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Infrastructure.Repositories.Medicine;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Infrastructure.Repositories.DoctorFeeCheckUpFee;

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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                //Options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<DocterPatiendDbContext>()
               .AddDefaultTokenProviders();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
           builder =>
           {
               builder.AllowAnyHeader()
                      .AllowAnyMethod()
                      .SetIsOriginAllowed((host) => true)
                      .AllowCredentials();
           }));
            var jwtSecretKey = GenerateJwtSecretKey();
            configuration["JWT:Secret"] = jwtSecretKey;
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
            services.AddScoped<ICountResponse, CountResponse>(); 
            services.AddScoped<ICryptoService, CryptoHelper>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ILocalAuthenticationRepository, UserRepository>();
            services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<IMedicinetypeRepository, MedicineTypeRepository>();
            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<IDoctorCheckUpFeeRepository, DoctorCheckUpFeeRepository>();
            return services;
        }
        private static string GenerateJwtSecretKey()
        {
            var keyBytes = new byte[32]; // 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }
            return Convert.ToBase64String(keyBytes);
        }

    }
}
