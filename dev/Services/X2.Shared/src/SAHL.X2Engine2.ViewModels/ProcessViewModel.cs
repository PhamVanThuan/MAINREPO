namespace SAHL.X2Engine2.ViewModels
{
    public class ProcessViewModel
    {
        public int ProcessID { get; set; }

        public string ProcessName { get; set; }

        public ProcessViewModel(int processId, string processName)
        {
            this.ProcessID = processId;
            this.ProcessName = processName;
        }
    }
}