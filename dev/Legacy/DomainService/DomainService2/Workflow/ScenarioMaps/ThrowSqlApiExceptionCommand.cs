namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowSqlApiExceptionCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a SQL exception.
        /// </summary>
        /// <param name="accountKeyThatDoesNotExist"></param>
        public ThrowSqlApiExceptionCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}