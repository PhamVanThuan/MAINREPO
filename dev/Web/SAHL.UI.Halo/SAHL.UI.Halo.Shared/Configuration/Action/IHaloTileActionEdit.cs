namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileActionEdit : IHaloTileAction
    {
    }

    public interface IHaloTileActionEdit<T> : IHaloTileActionEdit 
        where T : IHaloTileConfiguration
    {
    }
}
