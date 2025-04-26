using AuditTrail.DBClass;
using AuditTrail.Models;
using System;

namespace AuditTrail.Services
{
    public class AuditService : IAuditService
    {
        private readonly AuditDbContext _dbContext;

        public AuditService(AuditDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuditEntry> CreateAuditEntryAsync<T>(T before, T after, AuditAction action, string userId)
        {
            if (before == null && after == null)
                throw new ArgumentException("Both before and after cannot be null.");

            var entityName = typeof(T).Name;
            var auditEntry = new AuditEntry
            {
                EntityName = entityName,
                Action = action,
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };

            var properties = typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0); // Only properties without parameters;

            if (action == AuditAction.Created)
            {
    
                foreach (var prop in properties)
                {
                    var newValue = prop.GetValue(after);
                    if (newValue != null)
                    {
                        auditEntry.Changes.Add(new AuditChange
                        {
                            PropertyName = prop.Name,
                            OldValue = null,
                            NewValue = newValue
                        });
                    }
                }
            }
            else if (action == AuditAction.Deleted)
            {
                foreach (var prop in properties)
                {
                    var oldValue = prop.GetValue(before);
                    if (oldValue != null)
                    {
                        auditEntry.Changes.Add(new AuditChange
                        {
                            PropertyName = prop.Name,
                            OldValue = oldValue,
                            NewValue = null
                        });
                    }
                }
            }
            else if (action == AuditAction.Updated)
            {
                foreach (var prop in properties)
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

            // Save to database
            var auditLog = new AuditLog
            {
                EntityName = auditEntry.EntityName,
                Action = auditEntry.Action,
                UserId = auditEntry.UserId,
                Timestamp = auditEntry.Timestamp,
                Changes = auditEntry.Changes.Select(c => new AuditLogChange
                {
                    PropertyName = c.PropertyName,
                    OldValue = c.OldValue?.ToString(),
                    NewValue = c.NewValue?.ToString()
                }).ToList()
            };

            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();

            return auditEntry;
        }
    }

}