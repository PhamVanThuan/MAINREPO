namespace Automation.DataModels
{
    public class Activity
    {
        public string StateName { get; set; }

        public string ActivityName { get; set; }

        public string NextState { get; set; }

        public Activity NextActivity { get; set; }
    }
}