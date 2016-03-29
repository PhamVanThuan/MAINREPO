using System;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class AddAccountMemoMessageOnReceiptOfApplicationCommandHandler : IHandlesDomainServiceCommand<AddAccountMemoMessageOnReceiptOfApplicationCommand>
    {
        private IApplicationRepository applicationRepository;
        private IOrganisationStructureRepository organisationStructureRepository;
        private IMemoRepository memoRepository;
        private ILookupRepository lookupRepository;
        private ICastleTransactionsService service;
        private ISAHLPrincipalCacheProvider principalCacheProvider;

        public AddAccountMemoMessageOnReceiptOfApplicationCommandHandler(IApplicationRepository applicationRepository,
                                                                        IOrganisationStructureRepository organisationStructureRepository,
                                                                        IMemoRepository memoRepository,
                                                                        ILookupRepository lookupRepository,
                                                                        ICastleTransactionsService service,
                                                                        ISAHLPrincipalCacheProvider principalCacheProvider)
        {
            this.applicationRepository = applicationRepository;
            this.organisationStructureRepository = organisationStructureRepository;
            this.memoRepository = memoRepository;
            this.lookupRepository = lookupRepository;
            this.service = service;
            this.principalCacheProvider = principalCacheProvider;
        }

        public void Handle(IDomainMessageCollection messages, AddAccountMemoMessageOnReceiptOfApplicationCommand command)
        {
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            string message = string.Format("Application: {0} Received and has been initially assigned to: {1}", command.ApplicationKey, command.AssignedTo);
            IMemo memo = memoRepository.CreateMemo();
            memo.ADUser = this.organisationStructureRepository.GetAdUserForAdUserName(command.AssignedTo);
            memo.GenericKeyType = this.lookupRepository.GenericKeyType.ObjectDictionary[Convert.ToString((int)GenericKeyTypes.Account)];
            memo.GenericKey = application.ReservedAccount.Key;
            memo.InsertedDate = DateTime.Now;

            ISAHLPrincipalCache SPC = this.principalCacheProvider.GetPrincipalCache();

            memo.Description = message;
            memo.GeneralStatus = this.lookupRepository.GeneralStatuses[GeneralStatuses.Active];

            try
            {
                SPC.ExclusionSets.Add(RuleExclusionSets.MemoExpiryDateExclude);
                SPC.ExclusionSets.Add(RuleExclusionSets.MemoMandatoryDateExclude);
                memoRepository.SaveMemo(memo);
            }
            catch
            {
                throw;
            }
            finally
            {
                SPC.ExclusionSets.Remove(RuleExclusionSets.MemoExpiryDateExclude);
                SPC.ExclusionSets.Remove(RuleExclusionSets.MemoMandatoryDateExclude);
            }

            // send an sms to the client.
            // Get the Main Applicant.
            string cellNo = string.Empty;
            foreach (IRole role in application.Account.Roles)
            {
                if (role.RoleType.Key == (int)OfferRoleTypes.PreviousInsurer)
                {
                    cellNo = role.LegalEntity.CellPhoneNumber;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(cellNo))
            {
                message = "Your application for further funds has been received. An S.A. Home Loans consultant will contact you shortly";

                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sahldb..clientemail (EMailBody, EmailAttachment1, CellPhone, LoanNumber, EmailInsertDate)");
                sql.AppendFormat(" values ('{0}', 'SMS', '{1}', {2}, getdate())", message, cellNo, command.ApplicationKey);

                service.ExecuteQueryOnCastleTran(sql.ToString(), SAHL.Common.Globals.Databases.TwoAM, null);
            }
        }
    }
}