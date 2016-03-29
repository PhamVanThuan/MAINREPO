using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Templates
{
    interface GeneratorObject
    {
        void ParseJson();
        string TransformText();
    }
}
