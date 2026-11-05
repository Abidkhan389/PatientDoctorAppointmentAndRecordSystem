
namespace PatientDoctor.Infrastructure.Persistance;
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
        public virtual DbSet<MedicinePotency> MedicinePotency { get; set; } = null!;
        public virtual DbSet<DoctorCheckUpFeeDetails> DoctorCheckUpFeeDetails { get; set; } = null!;
        public virtual DbSet<PatientCheckedUpFeeHistroy> PatientCheckedUpFeeHistroy { get; set; } = null!;

        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<DoctorAvailabilities> DoctorAvailabilities { get; set; } = null!;

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<DoctorMedicines> DoctorMedicines { get; set; } = null!;
        public virtual DbSet<Prescription> Prescriptions { get; set; } = null!;
        public virtual DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; } = null!;
        public virtual DbSet<Attachments> Attachments { get; set; } = null!;
        public virtual DbSet<DoctorHolidays> DoctorHolidays { get; set; } = null!;
        public DbSet<GlobalExceptionLog> GlobalExceptionLogs { get; set; } = null!;
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public DbSet<UserLogin> UserLogin { get; set; } = null!;
        public DbSet<DoctorAssistant> DoctorAssistants { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Always keep this line first

            // Configure PrescriptionMedicine relationships with CASCADE
            builder.Entity<PrescriptionMedicine>()
                 .HasOne(pm => pm.Prescription)
                 .WithMany(p => p.Medicines)
                 .HasForeignKey(pm => pm.PrescriptionId)
                 .OnDelete(DeleteBehavior.Cascade); // ✅ Keep this Cascade

            builder.Entity<PrescriptionMedicine>()
                .HasOne(pm => pm.Medicine)
                .WithMany()
                .HasForeignKey(pm => pm.MedicineId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Change to Restrict or NoAction

            builder.Entity<PrescriptionMedicine>()
                .HasOne(pm => pm.MedicinePotency)
                .WithMany()
                .HasForeignKey(pm => pm.PotencyId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Change to Restrict or NoAction
            builder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.User)
                      .WithMany(x => x.Logins)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.Provider, x.ProviderKey })
                      .IsUnique();

                entity.Property(x => x.ProviderKey)
                      .IsRequired()
                      .HasMaxLength(200);
            });
            // DoctorAssistant Relation
            builder.Entity<DoctorAssistant>()
                .HasOne(da => da.Doctor)
                .WithMany(u=> u.Assistants)
                .HasForeignKey(da => da.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DoctorAssistant>()
                .HasOne(da => da.Assistant)
                .WithMany(u => u.AssignedDoctor)
                .HasForeignKey(da => da.AssistantId)
                .OnDelete(DeleteBehavior.Cascade);
            // Prevent Duplication Mapping
            builder.Entity<DoctorAssistant>()
                .HasIndex(da => new { da.DoctorId, da.AssistantId })
                .IsUnique();
            builder.Entity<Userdetail>()
                .HasOne(ud => ud.User)
                .WithOne(u => u.UserDetails)
                .HasForeignKey<Userdetail>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }

