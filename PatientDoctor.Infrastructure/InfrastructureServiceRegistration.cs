namespace PatientDoctor.Infrastructure;
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
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                         // THIS IS KEY: Map role claim type
                         RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                         NameClaimType = ClaimTypes.NameIdentifier

                     };
                 });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DoctorAssistantOrDoctor", policy =>
                    policy.RequireRole(Roles.DoctorAssistant, Roles.Doctor));
            });
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
        
    }

