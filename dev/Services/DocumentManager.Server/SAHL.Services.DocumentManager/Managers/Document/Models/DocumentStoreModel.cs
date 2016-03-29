namespace SAHL.Services.DocumentManager.Managers.Document.Models
{
    public class DocumentStoreModel
    {
        public decimal ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string NonIndexableChars { get; set; }

        public string Folder { get; set; }

        public string DefaultDocTitle { get; set; }

        public DocumentStoreModel(decimal id, string name, string description, string folder, string nonIndexableChars, string defaultDocTitle)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.NonIndexableChars = nonIndexableChars;
            this.Folder = folder;
            this.DefaultDocTitle = defaultDocTitle;
        }
    }
}