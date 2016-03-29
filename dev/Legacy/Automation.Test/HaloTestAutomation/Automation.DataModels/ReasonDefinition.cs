namespace Automation.DataModels
{
    public class ReasonDefinition : IDataModel
    {
        public int ReasonDefinitionKey { get; set; }

        public int ReasonTypeKey { get; set; }

        public string ReasonTypeDescription { get; set; }

        public int ReasonTypeGroupKey { get; set; }

        public bool EnforceComment { get; set; }

        public int GeneralStatusKey { get; set; }

        public int GenericKeyTypeKey { get; set; }
    }
}