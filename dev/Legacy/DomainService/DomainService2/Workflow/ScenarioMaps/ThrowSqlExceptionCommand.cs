namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowSqlExceptionCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a SQL exception.
        /// </summary>
        /// <param name="accountKeyThatDoesNotExist"></param>
        public ThrowSqlExceptionCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}