namespace DocumentManagement.Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; private set; }

        public Tag(string name)
        {
            Name = name;
        }

        public readonly List<DocumentTag> _documentTags = new();
        public IReadOnlyCollection<DocumentTag> DocumentTags => _documentTags.AsReadOnly();
    }
}
