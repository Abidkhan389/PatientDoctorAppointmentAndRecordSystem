using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Infrastructure.Persistance
{
    public class DocterPatiendDbContext: IdentityDbContext<ApplicationUser>
    {
        public DocterPatiendDbContext(DbContextOptions<DocterPatiendDbContext> options)
           : base(options)
        {
        }
        public virtual DbSet<Userdetail> Userdetail { get; set; } = null!;
        public virtual DbSet<Patient> Patient { get; set; } = null!;
        public virtual DbSet<PatientDetails> PatientDetails { get; set; } = null!;
        public virtual DbSet<Appointment> Appointment { get; set; } = null!;
        public virtual DbSet<Medicine> Medicine { get; set; } = null!;
        public virtual DbSet<MedicineType> MedicineType { get; set; } = null!;


    }
}
