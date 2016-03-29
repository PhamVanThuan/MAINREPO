using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Rules.EstateAgent;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data;
using SAHL.Test;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.EstateAgent
{
    [TestFixture]
    public class EstateAgent : RuleBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            Messages = new DomainMessageCollection();
        }

        [TearDown]
        public new void TearDown()
        {
            base.TearDown();
        }

        #region LegalEntityEstateAgentMandatoryFieldsTest
        [Test]
        #region LegalEntityEstateAgentMandatoryFieldsTest
        public void LegalEntityEstateAgentMandatoryFieldsTest()
        {
            // need to write a helper to create either a legalentitynaturalperson or legalentitycompany.
            // Pass - id no.
            createLegalEntityNaturalPerson(0, "Test", "Test", "1234567", "", "test@sahomeloans.com", "1234567");
            // Pass - passport no.
            createLegalEntityNaturalPerson(0, "Test", "Test", "", "1234567", "test@sahomeloans.com", "1234567");
            // FAIL - Firstname //this is not being tested by this rule, so pass
            createLegalEntityNaturalPerson(0, "", "Test", "1234567", "1234567", "test@sahomeloans.com", "1234567");
            // FAIL - Surname //this is not being tested by this rule, so pass
            createLegalEntityNaturalPerson(0, "Test", "", "1234567", "1234567", "test@sahomeloans.com", "1234567");
            // FAIL - ID or Passport //this is not being tested by this rule, so pass
            createLegalEntityNaturalPerson(0, "Test", "Test", "", "", "test@sahomeloans.com", "1234567");
            // FAIL - email
            createLegalEntityNaturalPerson(1, "Test", "Test", "1234567", "1234567", "", "1234567");
            // FAIL - phonenumber
            createLegalEntityNaturalPerson(1, "Test", "Test", "1234567", "1234567", "test@sahomeloans.com", "");

            // PASS - ALL
            createLegalEntityCompany(0, "Test", "Test", "1234567", "test@sahomeloans.com", "1234567");
            // FAIL - TradingName //this is not being tested by this rule, so pass
            createLegalEntityCompany(0, "", "Test", "1234567", "test@sahomeloans.com", "1234567");
            // FAIL - RegisteredName //this is not being tested by this rule, so pass
            createLegalEntityCompany(0, "Test", "", "1234567", "test@sahomeloans.com", "1234567");
            // FAIL - RegistrationNumber //this is not being tested by this rule, so pass
            createLegalEntityCompany(0, "Test", "Test", "", "test@sahomeloans.com", "1234567");
            // FAIL - email
            createLegalEntityCompany(1, "Test", "Test", "1234567", "", "1234567");
            // FAIL - contactnumber
            createLegalEntityCompany(1, "Test", "Test", "1234567", "test@sahomeloans.com", "");

        }
        #endregion
        #region createLegalEntityNaturalPerson

        private void createLegalEntityNaturalPerson(int expectedMessageCount, string FirstNames, string Surname, string IDNo, string PPNo, string email, string CellNo)
        {
            LegalEntityEstateAgentMandatoryFields rule = new LegalEntityEstateAgentMandatoryFields();

            ILegalEntityNaturalPerson le = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(le.FirstNames).Return(FirstNames);
            SetupResult.For(le.Surname).Return(Surname);
            SetupResult.For(le.IDNumber).Return(IDNo);
            SetupResult.For(le.PassportNumber).Return(PPNo);
            SetupResult.For(le.EmailAddress).Return(email);
            SetupResult.For(le.CellPhoneNumber).Return(CellNo);
            SetupResult.For(le.HomePhoneCode).Return(CellNo);
            SetupResult.For(le.HomePhoneNumber).Return(CellNo);
            SetupResult.For(le.WorkPhoneCode).Return(CellNo);
            SetupResult.For(le.WorkPhoneNumber).Return(CellNo);

            ExecuteRule(rule, expectedMessageCount, le);
        }
        #endregion
        #region createLegalEntityCompany
        private void createLegalEntityCompany(int expectedMessageCount, string TradingName, string RegisteredName, string RegistrationNumber, string email, string CellNo)
        {
            LegalEntityEstateAgentMandatoryFields rule = new LegalEntityEstateAgentMandatoryFields();

            ILegalEntityCompany le = _mockery.StrictMock<ILegalEntityCompany>();
            SetupResult.For(le.TradingName).Return(TradingName);
            SetupResult.For(le.RegisteredName).Return(RegisteredName);
            SetupResult.For(le.RegistrationNumber).Return(RegistrationNumber);
            SetupResult.For(le.EmailAddress).Return(email);
            SetupResult.For(le.CellPhoneNumber).Return(CellNo);
            SetupResult.For(le.HomePhoneCode).Return(CellNo);
            SetupResult.For(le.HomePhoneNumber).Return(CellNo);
            SetupResult.For(le.WorkPhoneCode).Return(CellNo);
            SetupResult.For(le.WorkPhoneNumber).Return(CellNo);

            ExecuteRule(rule, expectedMessageCount, le);
        }
        #endregion
        #endregion

        #region LegalEntityEstateAgentMandatoryAddressTest
        [Test]
        #region LegalEntityEstateAgentMandatoryAddressTest
        public void LegalEntityEstateAgentMandatoryAddressTest()
        {
            // need to write a helper to create LegalEntityAddresses
            // Pass - both addresses
            createLegalEntityAddresses(0, (int)AddressTypes.Residential, (int)AddressTypes.Postal);
            // FAIL - no residential
            createLegalEntityAddresses(0, 0, (int)AddressTypes.Postal);
            // FAIL - no postal
            createLegalEntityAddresses(0, (int)AddressTypes.Residential, 0);

        }
        #endregion

        #region createLegalEntityNaturalPerson
        private void createLegalEntityAddresses(int expectedMessageCount, int residential, int postal)
        {
            LegalEntityEstateAgencyMandatoryAddress rule = new LegalEntityEstateAgencyMandatoryAddress();

            ILegalEntityCompany le = _mockery.StrictMock<ILegalEntityCompany>();
            IEventList<ILegalEntityAddress> leAddresses = new EventList<ILegalEntityAddress>();
            ILegalEntityAddress leAddressPhysical = _mockery.StrictMock<ILegalEntityAddress>();
            IAddressType addTypeRes = _mockery.StrictMock<IAddressType>();
            SetupResult.For(addTypeRes.Key).Return(residential);
            SetupResult.For(leAddressPhysical.AddressType).Return(addTypeRes);

            ILegalEntityAddress leAddressPostal = _mockery.StrictMock<ILegalEntityAddress>();
            IAddressType addTypePost = _mockery.StrictMock<IAddressType>();
            SetupResult.For(addTypePost.Key).Return(postal);
            SetupResult.For(leAddressPostal.AddressType).Return(addTypePost);

            leAddresses.Add(new DomainMessageCollection(), leAddressPhysical);
            leAddresses.Add(new DomainMessageCollection(), leAddressPostal);

            SetupResult.For(le.LegalEntityAddresses).Return(leAddresses);


            ExecuteRule(rule, expectedMessageCount, le);
        }
        #endregion
        #endregion



        #region LegalEntityEstateAgencyCheckForPrinciple
        [Test]
        #region LegalEntityEstateAgencyCheckForPrincipleTest
        public void LegalEntityEstateAgencyCheckForPrincipleTest()
        {
            IControlRepository conRepo = RepositoryFactory.GetRepository<IControlRepository>();
            IControl control = conRepo.GetControlByDescription("EstateAgentChannelRoot");

            // 2340 is the top level estate agency so we're just getting a company below that.
            string sql = String.Format(@"select top 1 os.OrganisationStructureKey, leos.legalentitykey from OrganisationStructure os
                                join LegalEntityOrganisationStructure leos on leos.OrganisationStructureKey = os.OrganisationStructureKey
                                where OrganisationTypeKey = 1 and ParentKey = {0}", control.ControlNumeric);

            DataTable DT = base.GetQueryResults(sql);
            Assert.That(DT.Rows.Count == 1);

            int EACompanyLEKey = Convert.ToInt32(DT.Rows[0][1]);

            LegalEntityEstateAgencyCheckForPrincipleTestHelper(0, true, EACompanyLEKey);

            LegalEntityEstateAgencyCheckForPrincipleTestHelper(1, false, EACompanyLEKey);
        }

        private void LegalEntityEstateAgencyCheckForPrincipleTestHelper(int expectedMessageCount, bool passFail, int EACompanyLEKey)
        {
            //LegalEntityEstateAgencyCheckForPrinciple rule = new LegalEntityEstateAgencyCheckForPrinciple();

            //CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            //IEstateAgentRepository repo = _mockery.StrictMock<IEstateAgentRepository>();
            //MockCache.Add(typeof(IEstateAgentRepository).ToString(), repo);
            //SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            //ILegalEntityCompany le = _mockery.StrictMock<ILegalEntityCompany>();
            //SetupResult.For(le.Key).Return(EACompanyLEKey);

            //ILegalEntity lePrinciple = _mockery.StrictMock<ILegalEntity>();
            //if (passFail)
            //    SetupResult.For(repo.GetPrincipleForEstateAgency(le)).IgnoreArguments().Return(lePrinciple);
            //else
            //    SetupResult.For(repo.GetPrincipleForEstateAgency(le)).IgnoreArguments().Return(null);

            //ExecuteRule(rule, expectedMessageCount, le);
            
            LegalEntityEstateAgencyCheckForPrinciple rule = new LegalEntityEstateAgencyCheckForPrinciple();

            IEstateAgentOrganisationNode eaon = _mockery.StrictMock<IEstateAgentOrganisationNode>();
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            IEstateAgentRepository repo = _mockery.StrictMock<IEstateAgentRepository>();


            MockCache.Add(typeof(IEstateAgentRepository).ToString(), repo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            ILegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();
                                   
            if (passFail)
                SetupResult.For(eaon.GetEstateAgentPrincipal()).IgnoreArguments().Return(lenp);
            else
                SetupResult.For(eaon.GetEstateAgentPrincipal()).IgnoreArguments().Return(null);



            ExecuteRule(rule, expectedMessageCount, eaon);



        }
        #endregion

        #endregion

        #region OneLegalEntityInstanceInOrgStructureTest
        //[Test]
        //[Ignore]
        //public void OneLegalEntityInstanceInOrgStructureTest()
        //{
        //    // Pass Tests

        //    // Fail Tests 
        //    // LegalEntity with org type 7 (individual)
        //    CheckForOneLegalEntityInstanceInOrgStructure(547206, 7, 1);
        //    // LegalEntity with org type 2 ()

        //}

        
        //private void CheckForOneLegalEntityInstanceInOrgStructure(int legalEntityKey, int orgTypeKey, int expectedMessageCount)
        //{

        //    OneLegalEntityInstanceInOrgStructure rule = new OneLegalEntityInstanceInOrgStructure();


        //    ILegalEntityOrganisationStructure leOrgStructure = _mockery.StrictMock<ILegalEntityOrganisationStructure>();
        //    IOrganisationStructure orgstructure = _mockery.StrictMock<IOrganisationStructure>();
        //    IOrganisationType orgtype = _mockery.StrictMock<IOrganisationType>();
        //    ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();

        //    SetupResult.For(orgtype.Key).Return(orgTypeKey);
        //    SetupResult.For(orgstructure.OrganisationType).Return(orgtype);
        //    SetupResult.For(legalEntity.Key).Return(legalEntityKey);
        //    SetupResult.For(leOrgStructure.LegalEntity).Return(legalEntity);
        //    SetupResult.For(leOrgStructure.OrganisationStructure).Return(orgstructure);

        //    ExecuteRule(rule, expectedMessageCount, leOrgStructure);


        //}
        #endregion


        #region LegelEntityEstateAgencyOnlyOnePrinciple
        [Test]
        #region LegelEntityEstateAgencyOnlyOnePrincipleTest
        public void LegelEntityEstateAgencyOnlyOnePrincipleTest()
        {
            LegelEntityEstateAgencyOnlyOnePrincipleTestHelper(1, true);

            LegelEntityEstateAgencyOnlyOnePrincipleTestHelper(0, false);
        }

        private void LegelEntityEstateAgencyOnlyOnePrincipleTestHelper(int expectedMessageCount, bool passFail)
        {
            LegelEntityEstateAgencyOnlyOnePrinciple rule = new LegelEntityEstateAgencyOnlyOnePrinciple();

            
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            IEstateAgentRepository repo = _mockery.StrictMock<IEstateAgentRepository>();

            MockCache.Add(typeof(IEstateAgentRepository).ToString(), repo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IEstateAgentOrganisationNode eaon = _mockery.StrictMock<IEstateAgentOrganisationNode>();
            IEstateAgentOrganisationNode parent = _mockery.StrictMock<IEstateAgentOrganisationNode>();

            SetupResult.For(eaon.Parent).Return(parent);

            IEventList<ILegalEntityOrganisationNode> childNodes = new EventList<ILegalEntityOrganisationNode>();

            ILegalEntityOrganisationNode child1 = _mockery.StrictMock<ILegalEntityOrganisationNode>();
            IOrganisationType ot1 = _mockery.StrictMock<IOrganisationType>();
            SetupResult.For(ot1.Key).Return((int)OrganisationTypes.Designation);
            SetupResult.For(child1.OrganisationType).Return(ot1);
            IGeneralStatus genStat = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(genStat.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(child1.GeneralStatus).Return(genStat);
            
            ILegalEntityOrganisationNode child2 = _mockery.StrictMock<ILegalEntityOrganisationNode>();
            IOrganisationType ot2 = _mockery.StrictMock<IOrganisationType>();
            SetupResult.For(ot2.Key).Return((int)OrganisationTypes.Designation);
            SetupResult.For(child2.OrganisationType).Return(ot1);
            SetupResult.For(child2.GeneralStatus).Return(genStat);
            SetupResult.For(child2.Description).Return(Constants.EstateAgent.Consultant);

            if (passFail)
            {
                SetupResult.For(child1.Description).Return(Constants.EstateAgent.Principal);
                
                IEventList<ILegalEntity> legEntities = new EventList<ILegalEntity>();
                ILegalEntity leg1 = _mockery.StrictMock<ILegalEntity>();
                ILegalEntity leg2 = _mockery.StrictMock<ILegalEntity>();
                legEntities.Add(new DomainMessageCollection(), leg1);
                legEntities.Add(new DomainMessageCollection(), leg2);
                //SetupResult.For(legEntities.Count).Return(2);

                IReadOnlyEventList<ILegalEntity> legEnts = new ReadOnlyEventList<ILegalEntity>(legEntities);

                SetupResult.For(child1.LegalEntities).Return(legEnts);
            }
            else
                SetupResult.For(child1.Description).Return(Constants.EstateAgent.Consultant);

            childNodes.Add(new DomainMessageCollection(), child1);
            childNodes.Add(new DomainMessageCollection(), child2);
            SetupResult.For(parent.ChildOrganisationStructures).IgnoreArguments().Return(childNodes);


            ExecuteRule(rule, expectedMessageCount, eaon);



        }
        #endregion

        #endregion

    }
}
