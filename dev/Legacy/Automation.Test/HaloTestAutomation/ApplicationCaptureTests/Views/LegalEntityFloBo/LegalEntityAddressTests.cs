using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    /// <summary>
    /// Contains tests for legal entity addresses
    /// </summary>
    [RequiresSTA]
    public class LegalEntityAddressTests : TestBase<LegalEntityAddressDetails>
    {
        /// <summary>
        /// OfferKey for tests
        /// </summary>
        private Random randomNumber = new Random();

        private int legalEntityKey;
        private int offerKey;
        /// <summary>
        /// Database Connection to use
        /// </summary>

        #region Setup/Teardown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = Helper.CreateApplicationWithBrowser(TestUsers.BranchConsultant1, out offerKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, offerKey);

            var idNumber = String.Empty;
            Helper.AddNaturalPersonApplicantToOffer(base.Browser, offerKey, out idNumber, false);
            legalEntityKey = Service<ILegalEntityService>().GetLegalEntityKeyByIdNumber(idNumber);

            //Make the same legalentity play a role on another application
            var legalEntityRelatedOfferKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType
                (
                    WorkflowStates.ApplicationCaptureWF.ApplicationCapture,
                    Workflows.ApplicationCapture,
                    OfferTypeEnum.SwitchLoan,
                    ""
                );
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(legalEntityRelatedOfferKey);
            Service<IApplicationService>().CreateOfferRole(legalEntityKey, legalEntityRelatedOfferKey, OfferRoleTypeEnum.LeadMainApplicant, GeneralStatusEnum.Active);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
        }

        #endregion Setup/Teardown

        #region Tests

        /// <summary>
        /// This test will add a residential street address to a legal entity
        /// </summary>
        [Test, Description("This test will add a residential street address to a legal entity")]
        public void _001_AddResidentialAddressStreet()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            //need a random number
            var address = new Automation.DataModels.Address
                            {
                                StreetNumber = randomNumber.Next(0, 100000).ToString(),
                                StreetName = string.Format("Test Street {0}", randomNumber.Next(0, 100000)),
                                RRR_SuburbDescription = "Kloofview Road",
                                RRR_ProvinceDescription = Province.Kwazulunatal,
                                UnitNumber = "0",
                                BuildingName = "",
                                BuildingNumber = "0"
                            };
            base.View.AddStreetAddress(address, AddressType.Residential);
            int addressKey = AddressAssertions.AssertResidentialAddressRecordExists(address.StreetNumber, address.StreetName, address.RRR_ProvinceDescription,
                address.RRR_SuburbDescription);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Residential);
        }

        /// <summary>
        /// This test ensures that you can update a residential street address using the Update Legal Entity Address
        /// </summary>
        [Test, Description("This test ensures that you can update a residential street address using the Update Legal Entity Address")]
        public void _002_UpdateResidentialAddress()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Update);
            //we need a record to update
            var address = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey).First();
            var residentialAddress = new Address
                                         {
                                             StreetName = string.Format("Test Street 2 {0}", randomNumber.Next(0, 100000)),
                                             StreetNumber = randomNumber.Next(0, 100000).ToString(),
                                             RRR_SuburbDescription = "Kloofview Road",
                                             RRR_ProvinceDescription = Province.Kwazulunatal,
                                             UnitNumber = "0",
                                             BuildingName = "",
                                             BuildingNumber = "0"
                                         };
            base.View.SelectAndUpdateResidentialStreetAddress(address.DelimitedAddress, residentialAddress);
            int addressKey = AddressAssertions.AssertResidentialAddressRecordExists(residentialAddress.StreetNumber, residentialAddress.StreetName, residentialAddress.RRR_ProvinceDescription,
                        residentialAddress.RRR_SuburbDescription);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// This test will add a free text address to a legal entity
        /// </summary>
        [Test, Description("This test will add a free text address to a legal entity")]
        public void _003_AddResidentialAddressFreeText()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            var freeTextAddress = new Address
                                        {
                                            Line1 = string.Format(@"Line 1 {0}", randomNumber.Next(0, 10000)),
                                            Line2 = string.Format(@"Line 2 {0}", randomNumber.Next(0, 10000)),
                                            Line3 = string.Format(@"Line 3 {0}", randomNumber.Next(0, 10000)),
                                            Line4 = string.Format(@"Line 4 {0}", randomNumber.Next(0, 10000)),
                                            Line5 = string.Format(@"Line 5 {0}", randomNumber.Next(0, 10000)),
                                            RRR_CountryDescription = "United Kingdom"
                                        };

            base.View.AddResidentialAddressFreeText(freeTextAddress);
            int addressKey;
            AddressAssertions.AssertFreeTextAddressRecordExists(freeTextAddress.Line1, freeTextAddress.Line2, freeTextAddress.Line3, freeTextAddress.Line4,
                freeTextAddress.Line5, out addressKey);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Residential);
        }

        /// <summary>
        /// This test ensures that a user can add a postal address with a street address format.
        /// </summary>
        [Test, Description("This test ensures that a user can add a postal address with the a street address format.")]
        public void _004_AddPostalAddressStreet()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            var address = new Address
                                {
                                    StreetNumber = randomNumber.Next(0, 100000).ToString(),
                                    StreetName = string.Format("Test Street {0}", randomNumber.Next(0, 100000)),
                                    RRR_SuburbDescription = "Kloofview Road",
                                    RRR_ProvinceDescription = Province.Kwazulunatal,
                                    UnitNumber = "0",
                                    BuildingName = "",
                                    BuildingNumber = "0"
                                };
            base.View.AddStreetAddress(address, AddressType.Postal);
            int addressKey = AddressAssertions.AssertResidentialAddressRecordExists(address.StreetNumber, address.StreetName,
                address.RRR_ProvinceDescription, address.RRR_SuburbDescription);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Postal);
        }

        /// <summary>
        /// This test ensures that a user can add a postal address with a PO Box format.
        /// </summary>
        [Test, Description("This test ensures that a user can add a postal address with a PO Box format.")]
        public void _005_AddPostalAddressPostalBox()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            var postalAddress = new Address
                                    {
                                        BoxNumber = randomNumber.Next(0, 10000).ToString(),
                                        RRR_SuburbDescription = "Kloofview Road",
                                        PostalCode = ""
                                    };
            base.View.AddPostalBoxAddress(postalAddress);
            int addressKey;
            AddressAssertions.AssertPOBoxAddressRecordExists(postalAddress.BoxNumber, postalAddress.RRR_SuburbDescription, out addressKey);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Postal);
        }

        /// <summary>
        /// This test ensures that a cluster box address can be added
        /// </summary>
        [Test, Description("This test ensures that a cluster box address can be added")]
        public void _006_AddPostalAddressClusterBox()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            var clusterBox = new Address
                                {
                                    BoxNumber = randomNumber.Next(0, 10000).ToString(),
                                    RRR_SuburbDescription = "HAAKDOORNBOOM"
                                };
            base.View.AddClusterBoxAddress(clusterBox);
            int addressKey;
            AddressAssertions.AssertClusterBoxAddressRecordExists(clusterBox.BoxNumber, clusterBox.RRR_SuburbDescription, out addressKey);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Postal);
        }

        /// <summary>
        /// This test ensures that a postNet suite address can be added
        /// </summary>
        [Test, Description("This test ensures that a postNet suite address can be added")]
        public void _007_AddPostalAddressPostNetSuite()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            var postNet = new Address
                                {
                                    SuiteNumber = randomNumber.Next(0, 50000).ToString(),
                                    PrivateBag = string.Format(@"X{0}", randomNumber.Next(0, 50000)),
                                    RRR_SuburbDescription = "Kloofview Road"
                                };
            base.View.AddPostNetSuiteAddress(postNet);
            int addressKey;
            AddressAssertions.AssertPostNetAddressRecordExists(postNet.SuiteNumber, postNet.PrivateBag, postNet.RRR_SuburbDescription, out addressKey);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Postal);
        }

        /// <summary>
        /// This test ensures that a user can add a Private Bag Address
        /// </summary>
        [Test, Description("This test ensures that a user can add a Private Bag Address")]
        public void _008_AddPostalAddressPrivateBag()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
            var privateBag = new Address
                                {
                                    PrivateBag = string.Format(@"X{0}", randomNumber.Next(0, 50000)),
                                    RRR_SuburbDescription = "Kloofview Road"
                                };
            base.View.AddPrivateBagAddress(privateBag);
            int addressKey;
            AddressAssertions.AssertPrivateBagAddressRecordExists(privateBag.PrivateBag, privateBag.RRR_SuburbDescription, out addressKey);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(legalEntityKey, addressKey, AddressType.Postal);
        }

        /// <summary>
        /// This test ensures that we can delete a Legal Entity Address
        /// </summary>
        [Test, Description("This test ensures that we can delete a Legal Entity Address")]
        public void _009_DeleteLegalEntityAddress()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Delete);
            //we need to select the record and delete it.
            var address = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey).First();
            base.View.DeleteAddress(address.DelimitedAddress);
            //the link should now be inactive
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(legalEntityKey, address.AddressKey, GeneralStatusEnum.Inactive);
        }

        /// <summary>
        /// This test will ensure that the only legal entity address against an applicant cannot be removed.
        /// </summary>
        [Test, Description("This test will ensure that the only legal entity address against an applicant cannot be removed.")]
        public void _100_CannotRemoveOnlyLegalEntityAddress()
        {
            Service<ILegalEntityAddressService>().SetupAddressData(legalEntityKey);
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Delete);
            //we need to select the record and delete it.
            var address = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey).First();
            base.View.DeleteAddress(address.DelimitedAddress);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The address cannot be removed on a MainApplicant or Suretor when it is the only Legal Entity Address.");
        }

        /// <summary>
        /// This test will ensure that the legal entity address that is being used as the Application Mailing Address cannot be removed.
        /// </summary>
        [Test, Description("This test will ensure that the legal entity address that is being used as the Application Mailing Address cannot be removed.")]
        public void _101_CannotRemoveLegalEntityAddressMailingAddress()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Delete);
            Service<ILegalEntityAddressService>().SetupAddressAsApplicationMailingAddress(legalEntityKey, offerKey);
            //we need to select the record and delete it.
            var address = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey).First();
            base.View.DeleteAddress(address.DelimitedAddress);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"The address cannot be removed from the legal entity as it is the mailing address for an open application ({0})", offerKey));
        }

        [Test]
        public void _102_when_deleting_a_legalEntityAddress_linked_to_a_pending_domicilium_the_user_is_not_allowed_to_continue()
        {
            var formattedAddress = SetupPendingLegalEntityOfferDomiciliumAddresses();
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Delete);
            base.View.DeleteAddress(formattedAddress);

            var legalName = Service<ILegalEntityService>().GetLegalEntityLegalName(legalEntityKey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                              String.Format(@"This address is linked to the Pending domicilium address for {0} and cannot be deleted.", legalName));
        }

        [Test]
        public void _103_when_updating_a_legalentity_address_linked_to_multilpe_pending_offer_domiciliums_should_get_a_warning_for_every_offer()
        {
            var formattedAddress = SetupPendingLegalEntityOfferDomiciliumAddresses();
            base.Browser.Navigate<LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Update);
            var addressToCapture = new Automation.DataModels.Address
            {
                StreetNumber = randomNumber.Next(0, 100000).ToString(),
                StreetName = string.Format("Test Street {0}", randomNumber.Next(0, 100000)),
                RRR_SuburbDescription = "Kloofview Road",
                RRR_ProvinceDescription = Province.Kwazulunatal,
                UnitNumber = "0",
                BuildingName = "",
                BuildingNumber = "0"
            };
            base.View.SelectAndUpdateResidentialStreetAddress(formattedAddress, addressToCapture);
            var legalName = Service<ILegalEntityService>().GetLegalEntityLegalName(legalEntityKey);

            var offerRolesByLe = from or in base.Service<IApplicationService>().GetActiveOfferRolesByLegalEntityKey(legalEntityKey, OfferRoleTypeGroupEnum.Client)
                                 where or.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife
                                 select or;
            //Should get warning for every application the legalentity plays a role in.
            foreach (var or in offerRolesByLe)
            {
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                         String.Format(@"{0} has elected to use this address as the proposed domicilium on application {1}, updating will change the domicilium on this application.", legalName, or.OfferKey));
            }
        }

        private string SetupPendingLegalEntityOfferDomiciliumAddresses()
        {
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(offerKey);
            //Remove legalentityaddresses so the we can create new ones
            Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, true, GeneralStatusEnum.Active);
            var leAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);

            var offerRolesByLe = from or in base.Service<IApplicationService>().GetActiveOfferRolesByLegalEntityKey(legalEntityKey, OfferRoleTypeGroupEnum.Client)
                                 where or.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife
                                 select or;
            foreach (var or in offerRolesByLe)
            {
                var legalentityDom = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(leAddress.LegalEntityAddressKey, legalEntityKey, GeneralStatusEnum.Pending);
                var offerRoleDomicilium = Service<IApplicationService>().InsertOfferRoleDomicilium(legalentityDom.LegalEntityDomiciliumKey, or.OfferRoleKey, or.OfferKey);
            }
            return leAddress.DelimitedAddress;
        }

        #endregion Tests
    }
}