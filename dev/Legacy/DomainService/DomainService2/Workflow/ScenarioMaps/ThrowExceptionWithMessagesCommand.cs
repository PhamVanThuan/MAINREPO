namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowExceptionWithMessagesCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a general exception with messages.
        /// </summary>
        /// <param name="ignorewarnings"></param>
        public ThrowExceptionWithMessagesCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}