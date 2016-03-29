using SAHL.Core.DataStructures;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.DecisionTreeDesign.Managers.CodeEditor;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;
using System.Reflection;

namespace SAHL.Services.DecisionTreeDesign.QueryHandlers
{
    public class GetCodeEditorKeywordsQueryHandler : IServiceQueryHandler<GetCodeEditorKeywordsQuery>
    {
        private ICodeEditorManager codeEditorManager;
        private IJsonActivator jsonActivator;

        private Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
            TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
            PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver { DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance },
            Error = (error, eventargs) => { }
        };

        public GetCodeEditorKeywordsQueryHandler(ICodeEditorManager codeEditorManager, IJsonActivator jsonActivator)
        {
            this.codeEditorManager = codeEditorManager;
            this.jsonActivator = jsonActivator;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(GetCodeEditorKeywordsQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            NestedTernary tree = codeEditorManager.GetTSTForCodeEditor();
            string data = jsonActivator.SerializeObject(tree, settings);
            query.Result = new ServiceQueryResult<GetCodeEditorKeywordsQueryResult>(new GetCodeEditorKeywordsQueryResult[]{
                new GetCodeEditorKeywordsQueryResult(){Data = data}
            });

            return messages;
        }
    }
}