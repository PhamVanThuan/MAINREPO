using System;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCommunicationDebtCounselling : ClientCommunicationBase
    {
        private int _debtCounsellingKey;
        private IDebtCounsellingRepository _debtCounsellingRepo;
        private IDebtCounselling _debtCounselling;

        protected override List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("ClientCommunicationDebtCounsellingEmail");
                    views.Add("ClientCommunicationDebtCounsellingSMS");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }


        public IDebtCounselling DebtCounselling
        {
            get
            {
                return _debtCounselling;
            }
            set
            {
                _debtCounselling = value;
            }
        }

        public int DebtCounsellingKey
        {
            get { return _debtCounsellingKey; }
        }

        public IDebtCounsellingRepository DebtCounsellingRepo
        {
            get
            {
                if (_debtCounsellingRepo == null)
                    _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _debtCounsellingRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ClientCommunicationDebtCounselling(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (base.GenericKeyTypeKey == (int)GenericKeyTypes.DebtCounselling2AM)
            {
                _debtCounsellingKey = base.GenericKey;
            }
            else if (base.GenericKeyTypeKey == (int)GenericKeyTypes.Account)
            {
                // walk up tree till we get a node with debtcounselling key
                SAHL.Common.UI.CBONode node = base.Node.GetParentNodeByType(GenericKeyTypes.DebtCounselling2AM);
                if (node != null)
                    _debtCounsellingKey = node.GenericKey;
            }

            if (_debtCounsellingKey > 0)
            {
                _debtCounselling = DebtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);

                // setup bank details - this is based on the origination source of the account
                ICorrespondenceTemplate template = null;
                if (_debtCounselling.Account.OriginationSource.Key == (int)OriginationSources.RCS)
                    template = CRepo.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.RCSBankDetails);
                else
                    template = CRepo.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.SAHLBankDetails);

                _view.BankDetails = String.Format(template.Template, _debtCounselling.Account.Key);

                // get list of DebtCounselling Clients
                foreach (var le in _debtCounselling.Clients)
                {
                    base.RecipientsList.Add(new BindableRecipient(le, "Debt Counselling Client"));
                }

                // get list of Main Applicants and/or Suretors - exclude guys who have already been loaded as DebtCounselling Clients above
                foreach (var role in _debtCounselling.Account.Roles)
                {
                    if (role.RoleType.Key == (int)RoleTypes.MainApplicant || role.RoleType.Key == (int)RoleTypes.Suretor)
                    {
                        // check if the le already exists as a debtcounselling client
                        var legalEntityCount = (from clientLE in _debtCounselling.Clients
                                                where clientLE.Key == role.LegalEntity.Key
                                                select clientLE.Key).FirstOrDefault<int>();

                        // if he doesnt exist then add 
                        if (legalEntityCount <= 0)
                            base.RecipientsList.Add(new BindableRecipient(role.LegalEntity, role.RoleType.Description));
                    }
                }


                if (_view.CorrespondenceMedium == CorrespondenceMediums.Email)
                {
                    // get the DebtCounsellor Contact and Company on the case
                    if (_debtCounselling.DebtCounsellor != null)
                        base.RecipientsList.Add(new BindableRecipient(_debtCounselling.DebtCounsellor, "Debt Counsellor"));
                    if (_debtCounselling.DebtCounsellorCompany != null)
                        base.RecipientsList.Add(new BindableRecipient(_debtCounselling.DebtCounsellorCompany, "Debt Counsellor Company"));

                    // get the Litigation Attorney Company
                    if (_debtCounselling.LitigationAttorney != null)
                    {
                        base.RecipientsList.Add(new BindableRecipient(_debtCounselling.LitigationAttorney.LegalEntity, "Litigation Attorney Company"));
                        // get the attorney contacts
                        foreach (var ac in _debtCounselling.LitigationAttorney.GetContacts(ExternalRoleTypes.DebtCounselling, GeneralStatuses.Active))
                        {
                            base.RecipientsList.Add(new BindableRecipient(ac, "Litigation Attorney"));
                        }
                    }

                    // get the PDA
                    if (_debtCounselling.PaymentDistributionAgent != null)
                        base.RecipientsList.Add(new BindableRecipient(_debtCounselling.PaymentDistributionAgent, "Payment Distribution Agent"));

                    // get list of NCR Depts (Only NCR Terminations & Complaints)
                    IOrganisationStructure osNCR = OSRepo.GetRootOrganisationStructureForDescription("National Credit Regulator");
                    if (osNCR != null)
                    {
                        ILegalEntityOrganisationNode leon = OSRepo.GetLegalEntityOrganisationNodeForKey(osNCR.Key);

                        foreach (var len in leon.ChildOrganisationStructures)
                        {
                            if (String.Compare(len.Description, "Legal Advisor", true) == 0) // exclude legal advisor
                                continue;

                            foreach (var le in len.LegalEntities)
                            {
                                base.RecipientsList.Add(new BindableRecipient(le, "NCR - " + len.Description));
                            }

                        }
                    }
                }
            }

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}
