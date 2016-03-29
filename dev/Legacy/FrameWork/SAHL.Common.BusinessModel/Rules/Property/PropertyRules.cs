using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Property
{
    [RuleDBTag("PropertySectionalSchemeNameMandatory",
    "If the dbo.property is Sectional Title these are mandatory: SectionalTitleName,SectionalTitleUnit,SectionalTitleSchemeNumber ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertySectionalSchemeNameMandatory")]
    [RuleInfo]
    public class PropertySectionalSchemeNameMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertySectionalSchemeNameMandatory  rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertySectionalSchemeNameMandatory  rule expects the following objects to be passed: IProperty.");

            //if (RuleItem.RuleParameters.Count < 1)
            //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            # region Rule Check

            IProperty property = Parameters[0] as IProperty;
            if (property.TitleType.Key == 7 | property.TitleType.Key == 3)
            {
                if (!String.IsNullOrEmpty(property.SectionalSchemeName))
                    return 0;
                else
                    AddMessage(String.Format("SectionalSchemeName is a mandatory field."), "", Messages);
            }

            #endregion

            return 0;
        }
    }

    [RuleDBTag("PropertySectionalUnitNumberMandatory",
    "If the dbo.property is Sectional Title these are mandatory: SectionalTitleName,SectionalTitleUnit,SectionalTitleSchemeNumber ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertySectionalUnitNumberMandatory")]
    [RuleInfo]
    public class PropertySectionalUnitNumberMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertySectionalUnitNumberMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertySectionalUnitNumberMandatory rule expects the following objects to be passed: IProperty.");

            //if (RuleItem.RuleParameters.Count < 1)
            //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            # region Rule Check

            IProperty property = Parameters[0] as IProperty;
            if (property.TitleType.Key == 7 | property.TitleType.Key == 3)
            {
                if (!String.IsNullOrEmpty(property.SectionalUnitNumber))
                    return 0;
                else
                    AddMessage(String.Format("SectionalUnitNumber is a mandatory field."), "", Messages);
            }

            #endregion

            return 0;
        }
    }

    [RuleDBTag("PropertyAdCheckDataProvider",
    "Data Provider must not be set to AdCheck.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyAdCheckDataProvider")]
    [RuleInfo]
    public class PropertyAdCheckDataProvider : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IProperty property = (IProperty)Parameters[0];
            if (property.DataProvider != null && property.DataProvider.Key == (int)DataProviders.AdCheck)
            {
                AddMessage("Data Provider must not be set to AdCheck", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("PropertyTypeMandatory",
    "Requires that a PropertyType is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyTypeMandatory")]
    [RuleInfo]
    public class PropertyTypeMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyTypeMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyTypeMandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (property.PropertyType == null)
            {
                string errorMessage = "Property Type is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyTitleTypeMandatory",
    "Requires that a TitleType is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyTitleTypeMandatory")]
    [RuleInfo]
    public class PropertyTitleTypeMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyTitleTypeMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyTitleTypeMandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (property.TitleType == null)
            {
                string errorMessage = "Title Type is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyOccupancyTypeMandatory",
    "Requires that a OccupancyType is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyOccupancyTypeMandatory")]
    [RuleInfo]
    public class PropertyOccupancyTypeMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyOccupancyTypeMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyOccupancyTypeMandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (property.OccupancyType == null)
            {
                string errorMessage = "Occupancy Type is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyAreaClassificationMandatory",
    "Requires that a AreaClassification is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyAreaClassificationMandatory")]
    [RuleInfo]
    public class PropertyAreaClassificationMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyAreaClassificationMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyAreaClassificationMandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (property.AreaClassification == null)
            {
                string errorMessage = "Area Classification is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyDeedsPropertyTypeMandatory",
    "Requires that a DeedsPropertyType is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyDeedsPropertyTypeMandatory")]
    [RuleInfo]
    public class PropertyDeedsPropertyTypeMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyDeedsPropertyTypeMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyDeedsPropertyTypeMandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (property.DeedsPropertyType == null)
            {
                string errorMessage = "Deeds Property Type is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyDescription1Mandatory",
    "Requires that PropertyDescription1 is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyDescription1Mandatory")]
    [RuleInfo]
    public class PropertyDescription1Mandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyDescription1Mandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyDescription1Mandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (Utils.StringUtils.IsNullOrEmptyTrimmed(property.PropertyDescription1))
            {
                string errorMessage = "Property Description line 1 is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            if (property.PropertyDescription1.Length > 100)
            {
                string errorMessage = "Property Description line 1 length exceeds the limit of 100 characters";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyDescription2Mandatory",
    "Requires that PropertyDescription2 is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyDescription2Mandatory")]
    [RuleInfo]
    public class PropertyDescription2Mandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyDescription2Mandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyDescription2Mandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (Utils.StringUtils.IsNullOrEmptyTrimmed(property.PropertyDescription2))
            {
                string errorMessage = "Property Description line 2 is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            if (property.PropertyDescription2.Length > 100)
            {
                string errorMessage = "Property Description line 2 length exceeds the limit of 100 characters";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyDescription3Mandatory",
    "Requires that PropertyDescription3 is entered when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyDescription3Mandatory")]
    [RuleInfo]
    public class PropertyDescription3Mandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyDescription3Mandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyDescription3Mandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            if (Utils.StringUtils.IsNullOrEmptyTrimmed(property.PropertyDescription3))
            {
                string errorMessage = "Property Description line 3 is Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            if (property.PropertyDescription3.Length > 100)
            {
                string errorMessage = "Property Description line 3 length exceeds the limit of 100 characters";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("PropertyNoUpdateOnOpenLoan",
    "Only allows a property to be updated if it is not attached to an Open Loan.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyNoUpdateOnOpenLoan")]
    [RuleInfo]
    public class PropertyNoUpdateOnOpenLoan : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region parameters

            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyNoUpdateOnOpenLoan rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyNoUpdateOnOpenLoan rule expects the following objects to be passed: IProperty, IApplication.");

            if (!(Parameters[1] is IApplication))
                throw new ArgumentException("The PropertyNoUpdateOnOpenLoan rule expects the following objects to be passed: IProperty, IApplication.");

            #endregion

            IProperty property = Parameters[0] as IProperty;
            IApplication app = Parameters[1] as IApplication;

            if (app.ApplicationType.Key == (int)OfferTypes.SwitchLoan
                || app.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan
                || app.ApplicationType.Key == (int)OfferTypes.RefinanceLoan)
            {
                // check if this property is attached to an open loan
                bool propertyOnOpenLoan = false;

                foreach (IFinancialService financialservice in property.MortgageLoanProperties)
                {
                    if (financialservice.AccountStatus.Key == (int)AccountStatuses.Open || financialservice.AccountStatus.Key == (int)AccountStatuses.Dormant)
                    {
                        propertyOnOpenLoan = true;
                        break;
                    }
                }

                if (propertyOnOpenLoan)
                {
                    string errorMessage = "This property is connected to an open loan and cannot be updated, please use the capture property functionality or alternately contact client services to update.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("PropertyTitleDeedNumberMandatory",
    "Requires that at least one PropertyTitleDeed record exists when saving a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.PropertyTitleDeedNumberMandatory")]
    [RuleInfo]
    public class PropertyTitleDeedNumberMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyTitleDeedNumberMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The PropertyTitleDeedNumberMandatory rule expects the following objects to be passed: IProperty.");

            IProperty property = Parameters[0] as IProperty;

            bool valid = true;
            if (Parameters.Length > 1) // if we have passsed a string of titledeed numbers then use that in the validation
            {
                string titleDeedNumbers = Parameters[1] as string;
                if (titleDeedNumbers == null || titleDeedNumbers.Trim().Length <= 0)
                    valid = false;
            }
            else // otherwise use the PropertyTitleDeeds object
            {
                if (property.PropertyTitleDeeds == null || property.PropertyTitleDeeds.Count <= 0)
                    valid = false;
            }

            if (valid == false)
            {
                string errorMessage = "At least one Title Deed Number is required.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DetermineDuplicateApplication",
    "Checks whether the captured property is a duplicate application",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.DetermineDuplicateApplication")]
    [RuleInfo]
    public class DetermineDuplicateApplication : BusinessRuleBase
    {
        public DetermineDuplicateApplication(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int iOfferKey = (int)Parameters[0];
            int iPropertyKey = (int)Parameters[1];

            string sqlQuery = UIStatementRepository.GetStatement("Rules.Property", "DetermineDuplicateApplication");
            ParameterCollection prms = new ParameterCollection();

            prms.Add(new SqlParameter("@addtoOfferKey", iOfferKey));
            prms.Add(new SqlParameter("@propertyKey", iPropertyKey));
            prms.Add(new SqlParameter("@StageDefintionGroupNTUOffer", (int)StageDefinitionStageDefinitionGroups.NTUOffer));
            prms.Add(new SqlParameter("@StageDefintionGroupDeclineOffer", (int)StageDefinitionStageDefinitionGroups.DeclineOffer));

            DataSet dsProperty = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (dsProperty != null)
            {
                if (dsProperty.Tables.Count > 0)
                {
                    if (dsProperty.Tables[0].Rows.Count > 0)
                    {
                        int iExistingApplication = Convert.ToInt32(dsProperty.Tables[0].Rows[0]["OfferKey"]);
                        string errMsg = "Application " + iExistingApplication.ToString() + ", containing the clients ID number and the respective property, " +
                                        "already exists in the origination process";
                        AddMessage(errMsg, errMsg, Messages);
                        return 0;
                    }
                }
            }

            return 1;
        }
    }

    #region Prevent blank Properties Rules

    [RuleDBTag("ApplicationCaptureMinimumPropertyData",
    "Prevents a case from progressing any further if it has blank properties (blank properties could come from Migration).",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Property.ApplicationCaptureMinimumPropertyData", true)]
    [RuleInfo]
    public class ApplicationCaptureMinimumPropertyData : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCaptureMinimumPropertyData rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationCaptureMinimumPropertyData rule expects the following objects to be passed: IApplication.");

            #endregion

            IApplicationMortgageLoan app = Parameters[0] as IApplicationMortgageLoan;

            if (app != null && app.Property != null)
            {
                string msg = "";
                if (app.Property.Address == null)
                    msg += "property address, ";

                if (app.Property.PropertyDescription1 == null || app.Property.PropertyDescription1.Length < 1 ||
                    app.Property.PropertyDescription2 == null || app.Property.PropertyDescription2.Length < 1 ||
                    app.Property.PropertyDescription3 == null || app.Property.PropertyDescription3.Length < 1)
                    msg += "property description, ";

                if (app.Property.PropertyType == null)
                    msg += "property type, ";

                if (app.Property.TitleType == null)
                    msg += "property title type, ";

                if (app.Property.OccupancyType == null)
                    msg += "property occupancy type, ";

                if (app.Property.PropertyAccessDetails == null)
                    msg += "property inspection contact, ";

                if (app.Property.PropertyAccessDetails != null && (app.Property.PropertyAccessDetails.Contact1 == null || app.Property.PropertyAccessDetails.Contact1.Length < 1))
                    msg += "property inspection contact, ";

                if (app.Property.PropertyAccessDetails != null && (app.Property.PropertyAccessDetails.Contact1Phone == null || app.Property.PropertyAccessDetails.Contact1Phone.Length < 1))
                    msg += "property inspection contact number";

                if (!string.IsNullOrEmpty(msg))
                {
                    string error = "A valid " + msg + " are required";
                    AddMessage(error, error, Messages);
                    return 0;
                }
            }
            else
            {
                AddMessage("Application does not have a valid property.", "Application does not have a valid property.", Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("ManagementApplicationMinimumPropertyData",
        "Prevents a case from progressing any further if it has blank properties (blank properties could come from Migration).",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Property.ManagementApplicationMinimumPropertyData", true)]
    [RuleInfo]
    public class ManagementApplicationMinimumPropertyData : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ManagementApplicationMinimumPropertyData rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ManagementApplicationMinimumPropertyData rule expects the following objects to be passed: IApplication.");

            #endregion

            IApplicationMortgageLoan app = Parameters[0] as IApplicationMortgageLoan;

            if (app != null && app.Property != null)
            {
                string msg = "";
                if (app.Property.Address == null)
                    msg += "property address, ";

                if (app.Property.PropertyDescription1 == null || app.Property.PropertyDescription1.Length < 1 ||
                    app.Property.PropertyDescription2 == null || app.Property.PropertyDescription2.Length < 1 ||
                    app.Property.PropertyDescription3 == null || app.Property.PropertyDescription3.Length < 1)
                    msg += "property description, ";

                if (app.Property.PropertyType == null)
                    msg += "property type, ";

                if (app.Property.TitleType == null)
                    msg += "property title type, ";

                if (app.Property.OccupancyType == null)
                    msg += "property occupancy type, ";

                if (app.Property.PropertyAccessDetails == null)
                    msg += "property inspection contact, ";

                if (app.Property.PropertyAccessDetails != null && (app.Property.PropertyAccessDetails.Contact1 == null || app.Property.PropertyAccessDetails.Contact1.Length < 1))
                    msg += "property inspection contact, ";

                if (app.Property.PropertyAccessDetails != null && (app.Property.PropertyAccessDetails.Contact1Phone == null || app.Property.PropertyAccessDetails.Contact1Phone.Length < 1))
                    msg += "property inspection contact number";

                if (app.Property.DeedsPropertyType == null)
                    msg += "property deeds property type, ";

                if (app.Property.ErfSuburbDescription == null || app.Property.ErfSuburbDescription.Length < 1)
                    msg += "property erf suburb description, ";

                if (app.Property.ErfMetroDescription == null || app.Property.ErfMetroDescription.Length < 1)
                    msg += "property erf metro description, ";

                if (app.Property.DeedsPropertyType != null && app.Property.DeedsPropertyType.Key == (int)DeedsPropertyTypes.Unit)
                {
                    if (app.Property.SectionalSchemeName == null || app.Property.SectionalSchemeName.Length < 1)
                        msg += "property sectional scheme name, ";

                    if (app.Property.SectionalUnitNumber == null || app.Property.SectionalUnitNumber.Length < 1)
                        msg += "property sectional unit number, ";
                }
                else
                {
                    if (app.Property.ErfNumber == null || app.Property.ErfNumber.Length < 1)
                        msg += "property erf number, ";
                }

                // will need to loop through any properties that are attached to find titledeednumbers
                foreach (IPropertyTitleDeed propTitleDeed in app.Property.PropertyTitleDeeds)
                {
                    if (propTitleDeed != null)
                    {
                        if ((propTitleDeed.TitleDeedNumber == null || propTitleDeed.TitleDeedNumber.Length < 1))
                        {
                            msg += "property title deed number, ";
                        }
                    }
                    else
                    {
                        msg += "property title deed, ";
                    }
                }

                // if returnflag is false then we need to throw an error
                if (!string.IsNullOrEmpty(msg))
                {
                    string error = "A valid " + msg + " are required";
                    AddMessage(error, error, Messages);
                    return 0;
                }
            }
            else
            {
                AddMessage("Application does not have a valid property.", "Application does not have a valid property.", Messages);
                return 0;
            }
            return 1;
        }
    }

    #endregion

    [RuleDBTag("LightStonePropertyIDExists",
        "Checks to confirm a LightStone PropertyID exists before a LightStone valuation request is made.",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Property.LightStonePropertyIDExists", true)]
    [RuleInfo]
    public class LightStonePropertyIDExists : BusinessRuleBase
    {
		IPropertyRepository propertyRepository;
		public LightStonePropertyIDExists(IPropertyRepository propertyRepository)
		{
			this.propertyRepository = propertyRepository;
		}
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LightStonePropertyIDExists rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The LightStonePropertyIDExists rule expects the following objects to be passed: IProperty.");

            #endregion

            IProperty prop = Parameters[0] as IProperty;

			string id = propertyRepository.GetLightStonePropertyID(prop);

            if (string.IsNullOrEmpty(id))
            {
                AddMessage("There is no LightStone property ID to do a valuation.", "There is no LightStone property ID to do a valuation.", Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LightStoneValuationRecent",
        "Checks to confirm a recent LightStone valuation does not exist before a new LightStone valuation request is made.",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Property.LightStoneValuationRecent", true)]
    [RuleParameterTag(new string[] { "@Months,2,9" })]
    [RuleInfo]
    public class LightStoneValuationRecent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LightStoneValuationRecent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IProperty))
                throw new ArgumentException("The LightStoneValuationRecent rule expects the following objects to be passed: IProperty.");

            #endregion

            IProperty prop = Parameters[0] as IProperty;
            bool valRecentExists = false;
            int months = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            foreach (IValuation v in prop.Valuations)
            {
                IValuationDiscriminatedLightstoneAVM lsVal = v as IValuationDiscriminatedLightstoneAVM;

                if (lsVal != null && DateTime.Now < lsVal.ValuationDate.AddMonths(months))
                {
                    valRecentExists = true;
                    break;
                }
            }

            if (valRecentExists)
            {
                string msg = String.Format(@"A LightStone valuation for this property exists less than {0} months old.", months);
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    /*
    PropertyDomiciliumAddressChange? - this rule is preventing the update of a property address if it is being used as a domicilium address. 
    This is cumbersome as it is forcing the user to go and update the domicilium to something else so that they can change the property address 
    and then go back and reset it to the property address. I would prefer this to be refactored so that we can provide the legal entities who are 
    using the address as a domicilium (active or pending), and warn the user that updating the property address will change their domicilium
    */
    [RuleDBTag("WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium",
        "Warns users when changing an address (from property screens) that the property is used as the LEDomicilium for the following people",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Property.WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium", true)]
    [RuleParameterTag(new string[] { "@Months,2,9" })]
    [RuleInfo]
    public class WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium : BusinessRuleBase
    {
        ILegalEntityRepository legalEntityRepository = null;
        public WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium(ILegalEntityRepository legalEntityRepository)
        {
            this.legalEntityRepository = legalEntityRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int result = 0;
            IAddress address = Parameters[0] as IAddress;
            if (null == address)
                throw new ArgumentException("Incorrect type passed into WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium");

            
            IEventList<ILegalEntityDomicilium> legalentityDomiciliums = legalEntityRepository.GetLegalEntityDomiciliumsForAddressKey(address.Key);
            if (legalentityDomiciliums.Count > 0)
            {
                foreach (ILegalEntityDomicilium domicilium in legalentityDomiciliums)
                {
                    if (domicilium.GeneralStatus.Key == (int)GeneralStatuses.Active || domicilium.GeneralStatus.Key == (int)GeneralStatuses.Pending)
                    {
                        string message = string.Format(@"The same address is being used as the {0} domicilium address for {1} and may also need to be updated.",
                            domicilium.GeneralStatus.Description, domicilium.LegalEntityAddress.LegalEntity.DisplayName);
                        AddMessage(message, message, Messages);
                        result = 1;
                    }
                }
            }
            return result;
        }
    }
}