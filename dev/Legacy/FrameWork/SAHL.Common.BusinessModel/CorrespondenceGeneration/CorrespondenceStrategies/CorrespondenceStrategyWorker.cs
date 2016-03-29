using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Configuration;
using SAHL.Common.Configuration;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;

namespace SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceStrategies
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrespondenceStrategyWorker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_ViewName"></param>
        /// <param name="p_OriginationSourceProductKey"></param>
        /// <returns></returns>
        public static List<ReportData> GetReportData(string p_ViewName, int p_OriginationSourceProductKey)
        {
            List<ReportData> lr = new List<ReportData>();

            // Get the correspondence section from the web.config 
            CorrespondenceSection correspondenceSection = (CorrespondenceSection)ConfigurationManager.GetSection(SAHL.Common.Constants.WebConfig.CorrespondenceSection);

            foreach (CorrespondenceReportElement re in correspondenceSection.Views[p_ViewName].Reports)
            {
                bool batchPrint = correspondenceSection.Views[p_ViewName].BatchPrint;
                bool allowPreview = correspondenceSection.Views[p_ViewName].AllowPreview;
                string dataStorName = correspondenceSection.Views[p_ViewName].DataStorName;
                bool updateConditions = correspondenceSection.Views[p_ViewName].UpdateConditions;
                bool sendUserConfirmationEmail = correspondenceSection.Views[p_ViewName].SendUserConfirmationEmail;
                bool emailProcessedPDFtoConsultant = correspondenceSection.Views[p_ViewName].EmailProcessedPDFtoConsultant;
                SAHL.Common.Globals.CorrespondenceTemplates correspondenceTemplate = correspondenceSection.Views[p_ViewName].CorrespondenceTemplate;
                bool combineDocumentsIfEmailing = correspondenceSection.Views[p_ViewName].CombineDocumentsIfEmailing;

                ReportData rd = new ReportData(re.ReportName, p_OriginationSourceProductKey, re, batchPrint, allowPreview, dataStorName, updateConditions, sendUserConfirmationEmail, emailProcessedPDFtoConsultant, correspondenceTemplate, combineDocumentsIfEmailing);
                DefaultParameterBuilder irp = new DefaultParameterBuilder();

                irp.GetReportParameters(rd.ReportStatementKey, rd.ReportParameters);

                lr.Add(rd);
            }

            return lr;
        }
    }
}
