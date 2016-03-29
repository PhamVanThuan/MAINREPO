namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowDomainValidationExceptionCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a domain validation exception.
        /// </summary>
        /// <param name="ignorewarnings"></param>
        public ThrowDomainValidationExceptionCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}