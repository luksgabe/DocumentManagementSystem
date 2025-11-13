using DocumentManagement.Core.Enuns;

namespace DocumentManagement.Core.Entities
{
    public class DocumentShare : BaseEntity
    {
        public Guid DocumentId { get; private set; }
        public TargetType TargetType { get; private set; }
        public string TargetValue { get; private set; } = null!;
        public DateTime SharedAt { get; private set; }
        public Permission Permission { get; private set; }

        public virtual Document Document { get; private set; }
        public DocumentShare(Guid id, Guid documentId, TargetType targetType, string targetValue, Permission permission)
        {
            Id = id;
            DocumentId = documentId;
            TargetType = targetType;
            TargetValue = targetValue;
            Permission = permission;
            SharedAt = DateTime.UtcNow;
        }

        public void setDocument(Document document)
        {
            Document = document;
        }
    }
}
