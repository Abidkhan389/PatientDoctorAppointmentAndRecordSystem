using Hangfire;
using PatientDoctor.Application.Contracts.Persistance.ReminderService;
using PatientDoctor.Infrastructure.Persistance;

namespace PatientDoctor.Infrastructure.Repositories.ReminderSchedulers;

public class ReminderScheduler
{
    private readonly DocterPatiendDbContext _context;

    public ReminderScheduler(DocterPatiendDbContext context)
    {
        _context = context;
    }

    public void ScheduleReminders()
    {
        var today = DateTime.Now.Date;

        // 🔹 Fetch Upcoming Appointments (Jo 2 Din Baad Hain Aur Notify Nahi Huay)
        var upcomingAppointments = _context.Appointment
            .Where(a => a.AppointmentDate.Date == today.AddDays(2) && !a.IsNotified && !a.CheckUpStatus)
            .ToList();
        foreach (var appointment in upcomingAppointments)
        {
            var reminderMessage = $"Reminder: Your appointment is on {appointment.AppointmentDate:dddd, MMM dd yyyy}. And  {appointment.TimeSlot}";

            // 🔹 Calculate the TimeSpan for scheduling the reminder (Appointment Date - 2 Days - Current Time)
            var reminderTime = appointment.AppointmentDate.AddDays(-2); // 2 days before the appointment date
            var delay = reminderTime - DateTime.Now; // Delay until the reminder time

            if (delay > TimeSpan.Zero)
            {
                // 🔹 Hangfire Job Schedule Karega (Appointment ID Send Karna Zaroori Hai)
                BackgroundJob.Schedule<ReminderService>(
                    x => x.SendReminderSmsAsync(appointment.AppointmentId,reminderMessage),
                    delay
                );

                //Console.WriteLine($"✅ Reminder scheduled for {appointment.Email} on {reminderTime}.");
            }
            else
            {
                // If the reminder time has already passed (for some reason), we don't schedule the job.
                //Console.WriteLine($"❌ Cannot schedule reminder for {appointment.Email}, the reminder time has passed.");
            }
        }
    }
}
