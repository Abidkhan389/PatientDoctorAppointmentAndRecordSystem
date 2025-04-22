using Microsoft.Extensions.Configuration;
using PatientDoctor.Application.Contracts.Persistance.ISmsRepository;
using PatientDoctor.Application.Helpers.AppointmentSms;
using Newtonsoft.Json;
using System.Text;

namespace PatientDoctor.Infrastructure.Repositories.SmsRepository;
public class PatientAppointmentSmsRepository(IHttpClientFactory _httpClient, IConfiguration _configuration) : IPatientAppointmentSmsRepository
{
    public async Task<bool> SendSmsAsync(PatientAppointmentSmsRequest Model)
    {
        var apiUrl = _configuration["JazzSms:ApiUrl"];
        var apiKey = _configuration["JazzSms:ApiKey"];
        var senderPhoneNumber = _configuration["JazzSms:SenderId"];

        var payload = new
        {
            to = Model.PatientMobileNumber,
            from = senderPhoneNumber,
            text = $"Dear Patient, your appointment with Dr. {Model.DoctorName} is confirmed on {Model.AppointmentDate.Date:dd MMM yyyy} and time is {Model.TimeSlot}."
        };

        // Create an HttpClient from the factory
        var client = _httpClient.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}


