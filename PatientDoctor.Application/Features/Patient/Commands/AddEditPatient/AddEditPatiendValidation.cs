using FluentValidation;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.AddEditPatient
{
    public class AddEditPatiendValidation : AbstractValidator<AddEditPatientCommand>
    {
        public AddEditPatiendValidation()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("{FirstName} is required.")
                .MaximumLength(30).WithMessage("{FirstName} must not exceed 30 characters.")
                .MinimumLength(3).WithMessage("{FirstName} must be at least 3 characters long.");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("{LastName} is required.")
                .MaximumLength(30).WithMessage("{LastName} must not exceed 30 characters.")
                .MinimumLength(3).WithMessage("{LastName} must be at least 3 characters long.");

            RuleFor(c => c.Gender)
                .NotEmpty().WithMessage("{Gender} is required.")
                .Length(4).WithMessage("{Gender} must be exactly 4 characters long.");

            RuleFor(c => c.DoctoerId)
                .NotEmpty().WithMessage("{DoctoerId} is required.");

            RuleFor(c => c.DateofBirth)
                .NotEmpty().WithMessage("{DateofBirth} is required.");

            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage("{PhoneNumber} is required.")
                .Length(11).WithMessage("{PhoneNumber} must be exactly 11 characters long.");

            RuleFor(c => c.City)
                .NotEmpty().WithMessage("{City} is required.")
                .Length(4).WithMessage("{City} must be exactly 4 characters long.");

            RuleFor(c => c.BloodType)
                .NotEmpty().WithMessage("{BloodType} is required.");

            RuleFor(c => c.Cnic)
                .NotEmpty().WithMessage("{Cnic} is required.")
                .Length(13).WithMessage("{Cnic} must be exactly 13 characters long.");

            RuleFor(c => c.MaritalStatus)
                .NotEmpty().WithMessage("{MaritalStatus} is required.");
        }
    }
}
