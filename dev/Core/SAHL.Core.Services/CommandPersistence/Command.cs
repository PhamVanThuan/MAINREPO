namespace SAHL.Core.Services.CommandPersistence
{
    public class Command : ICommand
    {
        public string CommandJson { get; set; }

        public System.DateTime CommandDate { get; set; }
    }
}