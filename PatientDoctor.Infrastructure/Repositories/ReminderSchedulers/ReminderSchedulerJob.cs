
namespace PatientDoctor.Infrastructure.Repositories.ReminderSchedulers;

public class ReminderSchedulerJob
{
    private readonly IServiceProvider _serviceProvider;

    public ReminderSchedulerJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Execute()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var reminderScheduler = scope.ServiceProvider.GetRequiredService<ReminderScheduler>();
            reminderScheduler.ScheduleReminders();
        }
    }
}
