using Mono.Cecil;
using SAHL.Tools.TileModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Templates
{
    public partial class TileModelView
    {
        public TileModel Model { get; protected set; }

        public TileModelView(TileModel model)
        {
            this.Model = model;
        }
    }
}
