﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    [Table("Appointment", Schema = "Admin")]
    public class Appointment
    {
        [Key]
        public Guid AppointmentId { get; set; }

        public Guid PatientId { get; set; } // Foreign key to reference the patient.

        public string DoctorId { get; set; } // Foreign key to reference the doctor.
        public int DoctorFee { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public int PatientCheckUpDayId { get; set; }
        public Guid PatientDetailsId { get; set; }
        public bool CheckUpStatus { get; set; }
        public bool IsNotified { get; set; }
    }

}
