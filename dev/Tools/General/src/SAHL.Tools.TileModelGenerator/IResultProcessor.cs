using SAHL.Tools.TileModelGenerator.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public interface IResultProcessor
    {
        void ProcessResult(TileModelConvention models,TileEditorConvention editorConfiguration,string location);
    }
}
