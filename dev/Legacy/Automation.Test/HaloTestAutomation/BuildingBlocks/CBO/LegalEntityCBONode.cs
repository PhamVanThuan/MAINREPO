using Automation.DataAccess.DataHelper;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using Common.Constants;
using Common.Enums;
using System;
namespace BuildingBlocks.CBO
{
    public sealed class LegalEntityCBONode
    {
        /// <summary>
        /// This is a grouping of BuildingBlocks methods that can be called when a test needs to add data to an application using
        /// the Legal Entity node.
        /// </summary>
        /// <param name="browser">IE TestBrowser Object</param>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="declarations">True=add, False=do not add</param>
        /// <param name="employment">True=add, False=do not add</param>
        /// <param name="address">True=add, False=do not add</param>
        /// <param name="bankingDetails">True=add, False=do not add</param>
        /// <param name="affordability">True=add, False=do not add</param>
        /// <param name="assetsLiabilities">True=add, False=do not add</param>
        /// <param name="memo">True=add, False=do not add</param>
        /// <param name="leRelationships">True=add, False=do not add</param>
        public static void CompleteLegalEntityNode(TestBrowser browser, int offerKey, bool declarations, bool employment, bool address, bool bankingDetails,
            bool affordability, bool assetsLiabilities, bool memo, bool leRelationships)
        {
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.View);
            // Get LegalEntityLegalNames of all Main Applicant and Suretor LegalEntities on Offer
            _2AMDataHelper db = new _2AMDataHelper();
            var legalEntities = db.GetLegalEntityLegalNamesForOffer(offerKey);

            // Iterate through Main Applicant and Suretor LegalEntities on Offer and update LegalEntity Information
            for (int row = 0; row < legalEntities.RowList.Count; row++)
            {
                int legalEntityKey = legalEntities.Rows(row).Column("LegalEntityKey").GetValueAs<int>();
                int legalEntityTypeKey = legalEntities.Rows(row).Column("LegalEntityTypeKey").GetValueAs<int>();

                //  If Natural Person
                if (legalEntityTypeKey == (int)LegalEntityTypeEnum.NaturalPerson)
                {
                    browser.Navigate<Navigation.LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
                    if (declarations)
                    {
                        // Update Declarations
                        browser.Navigate<Navigation.LegalEntityNode>().ApplicationDeclarations(NodeTypeEnum.Update);
                        browser.Page<ApplicationDeclarations>().ApplicationDeclarationsUpdate();
                    }
                    if (address)
                    {
                        //add an address
                        browser.Navigate<Navigation.LegalEntityNode>().LegalEntityAddress(NodeTypeEnum.Add);
                        var r = new Random();
                        Automation.DataModels.Address residentialAddress = new Automation.DataModels.Address
                        {
                            StreetNumber = r.Next(0, 50000).ToString(),
                            StreetName = "Test Street",
                            RRR_ProvinceDescription = Province.Gauteng,
                            RRR_SuburbDescription = "Sandton",
                            UnitNumber = "0",
                            BuildingNumber = "0",
                            BuildingName = ""
                        };
                        browser.Page<LegalEntityAddressDetails>().AddStreetAddress(residentialAddress, AddressType.Residential);
                    }
                    if (employment)
                    {
                        //we need to save each of the employments
                        browser.Navigate<Navigation.LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Update);
                        browser.Page<LegalEntityEmploymentDetails>().SaveEmployment();
                    }
                }
            }
            legalEntities.Dispose();
        }

    }
}
