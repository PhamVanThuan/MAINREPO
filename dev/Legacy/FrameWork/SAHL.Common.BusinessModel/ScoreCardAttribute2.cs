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
	/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO
	/// </summary>
	public partial class ScoreCardAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO>, IScoreCardAttribute
	{

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnScoreCardAttributeRanges_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnScoreCardAttributeRanges_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnScoreCardAttributeRanges_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnScoreCardAttributeRanges_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }
}


