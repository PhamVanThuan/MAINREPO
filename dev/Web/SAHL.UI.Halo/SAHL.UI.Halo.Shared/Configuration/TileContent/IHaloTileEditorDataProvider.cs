using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileEditorDataProvider : IHaloTileDataProvider
    {
        void SaveData<T>(BusinessContext businessContext, T dataModel) where T : IHaloTileModel;
    }

    public interface IHaloTileEditorDataProvider<T> : IHaloTileEditorDataProvider where T : IHaloTileModel
    {
    }
}
