using AuditTrail.Models;

namespace AuditTrail.Services
{
    public class AuditService : IAuditService
    {
        public AuditTrailLog CreateAuditEntry<T>(T before, T after, AuditAction action, string userId)
        {
            if (before == null && after == null)
                throw new ArgumentException("Both objects cannot be null.");

            var entityName = typeof(T).Name;
            var auditEntry = new AuditTrailLog
            {
                EntityName = entityName,
                Action = action,
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };

            if (action == AuditAction.Created)
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    var newValue = prop.GetValue(after);
                    auditEntry.Changes.Add(new AuditChange
                    {
                        PropertyName = prop.Name,
                        OldValue = null,
                        NewValue = newValue
                    });
                }
            }
            else if (action == AuditAction.Deleted)
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    var oldValue = prop.GetValue(before);
                    auditEntry.Changes.Add(new AuditChange
                    {
                        PropertyName = prop.Name,
                        OldValue = oldValue,
                        NewValue = null
                    });
                }
            }
            else if (action == AuditAction.Updated)
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    var oldValue = prop.GetValue(before);
                    var newValue = prop.GetValue(after);

                    if (!Equals(oldValue, newValue))
                    {
                        auditEntry.Changes.Add(new AuditChange
                        {
                            PropertyName = prop.Name,
                            OldValue = oldValue,
                            NewValue = newValue
                        });
                    }
                }
            }

            return auditEntry;
        }
    }
}