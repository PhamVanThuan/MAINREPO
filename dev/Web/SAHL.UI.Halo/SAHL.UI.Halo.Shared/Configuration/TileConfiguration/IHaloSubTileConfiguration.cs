namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloSubTileConfiguration : IHaloTileConfiguration
    {
        int StartRow { get; }
        int StartColumn { get; }
        int NoOfRows { get; }
        int NoOfColumns { get; }
    }
}
