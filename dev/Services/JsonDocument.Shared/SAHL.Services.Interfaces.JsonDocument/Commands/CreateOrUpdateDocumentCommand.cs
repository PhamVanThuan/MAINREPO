using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.JsonDocument.Commands
{
    public class CreateOrUpdateDocumentCommand : ServiceCommand, IJsonDocumentCommand
    {
        public CreateOrUpdateDocumentCommand(string jsonDocument)
        {
            this.JsonDocument = jsonDocument;
        }
        public string JsonDocument { get; protected set; }
    }
}
