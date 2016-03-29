namespace SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish
{
    public class GetMachinesWithLogMessagesInLastDayQueryResult
    {
        public string MachineName { get; set; }

        public int ErrorCount { get; set; }

        public GetMachinesWithLogMessagesInLastDayQueryResult(string machineName, int errorCount)
        {
            this.MachineName = machineName;
            this.ErrorCount = errorCount;
        }
    }
}