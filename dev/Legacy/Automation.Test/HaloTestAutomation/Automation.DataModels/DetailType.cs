namespace Automation.DataModels
{
    public sealed class DetailType
    {
        public int DetailTypeKey { get; set; }

        public int DetailClassKey { get; set; }

        public string Description { get; set; }

        public DetailClass LoanDetailClass { get; set; }

        public int GeneralStatusKey { get; set; }

        public bool AllowScreen { get; set; }
    }
}