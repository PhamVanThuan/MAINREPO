using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.UI.Models
{
    public interface IDrillPreviewTileModel : ITileModel
    {
        string BusinessKeys { get; set; }

        GenericKeyType GenericKeysType { get; set; }
    }
}