namespace SAHL.VSExtensions.Interfaces.Configuration
{
    public interface IMenuItem : IMenuGroup
    {
        string Name { get; }

        bool CanExecute(ISAHLProjectItem projectItem);
    }
}