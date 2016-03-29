using Newtonsoft.Json.Linq;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.JsonDocument.Commands;
using SAHL.Services.JsonDocument.Managers;
using System;

namespace SAHL.Services.JsonDocument.CommandHandlers
{
    public class CreateOrUpdateDocumentCommandHandler : IServiceCommandHandler<CreateOrUpdateDocumentCommand>
    {
        private IJsonDocumentDataManager jsonDocumentDataManager;

        public CreateOrUpdateDocumentCommandHandler(IJsonDocumentDataManager jsonDocumentDataManager)
        {
            this.jsonDocumentDataManager = jsonDocumentDataManager;
        }

        public ISystemMessageCollection HandleCommand(CreateOrUpdateDocumentCommand command, IServiceRequestMetadata metadata)
        {
            var jObject = JObject.Parse(command.JsonDocument);
            Guid id = jObject["id"].ToObject<Guid>();
            string name = jObject["name"].ToString();
            string description = jObject["description"].ToString();
            int version = int.Parse(jObject["version"].ToString());
            string documentFormatVersion = jObject.GetValue("documentFormatVersion",StringComparison.InvariantCulture).Value<string>();
            string documentType = jObject["documentType"].ToString();
            string data = jObject["document"].ToString();

            version += 1;

            if (!this.jsonDocumentDataManager.DoesDocumentExist(id))
            {
                jsonDocumentDataManager.CreateJsonDocument(id, name, description, version, documentFormatVersion, documentType, data);
            }
            else
            {
                jsonDocumentDataManager.SaveJsonDocument(id, name, description, version, documentFormatVersion, documentType, data);
            }

            return new SystemMessageCollection();
        }
    }
}