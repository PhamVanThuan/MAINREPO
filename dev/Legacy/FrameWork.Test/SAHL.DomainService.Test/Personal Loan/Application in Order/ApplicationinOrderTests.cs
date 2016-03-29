using System;
using System.Data;
using Castle.ActiveRecord;
using DomainService2;
using DomainService2.Workflow.PersonalLoan;
using NUnit.Framework;

namespace SAHL.DomainService.Test.Personal_Loan.Application_in_Order
{
    [TestFixture]
    public class ApplicationinOrderTests : DomainServiceTestBase
    {
        [Ignore("used for dev testing only")]
        [Test]
        public void OnCompleteActivity_Application_in_Order()
        {
//            string sql = String.Format(@"select top 1 o.OfferKey
//                from [2am].dbo.Offer o
//                join [2am].dbo.ExternalRole er (nolock) on er.GenericKey = o.OfferKey
//	                and er.GenericKeyTypeKey = 2
//	                and er.GeneralStatusKey = 1
//                join [2am].dbo.LegalEntity le (nolock) on le.LegalEntityKey = er.LegalEntityKey
//                join [2am].dbo.ExternalRoleType ert (nolock) on ert.ExternalRoleTypeKey = er.ExternalRoleTypeKey
//                    and ert.ExternalRoleTypeGroupKey = 1
//                left join [2am].dbo.ExternalRoleDomicilium erd on erd.ExternalRoleKey = er.ExternalRoleKey
//                join [2am].dbo.LegalEntityAddress lea (nolock) on lea.LegalEntityKey = er.LegalEntityKey
//	                and lea.GeneralStatusKey = 1
//	                and lea.AddressTypeKey = 1
//                join [2am].dbo.LegalEntityDomicilium led (nolock) on led.LegalEntityAddressKey = lea.LegalEntityAddressKey
//	                and led.GeneralStatusKey = 1
//                join x2.X2DATA.Personal_Loans pl (nolock) on pl.ApplicationKey = o.OfferKey
//                join x2.x2.instance i on i.id = pl.InstanceID
//                join x2.x2.[state] s on s.id = i.stateid
//	                and s.Name = 'Manage Application'
//                where o.OfferTypeKey = 11
//                and erd.ExternalRoleDomiciliumKey is null");

//            DataTable dt = GetQueryResults(sql);

            //if (dt.Rows.Count == 0)
            //    Assert.Ignore("No data");

            int appKey = 1370365; //Convert.ToInt32(dt.Rows[0]["OfferKey"]);
            //dt.Dispose();

            CheckCreditSubmissionRulesCommand checkCreditSubmissionRulesCommand;
            IHandlesDomainServiceCommand<CheckCreditSubmissionRulesCommand> checkCreditSubmissionRulesCommandHandler = loader.DomainServiceIOC.GetCommandHandler<CheckCreditSubmissionRulesCommand>();

            using (new SessionScope(FlushAction.Never))
            {
                using (new TransactionScope(OnDispose.Rollback))
                {
                    checkCreditSubmissionRulesCommand = new CheckCreditSubmissionRulesCommand(appKey, true);
                    try
                    {
                        checkCreditSubmissionRulesCommandHandler.Handle(messages, checkCreditSubmissionRulesCommand);
                    }
                    catch
                    {
                        // fail
                    }
                }
            }
        }
    }
}