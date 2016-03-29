namespace SAHL.Common.BusinessModel.Interfaces.Validation
{
    public interface IEntityRuleConsumer
    {
        void AddProvider(IEntityRuleProvider Provider);

        void RemoveProvider(IEntityRuleProvider Provider);
    }
}