using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CorrespondenceTemplateDataModel :  IDataModel
    {
        public CorrespondenceTemplateDataModel(int correspondenceTemplateKey, string name, string subject, string template, int contentTypeKey, string defaultEmail)
        {
            this.CorrespondenceTemplateKey = correspondenceTemplateKey;
            this.Name = name;
            this.Subject = subject;
            this.Template = template;
            this.ContentTypeKey = contentTypeKey;
            this.DefaultEmail = defaultEmail;
		
        }		

        public int CorrespondenceTemplateKey { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Template { get; set; }

        public int ContentTypeKey { get; set; }

        public string DefaultEmail { get; set; }
    }
}