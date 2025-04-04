﻿using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.RegisterUser
{
    public class AddEditUserCommands : IRequest<IResponse>
    {
        public string? Id {  get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string Cnic { get; set; }
        public string City { get; set; }
        public string RoleId { get; set; }
        public int? Fee { get; set; }
        public string? ProfilePicture { get; set; }


    }
}
