namespace DocumentManagement.Core.Entities
{
    public class Document : BaseEntity
    {
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public AccessType AccessType { get; private set; }
        public string FileName { get; private set; } = null!;
        public long FileSizeBytes { get; private set; }
        public string ContentType { get; private set; } = null!;
        public string OwnerSub { get; private set; } = null!;
        public string OwnerEmail { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string StorageUri { get; private set; } = null!;
        public readonly List<DocumentTag> _documentTags = new();
        public IReadOnlyCollection<DocumentTag> DocumentTags => _documentTags.AsReadOnly();

        public Document(Guid id,
            string title,
            string description,
            AccessType accessType,
            string fileName,
            long fileSizeBytes,
            string contentType,
            string ownerSub,
            string ownerEmail,
            string storageUri
            )
        {
            Id = id;
            Title = title;
            Description = description;
            AccessType = accessType;
            FileName = fileName;
            FileSizeBytes = fileSizeBytes;
            ContentType = contentType;
            OwnerSub = ownerSub;
            OwnerEmail = ownerEmail;
            StorageUri = storageUri;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddTag(Tag tag)
        {
            if (_documentTags.Any(e => e.TagId == tag.Id))
            {
                throw new InvalidOperationException("Tag is already attached to this document.");
            }
            _documentTags.Add(new DocumentTag(this.Id, tag.Id));
        }
    }
}
