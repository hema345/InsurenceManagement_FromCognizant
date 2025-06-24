using InsurenceManagementSystemWebApi.Domain.Models;
using InsurenceManagementSystemWebApi.Infrastructure.Persistance.Configurations;

namespace InsurenceManagementSystemWebApi.Infrastructure.Persistance
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AvailablePolicy> AvailablePolicies { get; set; }
        public DbSet<PolicyRequest> PolicyRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role {Id =1, Name = "Admin" },
                new Role {Id =2, Name = "Customer" },
                new Role {Id=3 , Name = "Agent" }
                );
            modelBuilder.Entity<User>().HasData(
                new {
                    Id = new Guid("8f359361-c940-4177-83e0-b8ef6162cb3d"),
                    Username = "Rathna_2004",
                    // This PasswordHash is already a static, hardcoded string, which is correct.
                    PasswordHash = "$2a$11$G0em2dWFNc.OoC1lpOyu6eeCLM/tmMEg/uNOtXl66AKZUiq1.JNOa",
                    CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc)

                }

                );

            modelBuilder.Entity<UserRole>().HasData(
                new
                {
                    UserId = new Guid("8f359361-c940-4177-83e0-b8ef6162cb3d"), // This Guid is already hardcoded, which is correct.
                    RoleId = 2,
                    CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) // Hardcoded date and time
                });
            modelBuilder.Entity<Customer>().HasData(
     new 
     {
         CustomerId = 1,
         Name = "M V RATHNA REDDY",
         DateOfBirth = new DateTime(2004, 8, 5, 3, 52, 8, 233),
         Email = "rathna@gmail.com",
         Phone = "9381615847",
         Address = "NARASARAOPET",
         UserId = new Guid("8F359361-C940-4177-83E0-B8EF6162CB3D"),
         CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc)
     }
 );
            modelBuilder.Entity<User>().HasData(
              new  {
                  Id = new Guid("8180c0f7-1181-4ac8-88a4-5e3deb5d62e1"),
                  Username = "Seethayya_1980",
                  // This PasswordHash is already a static, hardcoded string, which is correct.
                  PasswordHash = "$2a$11$tpeUA3AslEmNSMZ8Bq9iHu1AXljxOSNtjdOoAC41BH1Ty7kToOCTC",
                  CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc)

              });
            modelBuilder.Entity<UserRole>().HasData(
             new
             {
                 UserId = new Guid("8180c0f7-1181-4ac8-88a4-5e3deb5d62e1"), // This Guid is already hardcoded, which is correct.
                 RoleId = 3,
                 CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) // Hardcoded date and time
             });
            modelBuilder.Entity<Agent>().HasData(
    new 
    {
        AgentId = 1,
        Name = "M SITHA RAMI REDDY",
        ContactInfo = "9959304189",
        UserId = new Guid("8180c0f7-1181-4ac8-88a4-5e3deb5d62e1"),
        CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    }
);








            modelBuilder.Entity<AvailablePolicy>().HasData(
     new  { AvailablePolicyId = 1, Name = "Basic Health", BasePremium = 5000m, CoverageDetails = "Covers basic health expenses", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 2, Name = "Comprehensive Health", BasePremium = 10000m, CoverageDetails = "Covers comprehensive health expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 3, Name = "Family Health", BasePremium = 15000m, CoverageDetails = "Covers health expenses for family", ValidityPeriod = 36, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 4, Name = "Senior Citizen Health", BasePremium = 8000m, CoverageDetails = "Covers health expenses for senior citizens", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 5, Name = "Child Health", BasePremium = 3000m, CoverageDetails = "Covers health expenses for children", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 6, Name = "Accident Coverage", BasePremium = 7000m, CoverageDetails = "Covers expenses due to accidents", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 7, Name = "Critical Illness", BasePremium = 12000m, CoverageDetails = "Covers expenses for critical illnesses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 8, Name = "Travel Health", BasePremium = 6000m, CoverageDetails = "Covers health expenses during travel", ValidityPeriod = 6, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 9, Name = "Dental Coverage", BasePremium = 4000m, CoverageDetails = "Covers dental expenses", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 10, Name = "Vision Coverage", BasePremium = 3500m, CoverageDetails = "Covers vision expenses", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 11, Name = "Maternity Coverage", BasePremium = 9000m, CoverageDetails = "Covers maternity expenses", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 12, Name = "Mental Health Coverage", BasePremium = 8000m, CoverageDetails = "Covers mental health expenses", ValidityPeriod = 12, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 13, Name = "Cancer Coverage", BasePremium = 15000m, CoverageDetails = "Covers cancer treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 14, Name = "Heart Disease Coverage", BasePremium = 14000m, CoverageDetails = "Covers heart disease treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 15, Name = "Diabetes Coverage", BasePremium = 13000m, CoverageDetails = "Covers diabetes treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 16, Name = "Stroke Coverage", BasePremium = 12000m, CoverageDetails = "Covers stroke treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 17, Name = "Kidney Disease Coverage", BasePremium = 11000m, CoverageDetails = "Covers kidney disease treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 18, Name = "Liver Disease Coverage", BasePremium = 10000m, CoverageDetails = "Covers liver disease treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 19, Name = "Lung Disease Coverage", BasePremium = 9000m, CoverageDetails = "Covers lung disease treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 20, Name = "Bone Disease Coverage", BasePremium = 8000m, CoverageDetails = "Covers bone disease treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 21, Name = "Skin Disease Coverage", BasePremium = 7000m, CoverageDetails = "Covers skin disease treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 22, Name = "Neurological Coverage", BasePremium = 16000m, CoverageDetails = "Covers neurological treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 23, Name = "Orthopedic Coverage", BasePremium = 15000m, CoverageDetails = "Covers orthopedic treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 24, Name = "Gastrointestinal Coverage", BasePremium = 14000m, CoverageDetails = "Covers gastrointestinal treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 25, Name = "Urological Coverage", BasePremium = 13000m, CoverageDetails = "Covers urological treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
     new  { AvailablePolicyId = 26, Name = "Endocrine Coverage", BasePremium = 12000m, CoverageDetails = "Covers endocrine treatment expenses", ValidityPeriod = 24, CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) }
     );


            

    
     
 





             modelBuilder.Entity<User>().HasData(
                new
                 {
                    Id = new Guid("46b23b43-59a2-4b06-a046-7693c706a883"),
                    Username = "Krishna_20",
         // This PasswordHash is already a static, hardcoded string, which is correct.
                    PasswordHash = "$2y$10$VxUmpNbbXg0kcVMZSAWjfeg3LRH.hs80dll6vjqyPqOmdViHBuAtK",
                    CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) // Hardcoded date and time
                });

            modelBuilder.Entity<UserRole>().HasData(
                new
                {
                    UserId = new Guid("46b23b43-59a2-4b06-a046-7693c706a883"), // This Guid is already hardcoded, which is correct.
                    RoleId = 1,
                    CreatedAt = new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc) // Hardcoded date and time
                });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsuranceDbContext).Assembly);
            AuditLogging.ApplyAuditShadowProperties(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUser = "System"; 
            AuditHelper.ApplyAuditFields(ChangeTracker, currentUser);

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
