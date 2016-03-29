using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileBaseEditorDataProvider<T> : HaloTileBaseSqlDataProvider,
                                                              IHaloTileEditorDataProvider<T> where T : IHaloTileModel
    {
        protected HaloTileBaseEditorDataProvider(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void SaveData<TDataModel>(BusinessContext businessContext, TDataModel dataModel) where TDataModel : IHaloTileModel
        {
            this.Update(businessContext, dataModel);
        }
    }
}
