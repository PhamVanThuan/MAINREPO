using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Configuration;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportData
    {
        string _ReportName;
        int _ReportStatementKey;
        int _OriginationSourceProductKey;
        string _GenericKeyParameterName = "";
        string _LegalEntityParameterName = "";
        string _AddressParameterName = "";
        string _MailingTypeParameterName = "";
        string _LanguageKeyParameterName = "";
        bool _BatchPrint = false;
        bool _AllowPreview = false;
        string _dataStorName = "";
        string _statementName = "";
        bool _updateConditions = false;
        bool _sendUserConfirmationEmail = true;
        bool _emailProcessedPDFtoConsultant = false;
        bool _combineDocumentsIfEmailing = false;
        bool _excludeFromDataSTOR = false;
        CorrespondenceTemplates _correspondenceTemplate = CorrespondenceTemplates.EmailCorrespondenceDefault;
        CorrespondenceReportElement _correspondenceReportElement;

        List<ReportDataParameter> _ReportParameters = new List<ReportDataParameter>();
        List<CorrespondenceMediums> _CorrespondenceMediums = new List<CorrespondenceMediums>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="originationSourceProductKey"></param>
        /// <param name="correspondenceReportElement"></param>
        /// <param name="batchPrint"></param>
        /// <param name="allowPreview"></param>
        /// <param name="dataStorName"></param>
        /// <param name="updateConditions"></param>
        /// <param name="sendUserConfirmationEmail"></param>
        /// <param name="emailProcessedPDFtoConsultant"></param>
        /// <param name="correspondenceTemplate"></param>
        /// <param name="combineDocumentsIfEmailing"></param>
        public ReportData(string reportName, int originationSourceProductKey, CorrespondenceReportElement correspondenceReportElement, bool batchPrint, bool allowPreview, string dataStorName, bool updateConditions, bool sendUserConfirmationEmail, bool emailProcessedPDFtoConsultant, CorrespondenceTemplates correspondenceTemplate, bool combineDocumentsIfEmailing)
        {
            _ReportName = reportName;
            _OriginationSourceProductKey = originationSourceProductKey;
            _correspondenceReportElement = correspondenceReportElement;

            // Get the Report Repository
            IReportRepository ReportRepo = RepositoryFactory.GetRepository<IReportRepository>();

            // Get the ReportStatement Object using the ReportName and OriginationSourceProductKey
            IReportStatement _reportStatement = ReportRepo.GetReportStatementByNameAndOSP(reportName, originationSourceProductKey);

            if (_reportStatement == null)
            {
                _reportStatement = ReportRepo.GetReportStatementByNameAndOSP(reportName, -1);
                if (_reportStatement == null)
                    _ReportStatementKey = -1;
            }

            if (_reportStatement != null)
            {
                _ReportStatementKey = _reportStatement.Key;
                _statementName = _reportStatement.StatementName;

                // Get the Reports Correspondence Mediums 
                foreach (ICorrespondenceMedium _correspondenceMedium in _reportStatement.CorrespondenceMediums)
                {
                    CorrespondenceMediums cm = new CorrespondenceMediums(_correspondenceMedium.Key, _correspondenceMedium.Description, null);
                    _CorrespondenceMediums.Add(cm);
                }
            }

            // Get the Report Parameter Mapping Names
            _GenericKeyParameterName = correspondenceReportElement.GenericKeyParameterName;
            _LegalEntityParameterName = correspondenceReportElement.LegalEntityParameterName;
            _AddressParameterName = correspondenceReportElement.AddressParameterName;
            _MailingTypeParameterName = correspondenceReportElement.MailingTypeParameterName;
            _LanguageKeyParameterName = correspondenceReportElement.LanguageKeyParameterName;

            _excludeFromDataSTOR = correspondenceReportElement.ExcludeFromDataSTOR;

            // is this to be printed via batch printing 
            _BatchPrint = batchPrint;
            // are we allowing the user to preview the report before processing
            _AllowPreview = allowPreview;
            // the data STOR name if required
            _dataStorName = dataStorName;
            // should this trigger an update of conditions
            _updateConditions = updateConditions;
            //should we send the user a confirmation email
            _sendUserConfirmationEmail = sendUserConfirmationEmail;
            //should we attach the processed pdf to the confirmation email
            _emailProcessedPDFtoConsultant = emailProcessedPDFtoConsultant;
            //the type of correspondence template to use when emailing documents
            _correspondenceTemplate = correspondenceTemplate;
            // this is used to set if we are sending all the document on one email - if email option is selected
            _combineDocumentsIfEmailing = combineDocumentsIfEmailing;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ReportName
        {
            get { return _ReportName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReportStatementKey
        {
            get { return _ReportStatementKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OriginationSourceProductKey
        {
            get { return _OriginationSourceProductKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ReportDataParameter> ReportParameters
        {
            get { return _ReportParameters; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CorrespondenceMediums> CorrespondenceMediums
        {
            get { return _CorrespondenceMediums; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string GenericKeyParameterName
        {
            get { return _GenericKeyParameterName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LegalEntityParameterName
        {
            get { return _LegalEntityParameterName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AddressParameterName
        {
            get { return _AddressParameterName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExcludeFromDataSTOR
        {
            get { return _excludeFromDataSTOR; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MailingTypeParameterName
        {
            get { return _MailingTypeParameterName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LanguageKeyParameterName
        {
            get { return _LanguageKeyParameterName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool BatchPrint
        {
            get { return _BatchPrint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowPreview
        {
            get { return _AllowPreview; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataStorName
        {
            get { return _dataStorName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StatementName
        {
            get { return _statementName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool UpdateConditions
        {
            get { return _updateConditions; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SendUserConfirmationEmail
        {
            get { return _sendUserConfirmationEmail; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EmailProcessedPDFtoConsultant
        {
            get { return _emailProcessedPDFtoConsultant; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CorrespondenceTemplates CorrespondenceTemplate
        {
            get { return _correspondenceTemplate; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CombineDocumentsIfEmailing
        {
            get { return _combineDocumentsIfEmailing; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CorrespondenceReportElement CorrespondenceReportElement
        {
            get { return _correspondenceReportElement; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReportDataParameter
    {
        int _ReportParameterKey;
        string _ParameterName;
        object _ParameterValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_ReportParameterKey"></param>
        /// <param name="p_ParameterName"></param>
        /// <param name="p_ParameterValue"></param>
        public ReportDataParameter(int p_ReportParameterKey, string p_ParameterName, object p_ParameterValue)
        {
            _ReportParameterKey = p_ReportParameterKey;
            _ParameterName = p_ParameterName;
            _ParameterValue = p_ParameterValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReportParameterKey
        {
            get { return _ReportParameterKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParameterName
        {
            get { return _ParameterName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object ParameterValue
        {
            get { return _ParameterValue; }
            set { _ParameterValue = value; }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CorrespondenceMediums
    {
        int _CorrespondenceMediumKey;
        string _Description;
        string _Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_CorrespondenceMediumKey"></param>
        /// <param name="p_Description"></param>
        /// <param name="p_Value"></param>
        public CorrespondenceMediums(int p_CorrespondenceMediumKey, string p_Description, string p_Value)
        {
            _CorrespondenceMediumKey = p_CorrespondenceMediumKey;
            _Description = p_Description;
            _Value = p_Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CorrespondenceMediumKey
        {
            get { return _CorrespondenceMediumKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return _Description; }
        }
    }


}
