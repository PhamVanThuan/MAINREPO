namespace SAHL.Services.Interfaces.Communications.ExternalServices.Notification
{
    public class Recipient
    {
        public string CellPhoneNumber { get; set; }

        public Recipient(string cellPhoneNumber)
        {
            this.CellPhoneNumber = cellPhoneNumber;
        }
    }
}