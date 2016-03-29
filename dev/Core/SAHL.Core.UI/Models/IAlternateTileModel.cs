namespace SAHL.Core.UI.Models
{
    public interface IAlternateTileModel
    {
    }

    public interface IAlternateTileModel<T> : IAlternateTileModel, ITileModel where T : ITileModel
    {
    }
}