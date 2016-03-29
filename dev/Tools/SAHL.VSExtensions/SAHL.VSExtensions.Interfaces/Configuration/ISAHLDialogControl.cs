namespace SAHL.VSExtensions.Interfaces.Configuration
{
    public interface ISAHLConfiguration : IMenuItem
    {
        void Execute(ISAHLProjectItem projectItem, dynamic model);
    }
}