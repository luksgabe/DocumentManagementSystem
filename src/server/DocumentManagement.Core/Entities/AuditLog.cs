namespace DocumentManagement.Core.Entities
{
    public class AuditLog : BaseEntity
    {
        public AuditLog(Guid id, DateTime whenUtc, string action, string userSub, string userEmail, string entity, string entityId, string ip, string metadata)
        {
            Id=id;
            WhenUtc=whenUtc;
            Action=action;
            UserSub=userSub;
            UserEmail=userEmail;
            Entity=entity;
            EntityId=entityId;
            Ip=ip;
            Metadata=metadata;
        }

        public DateTime WhenUtc { get; private set; }
        public string Action { get; private set; } = null!;
        public string UserSub { get; private set; } = null!;
        public string UserEmail { get; private set; } = null!;
        public string Entity { get; private set; } = null!;
        public string EntityId { get; private set; } = null!;
        public string Ip { get; private set; } = null!;
        public string Metadata { get; private set; } = null!;

        
    }
}
