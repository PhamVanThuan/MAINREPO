namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloModuleConfiguration
    {
        string Name { get; }
        int Sequence { get; }
        bool IsTileBased { get; }
        string NonTilePageState { get; }
    }
}