using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.Core.BusinessModel;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Interfaces.Halo
{
    public class TileEditorUpdateCommand : ServiceCommand
    {
        public TileEditorUpdateCommand(IHaloTileConfiguration tileConfiguration, BusinessContext businessContext, IHaloTileModel tileDataModel)
        {
            if (tileConfiguration == null) { throw new ArgumentNullException("tileConfiguration"); }
            if (businessContext == null) { throw new ArgumentNullException("businessContext"); }
            if (tileDataModel == null) { throw new ArgumentNullException("tileDataModel"); }

            this.TileConfiguration = tileConfiguration;
            this.BusinessContext   = businessContext;
            this.TileDataModel     = tileDataModel;
        }

        public IHaloTileConfiguration TileConfiguration { get; protected set; }
        public BusinessContext BusinessContext { get; protected set; }
        public IHaloTileModel TileDataModel { get; protected set; }
    }
}
