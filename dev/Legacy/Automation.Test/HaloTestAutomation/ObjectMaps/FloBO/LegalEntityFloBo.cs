using System;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps
{
    public abstract class LegalEntityNodeControls : NavigationControls.BaseNavigation
    {
        protected Link LegalEntityMain(string LegalEntityName)
        {
            return base.Document.Link(Find.ByText(new Regex(@"^[\x20-\x7E]*" + LegalEntityName + "$")));
        }

        protected Link LegalEntityMain(int NodeIndex)
        {
            DivCollection divCol = base.Document.Divs.Filter(Find.ByClass("TreeNode"));
            if (divCol.Count > 0)
            {
                foreach (Div d in divCol)
                {
                    if (d.Id.Contains("Applicants"))
                    {
                        foreach (Div d1 in d.Divs.Filter(Find.ByClass("TreeNode")))
                        {
                            return d1.Links[0];
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Could not find any legal entities in the webbrowser");
            }
            return null;
        }

        #region LegalEntityDetails

        [FindBy(Title = "Legal Entity Details")]
        public Link LegalEntityDetails { get; set; }

        [FindBy(Title = "Update Legal Entity Details")]
        public Link UpdateLegalEntityDetails { get; set; }

        [FindBy(Title = "Legal Entities")]
        public Link DCLegalEntityMain { get; set; }

        #endregion LegalEntityDetails

        #region ApplicationDeclarations

        [FindBy(Title = "Application Declarations")]
        public Link ApplicationDeclarations { get; set; }

        [FindBy(Title = "Update Declarations")]
        public Link UpdateDeclarations { get; set; }

        #endregion ApplicationDeclarations

        #region LegalEntityAddress

        [FindBy(Title = "Legal Entity Address Details")]
        public Link LegalEntityAddressDetails { get; set; }

        [FindBy(Title = "Add Legal Entity Address")]
        public Link AddLegalEntityAddress { get; set; }

        [FindBy(Title = "Update Legal Entity Address")]
        public Link UpdateLegalEntityAddress { get; set; }

        [FindBy(Title = "Delete Legal Entity Address")]
        public Link DeleteLegalEntityAddress { get; set; }

        #endregion LegalEntityAddress

        #region EmploymentDetails

        [FindBy(Title = "Employment Details")]
        public Link EmploymentDetails { get; set; }

        [FindBy(Title = "Add Employment Details")]
        public Link AddEmploymentDetails { get; set; }

        [FindBy(Title = "Update Employment Details")]
        public Link UpdateEmploymentDetails { get; set; }

        #endregion EmploymentDetails

        #region BankingDetails

        [FindBy(Title = "Banking Details")]
        public Link BankingDetails { get; set; }

        [FindBy(Title = "Add Banking Details")]
        public Link AddBankingDetails { get; set; }

        [FindBy(Title = "Update Banking Details")]
        public Link UpdateBankingDetails { get; set; }

        [FindBy(Title = "Delete Banking Details")]
        public Link DeleteBankingDetails { get; set; }

        #endregion BankingDetails

        #region AffordabilityAndExpenses

        [FindBy(Title = "Affordability and Expenses")]
        public Link AffordabilityandExpenses { get; set; }

        [FindBy(Title = "Update Affordability Details")]
        public Link UpdateAffordabilityDetails { get; set; }

        #endregion AffordabilityAndExpenses

        #region AssetsAndLiabilities

        [FindBy(Title = "Assets And Liabilities")]
        public Link AssetsAndLiabilities { get; set; }

        [FindBy(Title = "Add Asset/Liability")]
        public Link AddAssetLiability { get; set; }

        [FindBy(Title = "Update Asset/Liability")]
        public Link UpdateAssetLiability { get; set; }

        [FindBy(Title = "Delete Asset/Liability")]
        public Link DeleteAssetLiability { get; set; }

        [FindBy(Title = "Associate Asset/Liability")]
        public Link AssociateAssetLiability { get; set; }

        #endregion AssetsAndLiabilities

        #region LegalEntityMemo

        [FindBy(Title = "Legal Entity Memo")]
        public Link LegalEntityMemo { get; set; }

        [FindBy(Title = "Add Legal Entity Memo")]
        public Link AddLegalEntityMemo { get; set; }

        [FindBy(Title = "Update Legal Entity Memo")]
        public Link UpdateLegalEntityMemo { get; set; }

        #endregion LegalEntityMemo

        #region LegalEntityRelationships

        [FindBy(Title = "Legal Entity Relationships")]
        public Link LegalEntityRelationships { get; set; }

        [FindBy(Title = "Add Relationship")]
        public Link AddLegalEntityRelationship { get; set; }

        [FindBy(Title = "Update Relationship")]
        public Link UpdateLegalEntityRelationship { get; set; }

        [FindBy(Title = "Delete Relationship")]
        public Link DeleteLegalEntityRelationship { get; set; }

        #endregion LegalEntityRelationships

        [FindBy(Text = "Maintain Legal Entities")]
        protected Link MaintainLegalEntities { get; set; }

        [FindBy(Text = "Domicilium Address Details")]
        protected Link DomiciliumAddressDetails { get; set; }

        [FindBy(Text = "Update Domicilium Address")]
        protected Link UpdateDomiciliumAddress { get; set; }
    }
}