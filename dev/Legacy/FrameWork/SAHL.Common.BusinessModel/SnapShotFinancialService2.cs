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
	/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO
	/// </summary>
    public partial class SnapShotFinancialService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO>, ISnapShotFinancialService
	{
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.ShotFinancialAdjustments
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        public void OnSnapShotFinancialAdjustments_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.ShotFinancialAdjustments
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        public void OnSnapShotFinancialAdjustments_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.ShotFinancialAdjustments
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        public void OnSnapShotFinancialAdjustments_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.ShotFinancialAdjustments
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        public void OnSnapShotFinancialAdjustments_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
	}
}


