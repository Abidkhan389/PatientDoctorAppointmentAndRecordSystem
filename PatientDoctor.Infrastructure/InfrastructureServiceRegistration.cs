using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PatientDoctor.Application.Contracts.Persistance.Dashboard;
using PatientDoctor.Application.Contracts.Persistance.GoogeLogin;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Contracts.Persistance.IDoctorAvailability;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
using PatientDoctor.Application.Contracts.Persistance.IDoctorMedicine;
using PatientDoctor.Application.Contracts.Persistance.IEmail;
using PatientDoctor.Application.Contracts.Persistance.IException;
using PatientDoctor.Application.Contracts.Persistance.IFileRepository;
using PatientDoctor.Application.Contracts.Persistance.IFileStorage;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Contracts.Persistance.IReminderServices;
using PatientDoctor.Application.Contracts.Persistance.IReports;
using PatientDoctor.Application.Contracts.Persistance.ISecurity;
using PatientDoctor.Application.Contracts.Persistance.ISmsRepository;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Contracts.Persistance.ReminderService;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Email;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.Administrator;
using PatientDoctor.Infrastructure.Repositories.CryptoService;
using PatientDoctor.Infrastructure.Repositories.Dashboard;
using PatientDoctor.Infrastructure.Repositories.DoctorAvailability;
using PatientDoctor.Infrastructure.Repositories.DoctorFeeCheckUpFee;
using PatientDoctor.Infrastructure.Repositories.DoctorHoliday;
using PatientDoctor.Infrastructure.Repositories.DoctorMedicine;
using PatientDoctor.Infrastructure.Repositories.Email;
using PatientDoctor.Infrastructure.Repositories.ExceptionLog;
using PatientDoctor.Infrastructure.Repositories.FileSystemStorage;
using PatientDoctor.Infrastructure.Repositories.FileUploaders;
using PatientDoctor.Infrastructure.Repositories.GoogleLogin;
using PatientDoctor.Infrastructure.Repositories.Identity;
using PatientDoctor.Infrastructure.Repositories.Medicine;
using PatientDoctor.Infrastructure.Repositories.MedicineType;
using PatientDoctor.Infrastructure.Repositories.Patient;
using PatientDoctor.Infrastructure.Repositories.PatientCheckUpHistroy;
using PatientDoctor.Infrastructure.Repositories.ReminderSchedulers;
using PatientDoctor.Infrastructure.Repositories.Reports;
using PatientDoctor.Infrastructure.Repositories.SecurityRepository;
using PatientDoctor.Infrastructure.Repositories.SmsRepository;
using PatientDoctor.Infrastructure.Utalities.ExceptionLoggers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
namespace PatientDoctor.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), configuration.GetValue<string>("FileStorageRootPath"));

            services.AddDbContextPool<DocterPatiendDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("PatientDoctorDbConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("PatientDoctor.Migrations")
                );
            });
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))); // Using HangfireConnection from appsettings.json
            services.AddHangfireServer();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

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
            //var jwtSecretKey = GenerateJwtSecretKey();
            //configuration["JWT:Secret"] = jwtSecretKey;
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
                     ValidateLifetime = true,            
                     ValidateIssuerSigningKey = true,
                     ClockSkew = TimeSpan.Zero, 
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
            services.AddScoped<IFileUploader, FileUploaderRepository>();
            services.AddScoped<IFileStorageRepository>(provider => new FileSystemStorageRepository(rootPath));

            services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<IMedicinetypeRepository, MedicineTypeRepository>();
            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<IDoctorCheckUpFeeRepository, DoctorCheckUpFeeRepository>();
            services.AddScoped<IDoctorAvailabilityRepository, DoctorAvailabilityRepository>();
            services.AddScoped<IDoctorMedicineRepository, DoctorMedicineRepository>();
            services.AddScoped<IPatientCheckUpHistroyRepository, PatientCheckUpHistroyRepository>();
            services.AddScoped<IDoctorHolidayRepository, DoctorHolidayRepository>();
            services.AddScoped<IPatientAppointmentSmsRepository, PatientAppointmentSmsRepository>();
            
            services.AddScoped<IReminderService, ReminderService>();
            services.AddScoped<IReports, ReportsRepository>();

            services.AddScoped<ReminderScheduler>();
            // Configure Email Settings
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // Register Email Service
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
            services.AddSingleton<IExceptionLogger, ExceptionLogger>();
            services.AddScoped<IGoogleTokenValidator, GoogleLoginRepository>();
            services.AddProblemDetails();
            services.AddAuthorization();
            services.AddHttpClient();
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
