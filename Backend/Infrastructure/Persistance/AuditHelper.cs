using InsurenceManagementSystemWebApi.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InsurenceManagementSystemWebApi.Infrastructure.Persistance
{
    public static class AuditHelper
    {
        public static void ApplyAuditFields(ChangeTracker changeTracker, string currentUser)
        {
            var entries = changeTracker
                .Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified)&& e.Entity.GetType()!=typeof(Notification) && e.Entity.GetType()!=typeof(Role));

            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Metadata.FindProperty("CreatedAt") != null && entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = currentTime;
                    entry.Property("CreatedBy").CurrentValue = currentUser;
                }

                if (entry.Metadata.FindProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = currentTime;
                    entry.Property("UpdatedBy").CurrentValue = currentUser;
                }
            }
        }
    }
}
