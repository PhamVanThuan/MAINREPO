using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO
    /// </summary>
    public partial interface IHelpDeskQuery : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.Memo
        /// </summary>
        IMemo Memo
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.ResolvedDate
        /// </summary>
        DateTime? ResolvedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.HelpDeskCategory
        /// </summary>
        IHelpDeskCategory HelpDeskCategory
        {
            get;
            set;
        }
    }
}