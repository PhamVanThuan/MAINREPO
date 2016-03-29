namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowExceptionWithoutMessagesCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a general exception without messages.
        /// </summary>
        /// <param name="ignorewarnings"></param>
        public ThrowExceptionWithoutMessagesCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}