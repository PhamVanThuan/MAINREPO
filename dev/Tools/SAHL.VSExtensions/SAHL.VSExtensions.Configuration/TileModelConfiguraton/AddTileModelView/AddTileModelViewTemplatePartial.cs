using System;
using System.Collections.Generic;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModelView
{
    public partial class AddTileModelViewTemplate
    {
        public dynamic Model
        {
            get;
            protected set;
        }

        public IDictionary<string, Object> ModelDictionary
        {
            get
            {
                return (this.Model as IDictionary<string, Object>);
            }
        }

        public AddTileModelViewTemplate(dynamic model)
        {
            this.Model = model;
        }
    }
}