using System.ComponentModel.DataAnnotations;

namespace AuditTrail.Models
{
    public enum AuditAction
    {
        Created,
        Updated,
        Deleted
    }

    public class AuditEntry
    {
        public string EntityName { get; set; }
        public AuditAction Action { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AuditChange> Changes { get; set; } = new();
    }

    public class AuditChange
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
    public class AuditLog
    {
        public int Id { get; set; }
        public string? EntityName { get; set; }
        public AuditAction Action { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public List<AuditLogChange> Changes { get; set; } = new();
    }

    public class AuditLogChange
    {
        public int Id { get; set; }
        public int AuditLogId { get; set; }
        public string? PropertyName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public AuditLog AuditLog { get; set; }
    }
    public class AuditRequest
    {
        public dynamic Before { get; set; }
        public dynamic After { get; set; }
        public AuditAction Action { get; set; }
        public string UserId { get; set; }
    }

}
