using InsurenceManagementSystemWebApi.Domain.Models;

namespace InsurenceManagementSystemWebApi.Infrastructure.Persistance.Configurations
{
    public static  class AuditLogging
    {
        private static readonly string[] ExcludedEntities =
   {
        typeof(Notification).FullName!,
        typeof(Role).FullName!
    };

        public static void ApplyAuditShadowProperties(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (ExcludedEntities.Contains(entityType.Name))
                    continue;

                modelBuilder.Entity(entityType.ClrType).Property<DateTime>("CreatedAt");
                modelBuilder.Entity(entityType.ClrType).Property<DateTime?>("UpdatedAt");
                modelBuilder.Entity(entityType.ClrType).Property<string>("CreatedBy");
                modelBuilder.Entity(entityType.ClrType).Property<string>("UpdatedBy");
            }
        }
    }
}
