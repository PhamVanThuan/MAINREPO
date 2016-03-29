using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
    public partial class SubsidyProvider : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO>, ISubsidyProvider
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("SubsidyProviderAddUnique");
            Rules.Add("SubsidyProviderAddressMandatory");
            Rules.Add("SubsidyProviderLegalEntityType");
            Rules.Add("SubsidyProviderLegalEntityEmailAddress");
        }

        public static IReadOnlyEventList<ISubsidyProvider> GetSubsidyProviderAndParentByKey(IDomainMessageCollection Messages, int SubsidyProviderKey)
        {
            SubsidyProvider_DAO[] res = SubsidyProvider_DAO.FindAllByProperty("Key", SubsidyProviderKey);
            IEventList<ISubsidyProvider> list = new DAOEventList<SubsidyProvider_DAO, ISubsidyProvider, SubsidyProvider>(res);
            return new ReadOnlyEventList<ISubsidyProvider>(list);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnSubsidies_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnSubsidies_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnSubsidyProviders_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnSubsidyProviders_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}
	}
}


