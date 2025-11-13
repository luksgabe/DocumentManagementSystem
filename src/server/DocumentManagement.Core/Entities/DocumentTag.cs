namespace DocumentManagement.Core.Entities
{
    public class DocumentTag
    {
        public Guid DocumentId { get; private set; } 
        public Document Document { get; private set; } = null!; 

        public Guid TagId { get; private set; }
        public Tag Tag { get; private set; } = null!; 

        private DocumentTag() { }

        internal DocumentTag(Guid documentId, Guid tagId)
        {
            DocumentId = documentId;
            TagId = tagId;
        }
    }
}
