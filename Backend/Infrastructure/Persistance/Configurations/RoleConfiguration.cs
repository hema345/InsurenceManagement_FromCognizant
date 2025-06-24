using InsurenceManagementSystemWebApi.Domain.Models;

public class RoleConfiguration : IEntityTypeConfiguration<Role> 
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired();
    }

}
