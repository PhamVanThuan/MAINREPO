using System;
using System.Data;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.ITC;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.ITC
{
    [TestFixture]
    public class ITC : RuleBase
    {
        /// <summary>
        /// This interface is created for mocking purposes only, for rules that cast ILegalEntity objects
        /// to ILegalEntityNaturalPerson objects.
        /// </summary>
        public interface ILegalEntityLegalEntityNaturalPerson : ILegalEntity, ILegalEntityNaturalPerson
        {
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void ITCRequestLegalEntityNaturalPersonAddress_All()
        {
            ITCRequestLegalEntityNaturalPersonAddress rule = new ITCRequestLegalEntityNaturalPersonAddress();

            IITC itc = _mockery.StrictMock<IITC>();
            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();
            ILegalEntityAddress leAdd = _mockery.StrictMock<ILegalEntityAddress>();
            IAddressType addType = _mockery.StrictMock<IAddressType>();
            IAddress add = _mockery.StrictMock<IAddress>();
            IAddressFormat addFmt = _mockery.StrictMock<IAddressFormat>();
            IEventList<ILegalEntityAddress> leAddresses = new EventList<ILegalEntityAddress>();

            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.LegalEntityAddresses).Return(leAddresses);
            SetupResult.For(itc.LegalEntity).Return(lenp);

            //legal entity has no address, therefore fail
            ExecuteRule(rule, 1, itc);

            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.LegalEntityAddresses).Return(leAddresses);
            SetupResult.For(itc.LegalEntity).Return(lenp);

            SetupResult.For(addFmt.Key).Return((int)AddressFormats.Box);
            SetupResult.For(add.AddressFormat).Return(addFmt);
            SetupResult.For(leAdd.Address).Return(add);
            SetupResult.For(addType.Key).Return((int)AddressTypes.Residential);
            SetupResult.For(leAdd.AddressType).Return(addType);

            leAddresses.Add(Messages, leAdd);

            //Legalentity has address but only Box, so fail
            ExecuteRule(rule, 1, itc);

            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.LegalEntityAddresses).Return(leAddresses);
            SetupResult.For(itc.LegalEntity).Return(lenp);

            SetupResult.For(addFmt.Key).Return((int)AddressFormats.Street);
            SetupResult.For(add.AddressFormat).Return(addFmt);
            SetupResult.For(leAdd.Address).Return(add);
            SetupResult.For(addType.Key).Return((int)AddressTypes.Residential);
            SetupResult.For(leAdd.AddressType).Return(addType);

            leAddresses.Add(Messages, leAdd);

            //Legalentity has Street address, so pass
            ExecuteRule(rule, 0, itc);
        }

        [NUnit.Framework.Test]
        public void ITCRequestLegalEntityNaturalPersonIDNumber_All()
        {
            ITCRequestLegalEntityNaturalPersonIDNumber rule = new ITCRequestLegalEntityNaturalPersonIDNumber();

            IITC itc = _mockery.StrictMock<IITC>();

            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();

            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();

            // Fail - Invalid ID - SA Citizen
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("1111111111111");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 1, itc);

            // Fail - Invalid ID - Foreigner
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("1111111111111");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 1, itc);

            // Fail - No ID - SA Citizen
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 1, itc);

            // Pass - Valid ID - SA Citizen
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("0000000000000");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.SACitizen);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 0, itc);

            // Pass - Valid ID - Foreigner
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("0000000000000");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 0, itc);

            // Pass - No ID - Foreigner
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 0, itc);
        }

        [NUnit.Framework.Test]
        public void ITCRequestLegalEntityNaturalPersonIDNumberForeigner_All()
        {
            ITCRequestLegalEntityNaturalPersonIDNumberForeigner rule = new ITCRequestLegalEntityNaturalPersonIDNumberForeigner();

            IITC itc = _mockery.StrictMock<IITC>();

            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();

            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();

            // Fail - No ID - Foreigner
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 1, itc);

            // Pass - Valid ID - Foreigner
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.IDNumber).Return("0000000000000");
            SetupResult.For(citizenType.Key).Return((int)SAHL.Common.Globals.CitizenTypes.Foreigner);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            SetupResult.For(itc.LegalEntity).Return(lenp);
            ExecuteRule(rule, 0, itc);
        }

        [NUnit.Framework.Test]
        public void ITCRequestLegalEntityNaturalPerson_All()
        {
            ITCRequestLegalEntityNaturalPerson rule = new ITCRequestLegalEntityNaturalPerson();

            IITC itc = _mockery.StrictMock<IITC>();
            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();
            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();

            SetupResult.For(le.DisplayName).Return("Test LE not Natural Person");
            SetupResult.For(itc.LegalEntity).Return(le);

            //le will not cast to ILegalEntityNaturalPerson so fail
            ExecuteRule(rule, 1, itc);

            SetupResult.For(lenp.DisplayName).Return("Test LE Natural Person");
            SetupResult.For(itc.LegalEntity).Return(lenp);

            //le will cast to ILegalEntityNaturalPerson so pass
            ExecuteRule(rule, 0, itc);
        }

        [NUnit.Framework.Test]
        public void ITCRequestFrequency_All()
        {
            ITCRequestFrequency rule = new ITCRequestFrequency();

            int newAccKey = 1;
            int oldAccKey = 2;

            IITC newitc = _mockery.StrictMock<IITC>();
            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();
            IEventList<IITC> itcList = new EventList<IITC>();
            IITC olditc = _mockery.StrictMock<IITC>();
            IAccountSequence newAcc = _mockery.StrictMock<IAccountSequence>();
            IAccountSequence oldAcc = _mockery.StrictMock<IAccountSequence>();

            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.ITCs).Return(null);
            SetupResult.For(newitc.LegalEntity).Return(lenp);
            SetupResult.For(newitc.ReservedAccount).Return(newAcc);
            SetupResult.For(newAcc.Key).Return(newAccKey);

            //Le has no existing ITC, can create a new one, so pass
            ExecuteRule(rule, 0, newitc);

            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(lenp.ITCs).Return(itcList);
            SetupResult.For(newitc.LegalEntity).Return(lenp);
            SetupResult.For(newitc.ReservedAccount).Return(newAcc);
            SetupResult.For(newAcc.Key).Return(newAccKey);

            //Le has no existing ITC, can create a new one, so pass
            ExecuteRule(rule, 0, newitc);

            SetupResult.For(olditc.ChangeDate).Return(DateTime.Now.AddMonths(-5));
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            itcList.Add(Messages, olditc);
            SetupResult.For(lenp.ITCs).Return(itcList);
            SetupResult.For(newitc.LegalEntity).Return(lenp);
            SetupResult.For(newitc.ReservedAccount).Return(newAcc);
            SetupResult.For(newAcc.Key).Return(newAccKey);
            SetupResult.For(olditc.ReservedAccount).Return(oldAcc);
            SetupResult.For(oldAcc.Key).Return(newAccKey);

            //Le has existing ITC on file, with old date, can create a new one, so pass
            ExecuteRule(rule, 0, newitc);

            if (itcList.Contains(olditc))
                itcList.Remove(Messages, olditc);

            SetupResult.For(newitc.ReservedAccount).Return(newAcc);
            SetupResult.For(newAcc.Key).Return(newAccKey);
            SetupResult.For(olditc.ReservedAccount).Return(oldAcc);
            SetupResult.For(oldAcc.Key).Return(newAccKey);

            SetupResult.For(olditc.ChangeDate).Return(DateTime.Now);
            SetupResult.For(olditc.ResponseXML).Return("");
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            itcList.Add(Messages, olditc);
            SetupResult.For(lenp.ITCs).Return(itcList);
            SetupResult.For(newitc.LegalEntity).Return(lenp);

            //Le has existing ITC on file, not old, can not create a new one, so fail
            ExecuteRule(rule, 1, newitc);

            if (itcList.Contains(olditc))
                itcList.Remove(Messages, olditc);

            SetupResult.For(newitc.ReservedAccount).Return(newAcc);
            SetupResult.For(newAcc.Key).Return(newAccKey);
            SetupResult.For(olditc.ReservedAccount).Return(oldAcc);
            SetupResult.For(oldAcc.Key).Return(newAccKey);

            SetupResult.For(olditc.ChangeDate).Return(DateTime.Now);
            SetupResult.For(olditc.ResponseXML).Return("DisputeIndicator");
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            itcList.Add(Messages, olditc);
            SetupResult.For(lenp.ITCs).Return(itcList);
            SetupResult.For(newitc.LegalEntity).Return(lenp);

            //Le has existing ITC on file, not old, but with error in ResponseXML, so can create a new one, so pass
            ExecuteRule(rule, 0, newitc);
        }

        [NUnit.Framework.Test]
        public void ITCApplicationITCperLegalEntity_Pass()
        {
            string validID = "0000000000000";

            IAccountSequence accSeq = _mockery.StrictMock<IAccountSequence>();
            SetupResult.For(accSeq.Key).Return(1);

            ITCApplicationITCperLegalEntity rule = new ITCApplicationITCperLegalEntity();
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IEventList<IApplicationRole> appRolesTemp = new EventList<IApplicationRole>();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup appRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            IEventList<IITC> listITC = new EventList<IITC>();
            IITC itc = _mockery.StrictMock<IITC>();
            SetupResult.For(itc.ChangeDate).Return(DateTime.Now);
            SetupResult.For(itc.ResponseXML).Return(validID);
            SetupResult.For(itc.ReservedAccount).Return(accSeq);
            listITC.Add(Messages, itc);

            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();

            SetupResult.For(lenp.IDNumber).Return(validID);
            SetupResult.For(lenp.ITCs).Return(listITC);
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(appRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);
            SetupResult.For(appRoleType.ApplicationRoleTypeGroup).Return(appRoleTypeGroup);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRole.LegalEntity).Return(lenp);
            appRolesTemp.Add(Messages, appRole);

            IReadOnlyEventList<IApplicationRole> appRoles = new ReadOnlyEventList<IApplicationRole>(appRolesTemp);

            SetupResult.For(appML.ApplicationRoles).Return(appRoles);
            SetupResult.For(appML.ReservedAccount).Return(accSeq);

            ExecuteRule(rule, 0, appML);
        }

        [NUnit.Framework.Test]
        public void ITCApplicationITCperLegalEntity_Fail()
        {
            string validID = "0000000000000";
            string invalidID = "1111111111111";

            IAccountSequence accSeq = _mockery.StrictMock<IAccountSequence>();
            SetupResult.For(accSeq.Key).Return(1);

            ITCApplicationITCperLegalEntity rule = new ITCApplicationITCperLegalEntity();
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IEventList<IApplicationRole> appRolesTemp = new EventList<IApplicationRole>();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup appRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            IEventList<IITC> listITC = new EventList<IITC>();
            IITC itc = _mockery.StrictMock<IITC>();
            SetupResult.For(itc.ChangeDate).Return(DateTime.Now);
            SetupResult.For(itc.ResponseXML).Return(invalidID);
            SetupResult.For(itc.ReservedAccount).Return(accSeq);
            listITC.Add(Messages, itc);

            ILegalEntityLegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityLegalEntityNaturalPerson>();

            SetupResult.For(lenp.IDNumber).Return(validID);
            SetupResult.For(lenp.ITCs).Return(listITC);
            SetupResult.For(lenp.DisplayName).Return("Test LE");
            SetupResult.For(appRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);
            SetupResult.For(appRoleType.ApplicationRoleTypeGroup).Return(appRoleTypeGroup);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRole.LegalEntity).Return(lenp);
            appRolesTemp.Add(Messages, appRole);

            IReadOnlyEventList<IApplicationRole> appRoles = new ReadOnlyEventList<IApplicationRole>(appRolesTemp);

            SetupResult.For(appML.ApplicationRoles).Return(appRoles);
            SetupResult.For(appML.ReservedAccount).Return(accSeq);

            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void ITCAccountApplicationDisputeIndicated_Test()
        {
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                ITCAccountApplicationDisputeIndicated rule = new ITCAccountApplicationDisputeIndicated(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                //pass get top 1 for this
                //;  WITH XMLNAMESPACES( 'https://secure.transunion.co.za/TUBureau' AS  "TUBureau")
                //select   --isnull(le.FirstNames, '') + ' ' + isnull(le.Surname, '') + ' has a dispute on ITC.'
                //from ITC (nolock)    join LegalEntity le (nolock) on ITC.LegalEntityKey = le.LegalEntityKey
                //where ResponseXML.exist(N'BureauResponse/TUBureau:DisputeIndicatorDI01') = 0

                string sqlPass = ";  WITH XMLNAMESPACES( 'https://secure.transunion.co.za/TUBureau' AS  \"TUBureau\")     " +
                                    "select   top 1 ITC.AccountKey  " +
                                    "from ITC (nolock)    join LegalEntity le (nolock) on ITC.LegalEntityKey = le.LegalEntityKey  " +
                                    "join Account a (nolock) on ITC.AccountKey = a.AccountKey and a.AccountStatusKey = 1 " +
                                    "where ResponseXML.exist(N'BureauResponse/TUBureau:DisputeIndicatorDI01') = 0   ";

                using (IDbCommand cmd = dbHelper.CreateCommand(sqlPass))
                {
                    cmd.CommandTimeout = 300;

                    object obj = dbHelper.ExecuteScalar(cmd);
                    if (obj != null)
                    {
                        IAccount acc = AccRepo.GetAccountByKey((int)obj);
                        ExecuteRule(rule, 0, acc);
                    }
                }

                //fail get top 1 for this
                //;  WITH XMLNAMESPACES( 'https://secure.transunion.co.za/TUBureau' AS  "TUBureau")
                //select   --isnull(le.FirstNames, '') + ' ' + isnull(le.Surname, '') + ' has a dispute on ITC.'
                //from ITC (nolock)    join LegalEntity le (nolock) on ITC.LegalEntityKey = le.LegalEntityKey
                //where ResponseXML.exist(N'BureauResponse/TUBureau:DisputeIndicatorDI01') = 1
                //and AccountKey = @AccountKey
                string sqlFail = ";  WITH XMLNAMESPACES( 'https://secure.transunion.co.za/TUBureau' AS  \"TUBureau\")     " +
                                    "select   top 1 ITC.AccountKey  " +
                                    "from ITC (nolock)    join LegalEntity le (nolock) on ITC.LegalEntityKey = le.LegalEntityKey  " +
                                    "join Account a (nolock) on ITC.AccountKey = a.AccountKey and a.AccountStatusKey = 1 " +
                                    "where ResponseXML.exist(N'BureauResponse/TUBureau:DisputeIndicatorDI01') = 1   ";

                using (IDbCommand cmd = dbHelper.CreateCommand(sqlFail))
                {
                    cmd.CommandTimeout = 300;
                    object obj = dbHelper.ExecuteScalar(cmd);

                    if (obj != null)
                    {
                        IAccount acc = AccRepo.GetAccountByKey((int)obj);
                        ExecuteRule(rule, 1, acc);
                    }
                }
            }
        }

        #region ITCApplicationTest

        /// <summary>
        ///
        /// </summary>
        [NUnit.Framework.Test]
        public void ITCApplicationTest()
        {
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                ITCApplication rule = new ITCApplication(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                object obj = null;

                //Make a best attempt to get a < 80% ltv readvance to test a pass
                string sqlPassRALess80 = @"select top 1 o.offerkey
                                                from Account a (nolock)
                                                join FinancialService fs (nolock) on a.AccountKey = fs.AccountKey
                                                and fs.FinancialServiceTypeKey = 1 and fs.AccountStatusKey = 1
                                                join fin.balance bal (nolock) on fs.FinancialServiceKey = bal.FinancialServiceKey
                                                join fin.loanbalance lb (nolock) on  fs.FinancialServiceKey = lb.FinancialServiceKey
                                                join offer o (nolock) on a.AccountKey = o.AccountKey
                                                join (     select oi.offerkey, max(oi.offerinformationkey) as offerinformationkey
                                                from offer o (nolock)
                                                join offerinformation oi (nolock) on o.offerkey = oi.offerkey
                                                where o.offerstatuskey = 1 and o.offertypekey = 2
                                                group by oi.offerkey ) maxoi on o.offerkey = maxoi.offerkey
                                                join offerinformationvariableloan vl (nolock) on maxoi.offerinformationkey = vl.offerinformationkey
                                                where o.offerstatuskey = 1 and o.offertypekey = 2 and a.AccountStatusKey = 1
                                                and a.RRR_ProductKey != 2 and a.RRR_OriginationSourceKey != 4
                                                and lb.InitialBalance * 0.8 > (bal.amount + vl.LoanAgreementAmount)";

                using (IDbCommand cmd = dbHelper.CreateCommand(sqlPassRALess80))
                {
                    cmd.CommandTimeout = 90;

                    obj = dbHelper.ExecuteScalar(cmd);

                    if (obj != null)
                    {
                        using (new SessionScope())
                        {
                            IApplication app = AppRepo.GetApplicationByKey((int)obj);
                            ExecuteRule(rule, 0, app);
                        }
                    }
                }

                //Fail test for each LE must have ITC
                string sqlFailEveryLEITC = "select top 1 o.offerkey, o.reservedaccountkey from offer o (nolock) " +
                   "join offerrole ofr (nolock) on o.offerkey = ofr.offerkey and ofr.Offerroletypekey != 13 " +
                   "join offerroletype ort (nolock) on ofr.Offerroletypekey = ort.offerroletypekey and ort.offerroletypegroupkey = 3 " +
                   "join LegalEntity le (nolock) on ofr.LegalEntityKey = le.LegalEntityKey    " +
                   "and le.LegalEntityStatusKey = 1   and le.LegalEntityTypeKey = 2    " +
                   "and le.CitizenTypeKey not in (3,4,5,6,7,8)   " +
                   "left join itc (nolock) on ofr.LegalEntityKey = itc.LegalEntityKey " +
                   "where itc.AccountKey is null " +
                   "and o.offerTypekey != 2";

                using (IDbCommand cmd = dbHelper.CreateCommand(sqlPassRALess80))
                {
                    cmd.CommandTimeout = 90;

                    obj = dbHelper.ExecuteScalar(sqlFailEveryLEITC);

                    if (obj != null)
                    {
                        using (new SessionScope())
                        {
                            IApplication app = AppRepo.GetApplicationByKey((int)obj);
                            ExecuteRule(rule, 1, app);
                        }
                    }
                }

                //Pass test
                string sqlPass = "select top 1 o.offerkey " +
                   "from offer o (nolock) " +
                   "join offerrole ofr (nolock) on o.offerkey = ofr.offerkey and ofr.Offerroletypekey != 13 --exclude life roles " +
                   "join offerroletype ort (nolock) on ofr.Offerroletypekey = ort.offerroletypekey and ort.offerroletypegroupkey = 3 " +
                   "join LegalEntity le (nolock) on ofr.LegalEntityKey = le.LegalEntityKey    " +
                   "and le.LegalEntityStatusKey = 1 and le.LegalEntityTypeKey = 2    " +
                   "and le.CitizenTypeKey not in (3,4,5,6,7,8)  " +
                   "left join itc (nolock) on o.reservedaccountkey = itc.Accountkey " +
                   "where o.offerTypekey != 2 " +
                   "group by o.offerkey " +
                   "having Count(le.LegalEntityKey) = count(itc.AccountKey)";

                using (IDbCommand cmd = dbHelper.CreateCommand(sqlPassRALess80))
                {
                    cmd.CommandTimeout = 90;

                    obj = dbHelper.ExecuteScalar(sqlPass);

                    if (obj != null)
                    {
                        using (new SessionScope())
                        {
                            IApplication app = AppRepo.GetApplicationByKey((int)obj);
                            ExecuteRule(rule, 0, app);
                        }
                    }
                }
            }

            // }
            //// is re-advance and ltv is lower than 80% - PASS
            //ITCApplicationHelper(0, (int)ApplicationTypes.ReAdvance, 0.75);
            //ITCApplicationHelper(0, (int)ApplicationTypes.ReAdvance, 0.79);
        }

        /// <summary>
        /// Helper method to set up the expectations for the ITCApplication test.
        /// </summary>
        /// <param name="gs"></param>
        private void ITCApplicationHelper(int expectedMessageCount, int applicationType, double ltvValue)
        {
            ITCApplication rule = new ITCApplication(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationRepository appRepo = _mockery.StrictMock<IApplicationRepository>();
            MockCache.Add((typeof(IApplicationRepository)).ToString(), appRepo);

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(app.Account).Return(mla);
            IMortgageLoan vml = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(mla.SecuredMortgageLoan).Return(vml);

            SetupResult.For(vml.GetActiveValuationAmount()).IgnoreArguments().Return(45000.00);
            SetupResult.For(vml.CurrentBalance).Return(10000.00);
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();

            IEventList<IApplicationInformation> appInfos = new EventList<IApplicationInformation>();
            IApplicationInformation appInfo = _mockery.StrictMock<IApplicationInformation>();

            IApplicationInformationVariableLoan appInfVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            SetupResult.For(appInfVarLoan.LoanAgreementAmount).Return(10000.00);
            SetupResult.For(appInfVarLoan.LTV).Return(ltvValue);
            SetupResult.For(appRepo.GetApplicationInformationVariableLoan(1)).IgnoreArguments().Return(appInfVarLoan);
            SetupResult.For(appInfo.Key).Return(1);

            //SetupResult.For(LoanCalculator.CalculateLTV(1, 1)).IgnoreArguments().Return(0.79);

            appInfos.Add(new DomainMessageCollection(), appInfo);
            SetupResult.For(app.ApplicationInformations).Return(appInfos);

            SetupResult.For(appType.Key).Return(applicationType);
            SetupResult.For(app.ApplicationType).Return(appType);

            ExecuteRule(rule, expectedMessageCount, app);
        }

        #region Repositories

        private IAccountRepository accRepo;

        public IAccountRepository AccRepo
        {
            get
            {
                if (accRepo == null)
                    accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return accRepo;
            }
        }

        private IApplicationRepository appRepo;

        public IApplicationRepository AppRepo
        {
            get
            {
                if (appRepo == null)
                    appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return appRepo;
            }
        }

        #endregion Repositories

        #endregion ITCApplicationTest

        #region ITCApplicationEmpiricaScore

        [NUnit.Framework.Test]
        public void ITCApplicationEmpiricaScoreTest()
        {
            string sqlPassRALess80 = @";WITH XMLNAMESPACES('https://secure.transunion.co.za/TUBureau' AS ""TUBureau"") " +
                @"select top 1 o.offerKey " +
                @"from [2AM].dbo.Offer o (nolock) " +
                @"join (select max(offerInformationKey) offerInformationKey, offerKey " +
                @"		from [2AM].dbo.OfferInformation (nolock) " +
                @"		group by offerKey) maxoi on maxoi.offerKey = o.offerKey " +
                @"join [2AM].dbo.OfferInformationVariableLoan oivl (nolock) on maxoi.offerInformationKey = oivl.offerInformationKey " +
                @"	and oivl.categoryKey = 0 " +
                @"join [2AM].dbo.OfferRole ofr (nolock) on o.offerKey = ofr.OfferKey " +
                @"	and offerRoleTypeKey in (8,10,11,12) " +
                @"	and ofr.generalStatusKey = 1 " +
                @"join [2AM].dbo.OfferRoleAttribute ora (nolock) on ofr.offerRoleKey = ora.offerRoleKey " +
                @"	and ora.OfferRoleAttributeTypeKey = 1 " +
                @"join [2AM].dbo.ITC i (nolock) ON ofr.LegalEntityKey = i.LegalEntityKey " +
                @"	and o.ReservedAccountKey = i.AccountKey " +
                @"	and isnumeric(ResponseXML.value(N'(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore)[1]', 'int')) = 1 " +
                @"	and ResponseXML.value(N'(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore)[1]', 'int') > 575 " +
                @"where o.offerStartDate > dateadd(m, -6, getdate()) " +
                @"and o.offerStatusKey = 1 " +
                @"order by i.ITCKey desc ";

            ITCApplicationEmpiricaScore rule = new ITCApplicationEmpiricaScore(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            object obj = null;
            int appKey = 0;

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                using (IDbCommand cmd = dbHelper.CreateCommand(sqlPassRALess80))
                {
                    cmd.CommandTimeout = 180;

                    obj = dbHelper.ExecuteScalar(cmd);

                    if (obj != null)
                    {
                        if (!Int32.TryParse(obj.ToString(), out appKey))
                            Assert.Fail(String.Format(@"Error parsing offerkey: {0} to Int32", obj.ToString()));
                    }
                    else
                        Assert.Inconclusive("no data");
                }
            }

            if (appKey > 0)
            {
                using (new SessionScope())
                {
                    IApplication app = AppRepo.GetApplicationByKey(appKey);
                    ExecuteRule(rule, 0, app);
                }
            }
            else
                Assert.Inconclusive("no data");
        }

        #endregion ITCApplicationEmpiricaScore
    }
}