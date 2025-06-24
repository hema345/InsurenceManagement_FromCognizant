using InsurenceManagementSystemWebApi.Domain.Models;


namespace InsurenceManagementSystemWebApi.Infrastructure.Persistance.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.HasKey(a => a.AgentId);
            builder.Property(a => a.Name).IsRequired();

            builder.HasOne(a => a.User)
                   .WithOne(u=>u.Agent)
                   .HasForeignKey<Agent>(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
