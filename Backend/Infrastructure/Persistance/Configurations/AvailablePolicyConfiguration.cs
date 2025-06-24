using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Infrastructure.Persistance.Configurations
{
    public class AvailablePolicyConfiguration : IEntityTypeConfiguration<AvailablePolicy>
    {
        public void Configure(EntityTypeBuilder<AvailablePolicy> builder)
        {
            builder.HasKey(p => p.AvailablePolicyId);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.BasePremium).IsRequired();
            builder.Property(p => p.BasePremium).HasPrecision(10,2);

            builder.Property(p => p.CoverageDetails).IsRequired();
        }
    }
}
