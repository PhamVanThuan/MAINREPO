namespace Automation.DataModels
{
    public class CorrespondenceTemplate : IDataModel
    {
        public int CorrespondenceTemplateKey { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Template { get; set; }

        public int ContentTypeKey { get; set; }
    }
}