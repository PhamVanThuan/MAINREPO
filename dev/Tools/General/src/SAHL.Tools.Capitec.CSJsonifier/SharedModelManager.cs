using SAHL.Tools.Capitec.CSJsonifier.Models;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public class SharedModelManager : ISharedModelManager
    {
        ITemplateManager templateManager;

        public SharedModelManager(ITemplateManager templateManager)
        {
            this.templateManager = templateManager;
        }

        public IScanResult GetComplexModels(IEnumerable<IScanResult> results)
        {
            SharedModels sharedModels = new SharedModels();
            
            foreach (IScanResult result in results)
            {
                foreach (TypeDefinition typeDefined in result.CommandTypes)
                {
                    RecurseTypeDefinition(typeDefined, sharedModels);
                }
            }
            return sharedModels;
        }

        public void RecurseTypeDefinition(TypeDefinition type, SharedModels sharedModels)
        {
            MethodReference constructor = templateManager.GetConstructor(type);
            if(constructor != null)
            {
                foreach (ParameterDefinition param in templateManager.GetConstructor(type).Parameters)
                {
                    TypeReference reference = param.ParameterType;
                    if (reference.Name != "ISystemMessageCollection")
                    { 
                        TypeDefinition definition = reference.Resolve();

                        if (IsSharedModel(definition, reference))
                        {
                            sharedModels.AddTypeDefinition(definition);
						    RecurseTypeDefinition(definition, sharedModels);
                        }
                        else if (definition.Namespace == "System.Collections.Generic")
                        {
                            foreach (TypeReference argTypeRef in ((Mono.Cecil.GenericInstanceType)reference).GenericArguments)
                            {
                                TypeDefinition argTypeDef = argTypeRef.Resolve();
                                if (IsSharedModel(argTypeDef, argTypeRef))
                                {
                                    sharedModels.AddTypeDefinition(argTypeDef);
                                }
                                RecurseTypeDefinition(argTypeDef, sharedModels);
                            }
                        }
                        else if (reference.IsArray)
                        {
                            RecurseTypeDefinition(reference.GetElementType().Resolve(), sharedModels);
                        }
                    }
                }
            }
        }

        private bool IsSharedModel(TypeDefinition definition, TypeReference reference)
        {
            return definition.IsClass && !definition.IsAbstract && !definition.Namespace.StartsWith("System") && !reference.IsArray && !definition.IsEnum;
        }
    }
}
