using DocumentManagement.Core.Enuns;
using DocumentManagement.Core.Exceptions;

namespace DocumentManagement.Core.Entities
{
    public class Document : BaseEntity
    {
        #region Private properties
        private readonly List<DocumentShare> _shares = new();
        private readonly List<DocumentTag> _documentTags = new();
        #endregion

        #region Public properties
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
        public IReadOnlyCollection<DocumentTag> DocumentTags => _documentTags.AsReadOnly();
        public IReadOnlyCollection<DocumentShare> Shares => _shares.AsReadOnly();
        #endregion

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
                throw new AppValidationException("Tag is already attached to this document.");
            }
            _documentTags.Add(new DocumentTag(this.Id, tag.Id));
        }

        public void ClearDocumentsTag()
        {
            _documentTags.Clear();
        }

        public void AddShare(DocumentShare share)
        {
            if (_shares.Any(s =>
                    s.TargetType == share.TargetType &&
                    s.TargetValue.Equals(share.TargetValue, StringComparison.OrdinalIgnoreCase)))
            {
                throw new AppValidationException("Share already exists for this target.");
            }

            _shares.Add(share);
        }

        public void RemoveShare(Guid shareId)
        {
            var existing = _shares.FirstOrDefault(s => s.Id == shareId);
            if (existing is null)
                throw new AppValidationException("Share not found for this document.");

            _shares.Remove(existing);
        }

        public bool CanRead(string userSub, string userEmail, string userRole)
        {
            if (AccessType == AccessType.Public)
                return true;

            if (OwnerSub == userSub)
                return true;

            if (AccessType != AccessType.Restricted)
                return false;

            return HasPermissionInternal(Permission.Read, userEmail, userRole);
        }

        public bool CanWrite(string userSub, string userEmail, string userRole)
        {
            if (OwnerSub == userSub)
                return true;

            if (AccessType != AccessType.Restricted)
                return false;

            return HasPermissionInternal(Permission.Write, userEmail, userRole);
        }

        private bool HasPermissionInternal(Permission required, string userEmail, string userRole)
        {
            return _shares.Any(s =>
            {
                var matchesTarget = s.TargetType switch
                {
                    TargetType.User => s.TargetValue.Equals(userEmail, StringComparison.OrdinalIgnoreCase),
                    TargetType.Role => s.TargetValue.Equals(userRole, StringComparison.OrdinalIgnoreCase),
                    _ => false
                };

                if (!matchesTarget) return false;

                // Write > Read; Delete/Share também implicam Read
                return s.Permission switch
                {
                    Permission.Read => required == Permission.Read,
                    Permission.Write => required is Permission.Read or Permission.Write,
                    Permission.Delete => true,
                    Permission.Share => true,
                    _ => false
                };
            });
        }

        public void Updated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
