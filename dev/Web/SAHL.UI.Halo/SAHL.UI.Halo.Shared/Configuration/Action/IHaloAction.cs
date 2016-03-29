namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloAction
    {
        string Name { get; }
        string IconName { get; }
        string Group { get; }
        int Sequence { get; }
    }
}
