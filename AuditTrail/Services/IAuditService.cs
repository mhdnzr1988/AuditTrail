using AuditTrail.Models;

namespace AuditTrail.Services
{
    public interface IAuditService
    {
        AuditTrailLog CreateAuditEntry<T>(T before, T after, AuditAction action, string userId);
    }
}
