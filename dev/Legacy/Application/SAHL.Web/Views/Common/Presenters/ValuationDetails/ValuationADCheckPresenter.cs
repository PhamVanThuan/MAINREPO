using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using IValuation = SAHL.Common.BusinessModel.Interfaces.IValuation;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationADCheckPresenter : SAHLCommonBasePresenter<IValuationAdCheckView>
    {

        public ILookupRepository _lookupRepository;
        public IAddressRepository _addressRepository;
        public IPropertyRepository _propertyRepository;
        public IApplicationRepository _applicationRepository;
        public IAccountRepository _accountRepository;
        public IBondRepository _bondRepository;
        public DataTable dsValuations = new DataTable();
        public DataSet dsAdCheck = new DataSet();
        public string OfferApplicationTypeDescription;
        //private InstanceNode instanceNode;

        public IValuation valuations;
        public int valuationkey;


        //STRING ARRAYS FOR ADCHECK'S RETURNED DATA **********************************

        private readonly string[] arrayIdentifiedType =
            { "No", "Yes" };

        //private readonly string[] arrayErfType =
        //    { "", "Ordinary", "Vacant Land", "Sectional title unit" };

        //private readonly string[] arrayReasonType =
        //    { "", "Arrears", "Audit Valuation", "Further Loan", "New Purchase", "Switch Loan" };

        //private readonly string[] arrayPropertyUseType =
        //    { "", "Residential", "Commercial - Partial", "Commercial - Full", "Other" };

        //private readonly string[] arrayPropertyType =
        //    { "", "Vacant Land", "Residential", "Business" };

        //private readonly string[] arrayAreaType =
        //    { "", "Urban", "Business", "Other" };

        //private readonly string[] arrayConditionType =
        //    { "", 
        //        "Approved Plans", 
        //        "Engineers Clearance certificate", 
        //        "NHBRC Registration certificate",
        //        "NHBRC Enrolment certificate",
        //        "Site identification required",
        //        "Waterproof certificate",
        //        "Appointment of engineer",
        //        "Pest control certificate",
        //        "Surveyer General diagram",
        //        "Borehole certificate",
        //        "Occupational certificate",
        //        "Replacement value" };

        //private readonly string[] arraySecurityMortgagePurposeType =
        //    { "", "Good", "Average", "Poor", "Unacceptable" };

        //private readonly string[] arraySectorType =
        //    { "", "Urban", "Rural" };

        //private readonly string[] arrayLocationCommentType =
        //    { "", 
        //        "Good", 
        //        "Informal and unrecognised settlement", 
        //        "Within 50 year flood line",
        //        "Adjoining industrial township",
        //        "Remote with difficult access",
        //        "High noise factor",
        //        "Economically depressed area",
        //        "High risk area",
        //        "Inaccessible",
        //        "Other" };

        //private readonly string[] arrayImprovementCommentType =
        //    { "", 
        //       "Good", 
        //        "Structurally unsound", 
        //        "Poor design", 
        //        "Physically obselete",
        //        "Poor positioning on site",
        //        "Vandalized",
        //        "Contravenes town planning",
        //        "Over capitalized",
        //        "Does not comply with NBR",
        //        "Not SAHL approved building system",
        //        "Incompatible with improvements",
        //        "In need of maintenance",
        //        "Functionally obselete",
        //        "Other",
        //        "Adequate" };

        //private readonly string[] arrayMarketCommentType =
        //    { "", 
        //        "Good",
        //        "Stable",
        //        "Unstable",
        //        "Depressed",
        //        "Slow moving",
        //        "Over supply",
        //        "Rising",
        //        "Declining",
        //        "Other" };

        //private readonly string[] arrayPropertyCommentType =
        //    { "", 
        //        "Good", 
        //        "Derelict",
        //        "Below Par",
        //        "Neglected",
        //        "Adequate",
        //        "Well Maintained",
        //        "Superior",
        //        "Excellent",
        //        "To be completed" };

        //private readonly string[] arrayRetentionReasonType =
        //    { "", "Building Loan", "Small Works" };


        //private readonly string[] arraySwimmingPoolType =
        //    { "", "Concrete", "Vinyl", "Fibre Glass" };

        //private readonly string[] arrayTennisCourtType =
        //    { "", "Concrete", "All Weather", "Clay" };
        //private readonly string[] arrayOtherImprovementRoofType =
        //    { "No Selection", "Conventional", "Thatch", "No Roof", "Other" };

        //private readonly string[] arrayRoofType =
        //    { "No Selection", "Conventional", "Thatch", "No Roof", "Other" };


        //private readonly string[] arrayInsuranceRoofType =
        //    { "No Selection", "Conventional", "Thatch", "Other", "Wooden" };


        //private readonly string[] arrayComparableSales =
        //    {
        //        "",
        //        "Better condition than subject property",
        //        "Better than subject property",
        //        "In better condition than subject property",
        //        "Inferior to subject property",
        //        "Larger than subject property",
        //        "Similar to subject property, but offers better accomodation",
        //        "Smaller than subject property",
        //        "Superior to subject property"
        //    };

        //private readonly string[] arrayReturnReason =
        //    {"",
        //        "Invalid allocation",
        //        "Cannot find property",
        //        "Pending research",
        //        "Wrong property",
        //        "Other",
        //        "Mutual assessment"
        //    };


        //**************************************************************************


        public IBondRepository BondRepository
        {

            get
            {
                if (_bondRepository == null)
                    _bondRepository = RepositoryFactory.GetRepository<IBondRepository>();
                return _bondRepository;
            }

        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                    _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                return _accountRepository;
            }
        }

        public ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepository;
            }
        }

        public IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return _addressRepository;
            }
        }

        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                    _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return _applicationRepository;
            }
        }

        public IPropertyRepository PropertyRepository
        {
            get
            {
                if (_propertyRepository == null)
                    _propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
                return _propertyRepository;
            }
        }

        private IX2Repository x2repository;
        public IX2Repository X2Repository
        {
            get
            {
                if (x2repository == null)
                    x2repository = RepositoryFactory.GetRepository<IX2Repository>();
                return x2repository;
            }
        }

        public ValuationADCheckPresenter(IValuationAdCheckView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            // Set up EventHandlers for the interface
            View.btnBackClicked += btnBack_Clicked;
            //node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            valuationkey = Convert.ToInt32(GlobalCacheData["ValuationKey"]);
            valuations = PropertyRepository.GetValuationByKey(valuationkey);

            dsValuations = PopulateValuationsDS(valuations);
            InitialiseXMLDataset(dsValuations);
            SetupInterface();

        }

        void btnBack_Clicked(object sender, EventArgs e)
        {
            View.Navigator.Navigate("Back");
        }


        void SetupInterface()
        {
            try
            {

                if (dsAdCheck != null)
                {

                    if (dsAdCheck.Tables["Response"] != null)
                        SetupResponse(dsAdCheck.Tables["Response"].Rows[0]);

                    if (dsAdCheck.Tables["Val_CottageBuilding"] != null)
                        SetupCottage(dsAdCheck.Tables["Val_CottageBuilding"].Rows[0]);

                    if (dsAdCheck.Tables["Val_Insurance"] != null)
                    {
                        SetupCalculatedFields(dsAdCheck.Tables["Val_Insurance"].Rows[0]);
                        SetupInsuranceRow(dsAdCheck.Tables["Val_Insurance"].Rows[0]);
                    }

                    if (dsAdCheck.Tables["Val_MainBuilding"] != null)
                        SetupMainbuilding(dsAdCheck.Tables["Val_MainBuilding"].Rows[0]);

                    if (dsAdCheck.Tables["Val_OutBuilding"] != null)
                        SetupOutbuilding(dsAdCheck.Tables["Val_OutBuilding"].Rows[0]);

                    if (dsAdCheck.Tables["Val_Improvements"] != null)
                        SetupImprovements(dsAdCheck.Tables["Val_Improvements"].Rows[0]);

                    //POPULATE DATATABLES FOR THE GRIDS 
                    BindGridConditions();
                    BindGridConditionComments();
                    BindGridComparativeProperties();
                    BindGridImprovementResults();

                    GlobalCacheData.Remove("AdCheckLoaded");
                    GlobalCacheData.Add("AdCheckLoaded", true, new List<ICacheObjectLifeTime>());
                }

            }

            catch (Exception)
            {
                View.Messages.Add(new Warning("Unable to display AdCheck Valuation details. Please contact system administrator.", "Unable to display AdCheck Valuation details. Please contact system administrator."));
            }

        }

        private void BindGridImprovementResults()
        {
            DataTable OtherImprovementsDT = dsAdCheck.Tables["val_other_improvements_collection"];
            DataTable DSOtherImprovementsGridData = new DataTable();
            DSOtherImprovementsGridData.Columns.Add("number");
            DSOtherImprovementsGridData.Columns.Add("description");
            DSOtherImprovementsGridData.Columns.Add("rooftype");
            DSOtherImprovementsGridData.Columns.Add("value");

            if (OtherImprovementsDT != null)
            {

                for (int i = 0; i < OtherImprovementsDT.Rows.Count; i++)
                {
                    DataRow drow = OtherImprovementsDT.Rows[i];
                    DataRow valRow = DSOtherImprovementsGridData.NewRow();
                    valRow["number"] = "Improvement " + Convert.ToString(drow["val_capture_improvement_item_id"]);
                    valRow["description"] = Convert.ToString(drow["description"]);
                    valRow["rooftype"] = Enum.GetName(typeof(AdCheckInterfaces.val_other_improvement_roof_type), Convert.ToInt32(drow["val_other_improvement_roof_type_id"]));
                    double rate = Convert.ToDouble(drow["rate"]) * Convert.ToDouble(drow["square_meterage"]);
                    valRow["value"] = Convert.ToDouble(rate).ToString(SAHL.Common.Constants.CurrencyFormat);
                    DSOtherImprovementsGridData.Rows.Add(valRow);
                }
            }
            View.BindgrdImprovementResults(DSOtherImprovementsGridData);
        }

        private void BindGridConditions()
        {
            DataTable ConditionsCollectionDT = new DataTable();
            ConditionsCollectionDT.Columns.Add("Description");
            if (dsAdCheck.Tables["val_conditions_collection"] != null)
            {
                // edit for the enumeration
                for (int i = 0; i < dsAdCheck.Tables["val_conditions_collection"].Rows.Count; i++)
                {
                    DataRow valRow = ConditionsCollectionDT.NewRow();
                    int key = Convert.ToInt32(dsAdCheck.Tables["val_conditions_collection"].Rows[i]["val_conditions_type_id"]);
                    valRow["Description"] = Enum.GetName(typeof(AdCheckInterfaces.val_conditions_type), key);
                    ConditionsCollectionDT.Rows.Add(valRow);

                }
            }
            View.BindgrdConditions(ConditionsCollectionDT);
        }

        private void BindGridComparativeProperties()
        {
            DataTable ComparablSalesDT = new DataTable();
            ComparablSalesDT.Reset();
            ComparablSalesDT.Columns.Add("AssesmentDate");
            ComparablSalesDT.Columns.Add("PurchaseValue");
            ComparablSalesDT.Columns.Add("StandNo");
            ComparablSalesDT.Columns.Add("Suburb");
            if (dsAdCheck.Tables["val_comparable_sales_collection"] != null)
            {
                for (int i = 0; i < dsAdCheck.Tables["val_comparable_sales_collection"].Rows.Count; i++)
                {
                    DataRow valRow = ComparablSalesDT.NewRow();

                    valRow["AssesmentDate"] = Convert.ToString(dsAdCheck.Tables["val_comparable_sales_collection"].Rows[i]["assessment_date"]);
                    valRow["PurchaseValue"] = Convert.ToDouble(dsAdCheck.Tables["val_comparable_sales_collection"].Rows[i]["purchase_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                    valRow["StandNo"] = Convert.ToString(dsAdCheck.Tables["val_comparable_sales_collection"].Rows[i]["stand_no"]);
                    int key = Convert.ToInt32(dsAdCheck.Tables["val_comparable_sales_collection"].Rows[i]["sub_suburb_id"]);
                    ISuburb suburb = PropertyRepository.GetSuburbByKey(key);
                    valRow["Suburb"] = suburb.Description;
                    ComparablSalesDT.Rows.Add(valRow);

                }

            }

            View.BindgrdComparativeProperties(ComparablSalesDT);
        }

        private void BindGridConditionComments()
        {
            // Possible Multiple Data Rows in the following tables:
            DataTable DSGridData = new DataTable();
            DSGridData.Columns.Add("Description");
            if (dsAdCheck.Tables["val_condition_comments"] != null)
            {
                // Setup and populate the Conditions Grid
                DataRow drow = dsAdCheck.Tables["val_condition_comments"].Rows[0];
                DataRow valRow = DSGridData.NewRow();
                valRow["description"] = drow["comment1"].ToString();
                DSGridData.Rows.Add(valRow);
                valRow = DSGridData.NewRow();
                valRow["description"] = drow["comment2"].ToString();
                DSGridData.Rows.Add(valRow);
                valRow = DSGridData.NewRow();
                valRow["description"] = drow["comment3"].ToString();
                DSGridData.Rows.Add(valRow);
                valRow = DSGridData.NewRow();
                valRow["description"] = drow["comment4"].ToString();
                DSGridData.Rows.Add(valRow);
            }
            View.BindgrdConditionComments(DSGridData);
        }

        private void SetupCalculatedFields(DataRow insurancerow)
        {
            //*************************************************************************************************************
            //DO THE RELEVANT CALCULATIONS
            if (insurancerow != null)
            {
                double mainrate = 0;
                double mainmeterage = 0;

                if (FieldDataExists(insurancerow, "main_rate")) mainrate = Convert.ToDouble(insurancerow["main_rate"]);
                if (FieldDataExists(insurancerow, "main_square_meterage")) mainmeterage = Convert.ToDouble(insurancerow["main_square_meterage"]);

                // Main
                double mainVal = mainrate * mainmeterage; // Main building value

                double outrate = 0;
                double outmeterage = 0;

                if (FieldDataExists(insurancerow, "out_rate")) outrate = insurancerow["out_rate"] != DBNull.Value ? Convert.ToDouble(insurancerow["out_rate"]) : 0;
                if (FieldDataExists(insurancerow, "out_square_meterage")) outmeterage = insurancerow["out_square_meterage"] != DBNull.Value ? Convert.ToDouble(insurancerow["out_square_meterage"]) : 0;

                double outVal = outrate * outmeterage;

                double cottagerate = 0;
                double cottagemeterage = 0;

                if (FieldDataExists(insurancerow, "cottage_rate")) cottagerate = insurancerow["cottage_rate"] != DBNull.Value ? Convert.ToDouble(insurancerow["cottage_rate"]) : 0;
                if (FieldDataExists(insurancerow, "cottage_square_meterage")) cottagemeterage = insurancerow["cottage_square_meterage"] != DBNull.Value ? Convert.ToDouble(insurancerow["cottage_square_meterage"]) : 0;
                double cottageVal = cottagerate * cottagemeterage;

                //double totalSumInsured = mainVal + outVal + cottageVal; //total sum insured

                int mainRoofType =0;
                int outRoofType =0 ;
                int cottageRoofType =0 ;

                if (FieldDataExists(insurancerow, "main_roof_type_id")) mainRoofType = insurancerow["main_roof_type_id"] != DBNull.Value ? Convert.ToInt32(insurancerow["main_roof_type_id"]) : 0;
                if (FieldDataExists(insurancerow, "out_roof_type_id")) outRoofType = insurancerow["out_roof_type_id"] != DBNull.Value ? Convert.ToInt32(insurancerow["out_roof_type_id"]) : 0;
                if (FieldDataExists(insurancerow, "cottage_roof_type_id")) cottageRoofType = insurancerow["cottage_roof_type_id"] != DBNull.Value ? Convert.ToInt32(insurancerow["cottage_roof_type_id"]) : 0;

                double thatchMain = 0;
                double thatchOut = 0;
                double thatchCottage = 0;

                if (mainRoofType == 2)
                    thatchMain = mainVal;

                if (outRoofType == 2)
                    thatchOut = outVal;

                if (cottageRoofType == 2)
                    thatchCottage = cottageVal;

                double thatchAmount = thatchMain + thatchOut + thatchCottage; // Possible Thatch Value
                //double convAmount = convMain + convOut + convCottage + noMain + noOut + noCottage + otherMain + otherOut + otherCottage; // Total roof value regardless of type

                // Total improvements value
                //double totalImprovementsVal = 0;
                //if (improvementsrow != null) 
                //    totalImprovementsVal = improvementsrow["total_improvements_value"] != DBNull.Value ? Convert.ToDouble(improvementsrow["total_improvements_value"]) : 0;

                //double percentage20 = (totalSumInsured + totalImprovementsVal) * 0.2;

                double thatchOtherImprovements = 0;
                double thatchrate = 0;
                double thatchmeterage = 0;

                if (dsAdCheck.Tables["val_other_improvements_collection"] != null)
                {
                    foreach (DataRow row in dsAdCheck.Tables["val_other_improvements_collection"].Rows)
                    {
                        int roofType = 0;
                        if (FieldDataExists(row, "val_other_improvement_roof_type_id")) roofType = row["val_other_improvement_roof_type_id"] != DBNull.Value ? Convert.ToInt32(row["val_other_improvement_roof_type_id"]) : 0;

                        if (roofType == 2)
                        {
                            if (FieldDataExists(row, "rate")) thatchrate = row["rate"] != DBNull.Value ? Convert.ToDouble(row["rate"]) : 0;
                            if (FieldDataExists(row, "square_meterage")) thatchmeterage = row["square_meterage"] != DBNull.Value ? Convert.ToDouble(row["square_meterage"]) : 0;
                            thatchOtherImprovements += thatchrate * thatchmeterage;
                        }
                    }
                }

                thatchAmount = (thatchAmount + thatchOtherImprovements); // Final total thatch value
                //totalSumInsured = (totalSumInsured + totalImprovementsVal + percentage20); // final total - sum of all
                //convAmount = totalSumInsured - thatchAmount; // total conventional value (excluding thatch)


                // SET UP THE CALCULATED FIELDS

                //Main Building
                if (mainRoofType == 2) // thatch
                {
                    View.SetlblMainBuildingThatchRateValue = mainrate.ToString(SAHL.Common.Constants.CurrencyFormat);
                    View.SetlblMainBuildingThatchExtentValue = Convert.ToString(mainmeterage) + " M2";
                }
                else
                {
                    View.SetlblMainBuildingConventionalRateValue = mainrate.ToString(SAHL.Common.Constants.CurrencyFormat);
                    View.SetlblMainBuildingConventionalExtentValue = Convert.ToString(mainmeterage) + " M2";
                }

                //Cottage
                if (cottageRoofType == 2) // thatch
                {
                    View.SetlblCottageThatchRateValue = cottagerate.ToString(SAHL.Common.Constants.CurrencyFormat);
                    View.SetlblCottageThatchExtentValue = Convert.ToString(cottagemeterage) + " M2";
                }
                else
                {
                    View.SetlblCottageConventionalRateValue = cottagerate.ToString(SAHL.Common.Constants.CurrencyFormat);
                    View.SetlblCottageConventionalExtentValue = Convert.ToString(cottagemeterage) + " M2";
                }


                //OutBuilding
                if (outRoofType == 2) // thatch
                {
                    View.SetlblOutbuildingThatchRateValue = outrate.ToString(SAHL.Common.Constants.CurrencyFormat);
                    View.SetlblOutbuildingThatchExtentValue = Convert.ToString(outmeterage) + " M2";
                }
                else
                {
                    View.SetlblOutbuildingConventionalRateValue = outrate.ToString(SAHL.Common.Constants.CurrencyFormat);
                    View.SetlblOutbuildingConventionalExtentValue = Convert.ToString(outmeterage) + " M2";
                }


                //Thatch Summary
                View.SetlblThatchValue = thatchAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                View.SetlblThatchValueCheckValue = thatchrate.ToString(SAHL.Common.Constants.CurrencyFormat);
                View.SetlblThatchExtentCheckValue = Convert.ToString(thatchmeterage) + " M2";

                // Distances......
                View.SetlblThatchMinimumDistanceValue = "Not available yet.";

                if (FieldDataExists(insurancerow, "main_square_meterage")) View.SetlblUnitSizeValue = insurancerow["main_square_meterage"] + " M2";

            }
        }

        private void SetupResponse(DataRow responserow)
        {
            //Assessment Details
            if (responserow != null)
            {
                View.SetlblAssessmentNumberValue = valuations.Key.ToString();
                if (FieldDataExists(responserow, "alternate_valuation_id")) View.SetlblRequestNumberValue = responserow["alternate_valuation_id"].ToString();
                if (FieldDataExists(responserow, "val_request_reason_type_id")) View.SetlblAssessmentReasonValue = Enum.GetName(typeof(AdCheckInterfaces.val_request_reason_type), Convert.ToInt32(responserow["val_request_reason_type_id"]));
                View.SetlblRequestedbyValue = valuations.ValuationUserID;


                if (FieldDataExists(responserow, "identified")) View.SetlblCorrectPropertyValue = arrayIdentifiedType[Convert.ToInt32(responserow["identified"])];

                View.SetlblCorrectAddressValue = " ";

                // Valuation Amount
                if (FieldDataExists(responserow, "current_value")) View.SetlblValueAsIsValue = Convert.ToDouble(responserow["current_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(responserow, "cost_to_complete_value")) View.SetlblCostToCompleteValue = Convert.ToDouble(responserow["cost_to_complete_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(responserow, "completed_value")) View.SetlblValueOnCompletionValue = Convert.ToDouble(responserow["completed_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);

                // Valuation Address check
                if (FieldDataExists(responserow, "section_no")) View.SetlblStandNumberValue = Convert.ToString(responserow["section_no"]);
                if (FieldDataExists(responserow, "street_number")) View.SetlblStreetNumberValue = Convert.ToString(responserow["street_number"]);
                if (FieldDataExists(responserow, "street_name")) View.SetlblStreetNameValue = Convert.ToString(responserow["street_name"]);



                // Complex / Flat
                if (FieldDataExists(responserow, "door_no")) View.SetlblFlatNumberValue = Convert.ToString(responserow["door_no"]);
                if (FieldDataExists(responserow, "complex_details")) View.SetlblComplexNameValue = Convert.ToString(responserow["complex_details"]);
                if (FieldDataExists(responserow, "FloorsInComplex")) View.SetlblFloorsInComplexValue = Convert.ToString(responserow["FloorsInComplex"]);
                if (FieldDataExists(responserow, "UnitsInComplex")) View.SetlblUnitsInComplexValue = Convert.ToString(responserow["UnitsInComplex"]);

                //improvements comment
                if (FieldDataExists(responserow, "improvement_comment")) View.SetlblImprovementSummaryValue = Convert.ToString(responserow["improvement_comment"]);



                if (FieldDataExists(responserow, "additional_comments")) View.SettxtComment = Convert.ToString(responserow["additional_comments"]);

                //PROPERTY TAB

                // Retention
                if (FieldDataExists(responserow, "val_retention_reason_type_id")) View.SetlblRetentionReasonValue = Enum.GetName(typeof(AdCheckInterfaces.val_retention_reason_type), Convert.ToInt32(responserow["val_retention_reason_type_id"]));
                if (FieldDataExists(responserow, "retention_amount")) View.SetlblRetentionAmountValue = Convert.ToString(responserow["retention_amount"]);

                //Property 
                if (FieldDataExists(responserow, "val_erf_type_id")) View.SetlblERFTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_erf_type), Convert.ToInt32(responserow["val_erf_type_id"]));
                if (FieldDataExists(responserow, "erf_key")) View.SetlblERFNumberValue = Convert.ToString(responserow["erf_key"]);
                if (FieldDataExists(responserow, "legal_portion")) View.SetlblPortionValue = Convert.ToString(responserow["legal_portion"]);
                if (FieldDataExists(responserow, "legal_total_portions")) View.SetlblPortionOfPortionValue = Convert.ToString(responserow["legal_total_portions"]);
                if (FieldDataExists(responserow, "legal_stand_number")) View.SetlblSubdivisionPortionValue = Convert.ToString(responserow["legal_stand_number"]);
                if (FieldDataExists(responserow, "legal_land_size")) View.SetlblLandSizeValue = Convert.ToString(responserow["legal_land_size"]);
                if (FieldDataExists(responserow, "section_no")) View.SetlblSectionNumberValue = Convert.ToString(responserow["section_no"]);
                if (FieldDataExists(responserow, "legal_description")) View.SettxtLegalDescriptionValue = Convert.ToString(responserow["legal_description"]);
                if (FieldDataExists(responserow, "val_property_use_type_id")) View.SetlblPropertyUseValue = Enum.GetName(typeof(AdCheckInterfaces.val_property_use_type), Convert.ToInt32(responserow["val_property_use_type_id"]));
                if (FieldDataExists(responserow, "val_property_type_id")) View.SetlblPropertyTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_property_type), Convert.ToInt32(responserow["val_property_type_id"]));
                if (FieldDataExists(responserow, "property_comment")) View.SetlblPropertyCommentValue = Convert.ToString(responserow["property_comment"]);

                //Address
                if (FieldDataExists(responserow, "street_number") && FieldDataExists(responserow, "street_name")) View.SetlblStreetAddressValue = Convert.ToString(responserow["street_number"]) + " " + Convert.ToString(responserow["street_name"]);
                View.SetlblSuburbValue = valuations.Property.Address.RRR_SuburbDescription;
                if (FieldDataExists(responserow, "town_name")) View.SetlblCityValue = Convert.ToString(responserow["town_name"]);
                View.SetlblProvinceValue = valuations.Property.Address.RRR_ProvinceDescription;
                View.SetlblCountryValue = valuations.Property.Address.RRR_CountryDescription;
                View.SetlblPostalCodeValue = valuations.Property.Address.RRR_PostalCode;

                // Area
                if (FieldDataExists(responserow, "val_area_type_id")) View.SetlblAreaTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_area_type), Convert.ToInt32(responserow["val_area_type_id"]));
                if (FieldDataExists(responserow, "val_sector_type_id")) View.SetlblSectorTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_sector_type), Convert.ToInt32(responserow["val_sector_type_id"]));
                if (FieldDataExists(responserow, "location_comment")) View.SetlblLocalityCommentValue = Convert.ToString(responserow["location_comment"]);
                if (FieldDataExists(responserow, "market_comment")) View.SetlblMarketCommentValue = Convert.ToString(responserow["market_comment"]);
            }
        }

        private void SetupImprovements(DataRow improvementsrow)
        {
            // Swimming Pool  
            if (improvementsrow != null)
            {

                if (FieldDataExists(improvementsrow, "swimmingpool_value")) View.SetlblSwimmingPoolValue = Convert.ToDouble(improvementsrow["swimmingpool_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(improvementsrow, "val_swimmingpool_type_id")) View.SetlblSwimmingPoolTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_swimmingpool_type), Convert.ToInt32(improvementsrow["val_swimmingpool_type_id"]));
                // Tennis Court
                if (FieldDataExists(improvementsrow, "tenniscourt_value")) View.SetlblTennisCourtValue = Convert.ToDouble(improvementsrow["tenniscourt_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(improvementsrow, "val_tenniscourt_type_id")) View.SetlblTennisCourtTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_tenniscourt_type), Convert.ToInt32(improvementsrow["val_tenniscourt_type_id"]));

                // Improvement Summary

                if (FieldDataExists(improvementsrow, "total_improvements_value")) View.SetlblImprovementValue = Convert.ToDouble(improvementsrow["total_improvements_value"]).ToString(SAHL.Common.Constants.CurrencyFormat);

            }
        }

        private void SetupCottage(DataRow cottagerow)
        {
            //Cottage 
            if (cottagerow != null)
            {
                View.SetlblSCottageCottageStandNumberValue = " ";
                if (FieldDataExists(cottagerow, "total_improvements_value")) View.SetlblBedroomCountValue = Convert.ToString(cottagerow["bedroom_count"]);
                if (FieldDataExists(cottagerow, "kitchen_count")) View.SetlblCottageKitchenCountValue = Convert.ToString(cottagerow["kitchen_count"]);
                if (FieldDataExists(cottagerow, "livingroom_count")) View.SetlblCottageLivingRoomCountValue = Convert.ToString(cottagerow["livingroom_count"]);
                if (FieldDataExists(cottagerow, "other_count")) View.SetlblCottageOtherRoomCountValue = Convert.ToString(cottagerow["other_count"]);
            }
        }

        private void SetupOutbuilding(DataRow outbuildingrow)
        {
            // Outbuilding  
            if (outbuildingrow != null)
            {
                if (FieldDataExists(outbuildingrow, "bathroom_count")) View.SetlblOutbuildingBathroomCountValue = Convert.ToString(outbuildingrow["bathroom_count"]);
                if (FieldDataExists(outbuildingrow, "workshop_count")) View.SetlblOutbuildingWorkshopCountValue = Convert.ToString(outbuildingrow["workshop_count"]);
                if (FieldDataExists(outbuildingrow, "laundry_count")) View.SetlblOutbuildingLaundryCountValue = Convert.ToString(outbuildingrow["laundry_count"]);
                if (FieldDataExists(outbuildingrow, "garage_count")) View.SetlblOutbuildingGarageCountValue = Convert.ToString(outbuildingrow["garage_count"]);
                if (FieldDataExists(outbuildingrow, "bedroom_count")) View.SetlblOutbuildingBedroomCountValue = Convert.ToString(outbuildingrow["bedroom_count"]);
                if (FieldDataExists(outbuildingrow, "bathroom_count")) View.SetlblOutbuildingStoreRoomcountValue = Convert.ToString(outbuildingrow["bathroom_count"]);
                if (FieldDataExists(outbuildingrow, "kitchen_count")) View.SetlblOutbuildingKitchenCountValue = Convert.ToString(outbuildingrow["kitchen_count"]);
                if (FieldDataExists(outbuildingrow, "wc_count")) View.SetlblOutbuildingWCCountValue = Convert.ToString(outbuildingrow["wc_count"]);
            }
        }

        private void SetupInsuranceRow(DataRow insurancerow)
        {
            //Outbuilding summary 
            if (insurancerow != null)
            {

                if (FieldDataExists(insurancerow, "out_square_meterage")) View.SetlblOutbuildingExtentValue = insurancerow["out_square_meterage"] + " M2";
                if (FieldDataExists(insurancerow, "out_rate")) View.SetlblOutbuildingRateValue = Convert.ToDouble(insurancerow["out_rate"]).ToString(SAHL.Common.Constants.CurrencyFormat);

                double M2Value, RateValue, ReplacementValue;

                if (FieldDataExists(insurancerow, "out_square_meterage") && FieldDataExists(insurancerow, "out_rate"))
                {
                    M2Value = Convert.ToInt32(insurancerow["out_square_meterage"]);
                    RateValue = Convert.ToDouble(insurancerow["out_rate"]);
                    ReplacementValue = M2Value*RateValue;
                    View.SetlblOutbuildingReplacementValue = ReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                }

                // Main Building Summary
                View.SetlblClassificationValue = " ";
                if (FieldDataExists(insurancerow, "main_roof_type_id")) View.SetlblRoofTypeValue = Enum.GetName(typeof(AdCheckInterfaces.val_roof_type), Convert.ToInt32(insurancerow["main_roof_type_id"]));
                if (FieldDataExists(insurancerow, "main_square_meterage")) View.SetlblMainBuildingExtentValue = insurancerow["main_square_meterage"] + " M2";
                if (FieldDataExists(insurancerow, "main_rate")) View.SetlblMainbuildingRateValue = Convert.ToDouble(insurancerow["main_rate"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(insurancerow, "main_square_meterage") && FieldDataExists(insurancerow, "main_rate"))
                {
                    M2Value = Convert.ToInt32(insurancerow["main_square_meterage"]);
                    RateValue = Convert.ToDouble(insurancerow["main_rate"]);
                    ReplacementValue = M2Value*RateValue;
                    View.SetlblMainBuildingValue = ReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                }

                //Cottage Summary
                if (FieldDataExists(insurancerow, "cottage_square_meterage")) View.SetlblCottageExtentValue = insurancerow["cottage_square_meterage"] + " M2";
                if (FieldDataExists(insurancerow, "cottage_rate")) View.SetlblCottageRateValue = Convert.ToDouble(insurancerow["cottage_rate"]).ToString(SAHL.Common.Constants.CurrencyFormat);
                if (FieldDataExists(insurancerow, "cottage_square_meterage") && FieldDataExists(insurancerow, "cottage_rate"))
                {
                    M2Value = Convert.ToInt32(insurancerow["cottage_square_meterage"]);
                    RateValue = Convert.ToDouble(insurancerow["cottage_rate"]);
                    ReplacementValue = M2Value*RateValue;
                    View.SetlblCottageReplacementValue = ReplacementValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                }

            }
        }

        private void SetupMainbuilding(DataRow mainbuildingrow)
        {
            // Main building  
            if (mainbuildingrow != null)
            {
                if (FieldDataExists(mainbuildingrow, "bathroom_count")) View.SetlblBathRoomCountValue = Convert.ToString(mainbuildingrow["bathroom_count"]);
                if (FieldDataExists(mainbuildingrow, "study_count")) View.SetlblStudyCountValue = Convert.ToString(mainbuildingrow["study_count"]);
                if (FieldDataExists(mainbuildingrow, "familyroom_count")) View.SetlblFamilyRoomCountValue = Convert.ToString(mainbuildingrow["familyroom_count"]);
                if (FieldDataExists(mainbuildingrow, "entrance_count")) View.SetlblEntranceHallCountValue = Convert.ToString(mainbuildingrow["entrance_count"]);
                if (FieldDataExists(mainbuildingrow, "laundry_count")) View.SetlblLaundryRoomCountValue = Convert.ToString(mainbuildingrow["laundry_count"]);
                if (FieldDataExists(mainbuildingrow, "diningroom_count")) View.SetlblDiningCountValue = Convert.ToString(mainbuildingrow["diningroom_count"]);
                if (FieldDataExists(mainbuildingrow, "lounge_count")) View.SetlblLoungeCountValue = Convert.ToString(mainbuildingrow["lounge_count"]);
                if (FieldDataExists(mainbuildingrow, "bedroom_count")) View.SetlblMainBuildingBedroomCountValue = Convert.ToString(mainbuildingrow["bedroom_count"]);
                if (FieldDataExists(mainbuildingrow, "kitchen_count")) View.SetlblMainBuildingKitchenCountValue = Convert.ToString(mainbuildingrow["kitchen_count"]);
                if (FieldDataExists(mainbuildingrow, "pantry_count")) View.SetlblPantryCountValue = Convert.ToString(mainbuildingrow["pantry_count"]);
                if (FieldDataExists(mainbuildingrow, "wc_count")) View.SetlblMainBuildingWCCountValue = Convert.ToString(mainbuildingrow["wc_count"]);
                if (FieldDataExists(mainbuildingrow, "other_count")) View.SetlblOtherRoomCountValue = Convert.ToString(mainbuildingrow["other_count"]);
            }
        }


        public DataTable PopulateValuationsDS(IValuation Valuations)
        {
            // Setup the Valuations Table 
            DataTable val = new DataTable();
            val.Reset();
            val.Columns.Add("Key");
            val.Columns.Add("DataServiceProviderKey");
            val.Columns.Add("Valuer");
            val.Columns.Add("ValuationDate");
            val.Columns.Add("ValuationAmount");
            val.Columns.Add("HOCValuation");
            val.Columns.Add("ValuationPurpose");
            val.Columns.Add("RequestedBy");
            val.Columns.Add("ValuationType");
            val.Columns.Add("XMLData");
            val.TableName = "Valuations";

            DataRow valRow = val.NewRow();
            if (true) valRow["Key"] = Valuations.Key;
            if (Valuations.ValuationDataProviderDataService != null) valRow["DataServiceProviderKey"] = Valuations.ValuationDataProviderDataService.DataProviderDataService.Key;
            if (Valuations.Valuator.LegalEntity.DisplayName != null) valRow["Valuer"] = Valuations.Valuator.LegalEntity.DisplayName;
            if (true) valRow["ValuationDate"] = Convert.ToString(Valuations.ValuationDate.ToShortDateString());
            if (Valuations.ValuationAmount != null) valRow["ValuationAmount"] = Convert.ToString(Valuations.ValuationAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
            if (Valuations.ValuationHOCValue != null) valRow["HOCValuation"] = Convert.ToString(Valuations.ValuationHOCValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
            if (OfferApplicationTypeDescription != null) valRow["ValuationPurpose"] = OfferApplicationTypeDescription; //offer.ApplicationType.Description;
            if (Valuations.ValuationUserID != null) valRow["RequestedBy"] = Valuations.ValuationUserID;
            if (Valuations.ValuationDataProviderDataService != null) valRow["ValuationType"] = Valuations.ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description;
            if (Valuations.Data != null) valRow["XMLData"] = Valuations.Data;
            val.Rows.Add(valRow);

            //GlobalCacheData.Remove("ValuationDS");
            //GlobalCacheData.Add("ValuationDS", dsValuations, new List<ICacheObjectLifeTime>());
            return val;

        }


        void InitialiseXMLDataset(DataTable DSValuations)
        {
            string XMLData = null;
            for (int i = 0; i < DSValuations.Rows.Count; i++)
                if (Convert.ToUInt32(DSValuations.Rows[i][0]) == valuationkey)
                {
                    XMLData = DSValuations.Rows[i][9].ToString();
                    break;
                }

            if (XMLData != null)
                if (XMLData.Length > 0)
                {
                    System.IO.StringReader TextReader = new System.IO.StringReader(XMLData);
                    dsAdCheck.ReadXml(TextReader, XmlReadMode.Auto);
                }
        }


        public static IProperty GetSecuredPropertiesAccountKey(int AccountKey)
        {
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount acc = (IMortgageLoanAccount)accrep.GetAccountByKey(AccountKey);
            IProperty prop = acc.SecuredMortgageLoan.Property;
            return prop;
        }

        /// <summary>
        /// Checks if a Field Exists in a dataset
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool FieldDataExists(DataRow dataRow, string fieldName)
        {
            try
            {
                if (dataRow[fieldName] == null) return false;
                return dataRow.GetColumnError(fieldName).Length <= 0;
            }
            catch (Exception)
            {
                // This will happen if the field does not exist...
                return false;
            }

        }

    }
}


