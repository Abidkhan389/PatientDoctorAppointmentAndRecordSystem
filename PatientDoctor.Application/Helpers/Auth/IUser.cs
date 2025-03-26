using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Helpers.Auth;
  public  interface IUser
    {
    string UserId { get; }
    string Username { get; }
    string CultureName { get; }
    string DisplayName { get; }
    string Email { get; }
    string TimeZoneId { get; }
    bool Enabled { get; set; }
}

