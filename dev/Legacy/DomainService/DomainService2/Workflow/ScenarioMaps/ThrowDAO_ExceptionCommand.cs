namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowDAO_ExceptionCommand : StandardDomainServiceCommand
    {
        /// <summary>
        /// Use this command to throw a DAO exception.
        /// </summary>
        /// <param name="ignorewarnings"></param>
        public ThrowDAO_ExceptionCommand(bool ignorewarnings)
            : base(ignorewarnings)
        {
        }
    }
}