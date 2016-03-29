using Mono.Cecil;
using SAHL.Tools.Capitec.CSJsonifier.Models;
using SAHL.Tools.Capitec.CSJsonifier.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Filters
{
    public class DecisionTreeGlobalsFilter : IScanConventionFilter<DecisionTreeGlobalsConvention, GlobalsModel>
    {
        public IEnumerable<GlobalsModel> Filter(DecisionTreeGlobalsConvention model)
        {
            List<GlobalsModel> models = new List<GlobalsModel>();
            
            foreach (TypeDefinition typeDefintion in model.commandTypes)
            {
                models.Add(new GlobalsModel(typeDefintion));
            }

            return models;
        }
    }
}
