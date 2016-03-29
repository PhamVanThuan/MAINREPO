using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class ImportStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportStatus_DAO>, IImportStatus
	{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnImportApplications_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnImportApplications_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnImportApplications_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnImportApplications_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }
}


