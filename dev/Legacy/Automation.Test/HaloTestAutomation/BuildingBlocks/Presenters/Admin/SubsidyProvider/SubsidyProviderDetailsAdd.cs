namespace BuildingBlocks.Presenters.Admin
{
    public sealed class SubsidyProviderDetailsAdd : SubsidyProviderBase
    {
        public override void Populate(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            base.SubsidyType.Option(subsidyProvider.SubsidyProviderTypeDescription).Select();
            base.Populate(subsidyProvider);
        }
    }
}