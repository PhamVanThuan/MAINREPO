using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO
    /// </summary>
    public partial interface ICorrespondence : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.DestinationValue
        /// </summary>
        System.String DestinationValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.DueDate
        /// </summary>
        DateTime? DueDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CompletedDate
        /// </summary>
        DateTime? CompletedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.OutputFile
        /// </summary>
        System.String OutputFile
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceMedium
        /// </summary>
        ICorrespondenceMedium CorrespondenceMedium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.ReportStatement
        /// </summary>
        IReportStatement ReportStatement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceParameters
        /// </summary>
        IEventList<ICorrespondenceParameters> CorrespondenceParameters
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceDetail
        /// </summary>
        ICorrespondenceDetail CorrespondenceDetail
        {
            get;
            set;
        }
    }
}