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
	/// SAHL.Common.BusinessModel.DAO.ProposalStatus_DAO
	/// </summary>
	public partial class ProposalStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ProposalStatus_DAO>, IProposalStatus
	{


        public void Proposals_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        public void Proposals_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        public void Proposals_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        public void Proposals_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }


	}
}


