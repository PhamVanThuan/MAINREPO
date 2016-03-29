using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using presenter = SAHL.Web.Views.DebtCounselling.Interfaces;
using models = SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	/// <summary>
	/// Attorney Base
	/// </summary>
	public class AttorneyBase : SAHLCommonBasePresenter<presenter.IAttorney>
	{
		private CBOMenuNode node;
		private InstanceNode instanceNode;
		private int genericKeyTypeKey;
		protected int debtCounsellingKey;
		protected IDebtCounselling debtCounselling;
		private IOrganisationStructureRepository organisationStructureRepository;
		private IDebtCounsellingRepository debtCounsellingRepository;
		private ILegalEntityRepository legalEntityRepository;
		private ILookupRepository lookupRepository;

		public IOrganisationStructureRepository OrganisationStructureRepository
		{
			get
			{
				if (organisationStructureRepository == null)
				{
					organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
				}
				return organisationStructureRepository;
			}
		}
		public IDebtCounsellingRepository DebtCounsellingRepository
		{
			get
			{
				if (debtCounsellingRepository == null)
				{
					debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
				}
				return debtCounsellingRepository;
			}
		}
		public ILegalEntityRepository LegalEntityRepository
		{
			get
			{
				if (legalEntityRepository == null)
				{
					legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
				}
				return legalEntityRepository;
			}
		}
		public ILookupRepository LookupRepository
		{
			get
			{
				if (lookupRepository == null)
				{
					lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
				}
				return lookupRepository;
			}
		}

		/// <summary>
		/// Constructor for AttorneyUpdate
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public AttorneyBase(presenter.IAttorney view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		/// <summary>
		/// On View Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			#region Get the Node and Generic Key
			// get the cbo node

			node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

			if (node == null)
				throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

			//set the debt counsellingy key
			if (node is InstanceNode)
			{
				instanceNode = node as InstanceNode;
				genericKeyTypeKey = instanceNode.GenericKeyTypeKey;

				// Get values from the Debt Counselling Data 
				debtCounsellingKey = Convert.ToInt32(instanceNode.X2Data["DebtCounsellingKey"]);
			}
			else
			{
				genericKeyTypeKey = node.GenericKeyTypeKey;

				switch (genericKeyTypeKey)
				{
					case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
						debtCounsellingKey = node.GenericKey;
						break;
					default:
						break;
				}
			}

			if (debtCounsellingKey > 0)
				debtCounselling = DebtCounsellingRepository.GetDebtCounsellingByKey(debtCounsellingKey);
			#endregion

			_view.SelectedAttorneyName = debtCounselling.LitigationAttorney == null ? String.Empty : debtCounselling.LitigationAttorney.LegalEntity.DisplayName;
			_view.BindAttornies(GetLitigationAttorneys(), debtCounselling.LitigationAttorney == null ? -1 : debtCounselling.LitigationAttorney.LegalEntity.Key);
		}

		/// <summary>
		/// Get Litigation Attorneys
		/// </summary>
		/// <returns></returns>
		private IDictionary<int, string> GetLitigationAttorneys()
		{
			return DebtCounsellingRepository.GetLitigationAttorneys();
		}
	}
}