using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO
    /// </summary>
    public partial class DebtCounselling : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO>, IDebtCounselling
    {
        private IDebtCounsellingRepository _debtCounsellingRepository;

        public IDebtCounsellingRepository DebtCounsellingRepository
        {
            get
            {
                if (_debtCounsellingRepository == null)
                {
                    _debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                }
                return _debtCounsellingRepository;
            }
        }

        private IOrganisationStructureRepository _organisationStructureRepository;

        public IOrganisationStructureRepository OrganisationStructureRepository
        {
            get
            {
                if (_organisationStructureRepository == null)
                {
                    _organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                }
                return _organisationStructureRepository;
            }
        }

        public IList<ILegalEntity> Clients
        {
            get { return ExternalRoleHelper.GetExternalRoleList(_DAO.Key, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.Client, GeneralStatuses.Active); }
        }

        public ILegalEntity DebtCounsellor
        {
            get
            {
                return DebtCounsellingRepository.GetDebtCounsellorForDebtCounselling(_DAO.Key);
            }
        }

        public ILegalEntityOrganisationStructure DebtCounsellorLEOrganisationStructure
        {
            get
            {
                IList<ILegalEntityOrganisationStructure> leosList = OrganisationStructureRepository.GetLegalEntityOrganisationStructuresForLegalEntityKey(this.DebtCounsellor.Key);

                if (leosList != null && leosList.Count > 0)
                    return leosList[0];

                return null;
            }
        }

        /// <summary>
        /// Get Payment Distribution Agent Legal Organisation Structure
        /// </summary>
        public ILegalEntityOrganisationStructure PaymentDistributionAgentLEOrganisationStructure
        {
            get
            {
                IList<ILegalEntityOrganisationStructure> leosList = OrganisationStructureRepository.GetLegalEntityOrganisationStructuresForLegalEntityKey(this.PaymentDistributionAgent.Key);

                if (leosList != null && leosList.Count > 0)
                    return leosList[0];

                return null;
            }
        }

        public IAttorney LitigationAttorney
        {
            get
            {
                ILegalEntity le = ExternalRoleHelper.GetExternalRole(_DAO.Key, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.LitigationAttorney, GeneralStatuses.Active);
                if (le != null)
                    return OrganisationStructureRepository.GetAttorneyByLegalEntityKey(le.Key);
                else
                    return null;
            }
        }

        public ILegalEntity DebtCounsellorCompany
        {
            get
            {
                ILegalEntity debtCounsellorCompany = null;
                if (this.DebtCounsellor != null)
                    debtCounsellorCompany = DebtCounsellingRepository.GetDebtCounsellorCompanyForDebtCounsellor(this.DebtCounsellor.Key);

                return debtCounsellorCompany;
            }
        }

        public ILegalEntity PaymentDistributionAgent
        {
            get { return ExternalRoleHelper.GetExternalRole(_DAO.Key, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.PaymentDistributionAgent, GeneralStatuses.Active); }
        }

        public ILegalEntity NationalCreditRegulator
        {
            get { return ExternalRoleHelper.GetExternalRole(_DAO.Key, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.NationalCreditRegulator, GeneralStatuses.Active); }
        }

        public IProposal GetActiveProposal(ProposalTypes proposalType)
        {
            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            IList<IProposal> proposals = dcRepo.GetProposalsByTypeAndStatus(this.Key, proposalType, ProposalStatuses.Active);
            if (proposals == null || proposals.Count == 0)
                return null;
            else
                return proposals[0];
        }

        /// <summary>
        /// Get the Accepted Active Proposal for the Debt Counselling case
        /// </summary>
        public IProposal AcceptedActiveProposal
        {
            get
            {
                SimpleQuery<Proposal_DAO> q = new SimpleQuery<Proposal_DAO>(QueryLanguage.Hql,
                        @"
                        from Proposal_DAO p
                        where p.Accepted = 1
                        and p.ProposalStatus.Key = 1
                        and p.ProposalType.Key = 1
                        and p.DebtCounselling.Key = ? ", this.Key);

                q.SetQueryRange(1);

                q.AddSqlReturnDefinition(typeof(Proposal_DAO), "P");

                Proposal_DAO[] pDAO = q.Execute();

                if (pDAO != null && pDAO.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProposal, Proposal_DAO>(pDAO[0]);
                }

                return null;
            }
        }

        #region HearingDetail

        /// <summary>
        ///
        /// </summary>
        public IList<IHearingDetail> GetActiveHearingDetails
        {
            get
            {
                string HQL = "from HearingDetail_DAO where DebtCounselling.Key = ? and GeneralStatus.Key = ? ";

                SimpleQuery<HearingDetail_DAO> q = new SimpleQuery<HearingDetail_DAO>(HQL, this.Key, (int)GeneralStatuses.Active);
                HearingDetail_DAO[] daoList = q.Execute();

                IEventList<IHearingDetail> list = new DAOEventList<HearingDetail_DAO, IHearingDetail, SAHL.Common.BusinessModel.HearingDetail>(daoList);
                IList<IHearingDetail> hdList = list.ToList<IHearingDetail>();

                return hdList;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnHearingDetails_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnHearingDetails_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnHearingDetails_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnHearingDetails_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        #endregion HearingDetail

        //public override void Refresh()
        //{
        //    base.Refresh();
        //    _hearingDetails = null;
        //    _Proposals = null;
        //}

        private void OnProposals_AfterRemove(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        private void OnProposals_AfterAdd(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        private void OnProposals_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        private void OnProposals_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }
    }
}