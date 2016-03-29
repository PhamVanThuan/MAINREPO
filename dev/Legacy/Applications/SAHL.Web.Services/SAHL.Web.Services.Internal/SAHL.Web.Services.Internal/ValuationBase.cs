using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.X2.BusinessModel.Interfaces;
using System.Xml.Schema;
using System.IO;
using System.Xml;

namespace SAHL.Web.Services.Internal
{
    public abstract class ValuationBase : WebServiceBase
    {
        /// <summary>
        /// This method uses the passed in instanceID as the activating instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="activityName"></param>
        protected void TriggerExternalActivity(long instanceID, string activityName)
        {
            // get the valuations workflow
            IWorkFlow workflow = x2Repository.GetWorkFlowByName(SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);
            // get the external activity
            IExternalActivity extActivity = x2Repository.GetExternalActivityByName(activityName, workflow.ID);
            IActiveExternalActivity ext = x2Repository.GetEmptyActiveExternalActivity();
            ext.ActivatingInstanceID = instanceID;
            ext.ExternalActivity = extActivity;
            ext.WorkFlowID = workflow.ID;
            ext.ActivationTime = DateTime.Now;
            //ext.ActivityXMLData = null;
            //ext.WorkFlowProviderName = null;

            x2Repository.SaveActiveExternalActivity(ext);
        }

        protected XmlSchemaException ValidateXmlDocument(string xml, string xsd)
        {
            XmlSchemaException ex = null;
            StringReader stringReaderXml = new StringReader(xml);
            StringReader stringReaderXsd = new StringReader(xsd);
            XmlReader xmlReaderXsd = XmlReader.Create(stringReaderXsd);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xmlReaderXsd);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += (sender, args) =>
            {
                if (args.Severity == XmlSeverityType.Error)
                {
                    ex = args.Exception;
                }
            };

            using (var validationReader = XmlReader.Create(stringReaderXml, settings))
            {
                while (validationReader.Read()) ;
            }

            return ex;
        }
    }
}