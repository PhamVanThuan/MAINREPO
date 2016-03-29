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
	public partial class UserOrganisationStructure : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO>, IUserOrganisationStructure
	{

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("UserOrganisationStructureDistinctCheck");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnUserOrganisationStructureRoundRobinStatus_BeforeAdd(ICancelDomainArgs args, object Item)
        {

    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnUserOrganisationStructureRoundRobinStatus_BeforeRemove(ICancelDomainArgs args, object Item)
        {

}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnUserOrganisationStructureRoundRobinStatus_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnUserOrganisationStructureRoundRobinStatus_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }
}


