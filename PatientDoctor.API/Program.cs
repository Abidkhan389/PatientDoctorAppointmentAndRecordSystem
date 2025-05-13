using PatientDoctor.Infrastructure;
using PatientDoctor.Application;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Hangfire;
using PatientDoctor.Infrastructure.Repositories.ReminderSchedulers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

        //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;


    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Define the security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Make sure Swagger UI requires a Bearer token for authorization
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.UseHangfireDashboard();

// Use Hangfire Server to start processing jobs in the background
app.UseHangfireServer();

// Run reminder job once at application start
using (var scope = app.Services.CreateScope())
{
    var scheduler = scope.ServiceProvider.GetRequiredService<ReminderScheduler>();
    scheduler.ScheduleReminders();  // This will execute when the app starts
}

// Schedule the recurring job for 11:44 AM daily (you can modify the cron expression if needed)
//RecurringJob.AddOrUpdate<ReminderSchedulerJob>(
//    "daily-reminder-scheduler",
//    job => job.Execute(),  // Execute method from ReminderSchedulerJob
//    "01 12 * * *",  // Cron expression for 11:44 AM every day (modify if needed)
//    TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time")  // Ensure time zone is correct
//);

//RecurringJob.AddOrUpdate<ReminderScheduler>(
//    "daily-reminder-scheduler",
//    scheduler => scheduler.ScheduleReminders(),  // Make sure this is calling the correct method
//    "17 12 * * *",  // Cron expression for 11:44 AM daily
//    TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time")
//);

app.Run();

