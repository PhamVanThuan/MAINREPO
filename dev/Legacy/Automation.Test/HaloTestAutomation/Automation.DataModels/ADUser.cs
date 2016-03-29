using Common.Enums;
namespace Automation.DataModels
{
    public class ADUser : IDataModel
    {
        public int AdUserKey { get; set; }
        public string ADUserName { get; set; }
        public GeneralStatusEnum GeneralStatus { get; set; }
        public int LegalEntityKey { get; set; }
    }
}