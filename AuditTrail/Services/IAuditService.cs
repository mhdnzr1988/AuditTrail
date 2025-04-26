using AuditTrail.Models;

namespace AuditTrail.Services
{
    public interface IAuditService
    {
        Task<AuditEntry> CreateAuditEntryAsync<T>(T before, T after, AuditAction action, string userId);
    }

}
