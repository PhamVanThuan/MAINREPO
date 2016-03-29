namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowDomainMessageExceptionCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a domain message exception.
        /// </summary>
        /// <param name="ignorewarnings"></param>
        public ThrowDomainMessageExceptionCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}