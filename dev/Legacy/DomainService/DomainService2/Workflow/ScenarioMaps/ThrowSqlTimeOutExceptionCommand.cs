namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowSqlTimeOutExceptionCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a SQL time out exception.
        /// NB: This assumes sql will timeout after 30 seconds
        /// </summary>
        /// <param name="accountKeyThatDoesNotExist"></param>
        public ThrowSqlTimeOutExceptionCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}