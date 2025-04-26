using System.ComponentModel.DataAnnotations;

namespace AuditTrail.Models
{
    public enum AuditAction
    {
        Created,
        Updated,
        Deleted
    }

    public class AuditRequest
    {
        public object Before { get; set; }
        public object After { get; set; }
        public AuditAction Action { get; set; }
        public string UserId { get; set; }
    }

    public class AuditTrailLog
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
}
