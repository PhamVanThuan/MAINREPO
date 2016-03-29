using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.V3.Client.Models
{
    public class CommandResult
    {
        public JObject JsonObject { get; private set; }

        public ErrorResult Errors;

        public bool IsErrorResponse
        {
            get
            {
                return this.Errors != null;
            }
        }

        public CommandResult(string jsonResponse)
        {
            this.JsonObject = JObject.Parse(jsonResponse);
            ParseErrors();
        }

        private void ParseErrors()
        {
            var json = (dynamic)this.JsonObject;

            var systemMessages = json.SystemMessages;

            bool hasErrors = systemMessages.HasErrors;
            /*bool hasExceptions = systemMessages.HasExceptions;
            bool hasWarnings = systemMessages.HasWarnings;
            bool hasExceptionMessage = systemMessages.HasExceptionMessage;*/

            if(hasErrors)
            {
                var values = systemMessages.AllMessages["$values"];

                this.Errors = new ErrorResult();

                foreach (var message in values)
                {
                    this.Errors.AddError(message["Message"].ToString());
                }
            }
        }

    }
}
