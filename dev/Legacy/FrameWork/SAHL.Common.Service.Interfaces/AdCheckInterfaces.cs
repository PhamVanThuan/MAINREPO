using System;

namespace AdCheckInterfaces
{
    public enum val_erf_type
    {
        Freehold = 1,
        VacantLand = 2,
        SectionalTitle = 3
    };

    public enum val_valuation_state
    {
        PartRequested = 1,
        Requested = 2,
        Allocated = 3,
        Completed = 4,
        ProdUpdateConfirmation = 5
    };

    public enum val_priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    };

    public enum val_request_reason_type
    {
        NewPurchase = 1,
        SwitchLoan = 2,
        FurtherLoan = 3,
        ClientInArrears = 4,
        AuditValuation = 5
    };

    public enum val_area_type
    {
        Unknown = 1,
        Class1 = 2,
        Class2 = 3,
        Class3 = 4,
        Class4 = 5,
        Class5 = 6
    };

    public enum val_property_use_type
    {
        Residential = 1,
        Business = 2,
        Other = 3
    };

    public enum val_property_type
    {
        VacantLand = 1,
        Residential = 2,
        Business = 3
    };

    public enum val_smp_type
    {
        Good = 1,
        Average = 2,
        Poor = 3,
        Unacceptable = 4
    };

    public enum val_retention_reason_type
    {
        BuildingLoan = 1,
        SmallWorks = 2
    };

    public enum val_sector_type
    {
        Urban = 1,
        Rural = 2
    };

    public enum val_conditions_type
    {
        ApprovedPlans = 3,
        EngineersClearanceCertificate = 4,
        NHBRCRegistrationCertificate = 5,
        NHBRCEnrolmentCertificate = 6,
        SiteIdentificationRequired = 7,
        WaterproofCertificate = 8,
        AppointmentOfEngineer = 9,
        PestControlCertificate = 10,
        SurveyorGeneralDiagram = 11,
        BoreholeCertificate = 12,
        OccupationalCertificate = 13,
        ReplacementValue = 14
    };

    public enum val_swimmingpool_type
    {
        Concrete = 1,
        Vinyl = 2,
        Fibreglass = 3
    };

    public enum val_tenniscourt_type
    {
        Concrete = 1,
        AllWeather = 2,
        Clay = 3
    };

    public enum val_other_improvement_roof_type
    {
        Conventional = 1,
        Thatch = 2,
        NoRoof = 3,
        Other = 4,
        Shingle = 5
    };

    public enum val_roof_type
    {
        Conventional = 1,
        Thatch = 2,
        NoRoof = 3,
        Other = 4,
        Shingle = 5,
        Wooden = 6
    };

    /*
     * val_comment_improvements_type_id
     * val_comment_location_type_id
     * val_comment_market_type_id
     * val_comment_property_type_id
     * StructureType
     * WallType
     *
     *
     * val_comment_improvements_type_id	"Good",
                "Structurally unsound",
                "Poor design",
                "Physically obselete",
                "Poor positioning on site",
                "Vandalized",
                "Contravenes town planning",
                "Over capitalized",
                "Does not comply with NBR",
                "Not SAHL approved building system",
                "Incompatible with improvements",
                "In need of maintenance",
                "Functionally obselete",
                "Other",
         *		"Adequate"
     * val_comment_location_type_id	"Good",
     *			"Informal and unrecognised settlement",
                "Within 50 year flood line",
                "Adjoining industrial township",
                "Remote with difficult access",
                "High noise factor",
                "Economically depressed area",
                "High risk area",
                "Inaccessible",
                "Other"

     * val_comment_market_type_id	"Good",
                "Stable",
                "Unstable",
                "Depressed",
                "Slow moving",
                "Over supply",
                "Rising",
                "Declining",
                "Other"
     * val_comment_property_type_id	"Good",
                "Derelict",
                "Below Par",
                "Neglected",
                "Adequate",
                "Well Maintained",
                "Superior",
                "Excellent",
                "To be completed"
     * val_retention_reason_type_id	"Building Loan", "Small Works"
     * val_sector_type_id	"Urban", "Rural"
     * val_SMP_type_id	"Good", "Average", "Poor", "Unacceptable"
     * val_conditions_type_id
     * val_other_improvement_roof_type_id
     * val_roof_type_id	Conventional, Thatch, No Roof, Other
     * val_swimmingpool_type_id	Concrete, Vinyl, Fibreglass
     * val_tenniscourt_type_id	Concrete, All Weather, Clay
     * cottage_roof_type_id	Conventional, Thatch, Other, Wooden
     * main_roof_type_id	Conventional, Thatch, Other, Wooden
     * out_roof_type_id	Conventional, Thatch, Other, Wooden
     * StructureType
     * WallType
     */

    public interface IAdCheckAddress
    {
        string ComplexName { get; set; }

        double Confidence { get; set; }

        string ErfNumber { get; set; }

        double Latitude { get; set; }

        string LegalDescription { get; set; }

        double Longitude { get; set; }

        string NADCheck { get; set; }

        string Portion { get; set; }

        double Prediction { get; set; }

        int PropertyID { get; set; }

        string Province { get; set; }

        int ReferenceNumber { get; set; }

        double Safety { get; set; }

        string SellerID { get; set; }

        string SellerName { get; set; }

        string StreetName { get; set; }

        string StreetNumber { get; set; }

        string SuburbExtension { get; set; }

        string SuburbName { get; set; }

        string Town { get; set; }

        string UnitNumber { get; set; }

        string UserID { get; set; }
    }

    public interface IAdCheckValuation
    {
        //string Address { get; }
        string alternate_valuation_id { get; set; }

        double AreaUsedForCommercial { get; set; }

        decimal balance { get; set; }

        int BNPO { get; set; }

        decimal bond_amount { get; set; }

        decimal builder_contract_price { get; set; }

        string builder_name { get; set; }

        string complex_details { get; set; }

        int complex_number { get; set; }

        string conditions_comment { get; set; }

        string contact_access_details { get; set; }

        string contact1_name { get; set; }

        string contact1_phone { get; set; }

        string contact1_phone2 { get; set; }

        string contact1_phone3 { get; set; }

        string contact2_name { get; set; }

        string contact2_phone { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        DateTime date_request_completed { get; set; }

        int deleted { get; set; }

        bool desktop_valuation { get; set; }

        string door_no { get; set; }

        string erf_key { get; set; }

        int FloorsInComplex { get; set; }

        string instructions { get; set; }

        decimal insurance_amount { get; set; }

        bool irc_indicator { get; set; }

        decimal land_value { get; set; }

        int language_afrikaans { get; set; }

        string legal_description { get; set; }

        string legal_land_size { get; set; }

        string legal_portion { get; set; }

        string legal_stand_number { get; set; }

        string legal_suburb_name { get; set; }

        string legal_total_portions { get; set; }

        string legal_town { get; set; }

        decimal loan_amount { get; set; }

        bool merged { get; set; }

        DateTime prev_valuation_date { get; set; }

        string property_description { get; set; }

        DateTime purchase_date { get; set; }

        decimal purchase_price { get; set; }

        int reg_region_id { get; set; }

        //string Region { get; }
        DateTime requested_perform_date { get; set; }

        //Guid rowguid { get; }
        string section_no { get; set; }

        string street_name { get; set; }

        string street_number { get; set; }

        int sub_suburb_id { get; set; }

        string suburb_name { get; set; }

        string town_name { get; set; }

        int twn_town_id { get; set; }

        string Type { get; set; }

        int UnitsInComplex { get; set; }

        int usr_id { get; set; }

        int val_area_type_id { get; set; }

        int val_client_id { get; set; }

        int val_company_id { get; set; }

        int val_erf_type_id { get; set; }

        int val_priority_id { get; set; }

        int val_property_type_id { get; set; }

        int val_property_use_type_id { get; set; }

        int val_request_reason_type_id { get; set; }

        int val_valuation_id { get; set; }

        int val_valuation_state_id { get; set; }

        decimal valuation_amount { get; set; }
    }

    public interface IAdCheckValuationCapture
    {
        int val_capture_id { get; set; }

        int val_allocation_id { get; set; }

        System.DateTime capture_start { get; set; }

        System.DateTime capture_end { get; set; }

        decimal retention_amount { get; set; }

        decimal current_value { get; set; }

        decimal completed_value { get; set; }

        int require_senior_approval { get; set; }

        string additional_comments { get; set; }

        System.DateTime date_added { get; set; }

        System.DateTime date_modified { get; set; }

        int deleted { get; set; }

        int identified { get; set; }

        int street_address_confirmed { get; set; }
    }

    public interface IAdCheckValuationCaptureSAHL
    {
        string additional_comments { get; set; }

        DateTime capture_end { get; set; }

        DateTime capture_start { get; set; }

        decimal completed_value { get; set; }

        decimal cost_to_complete_value { get; set; }

        decimal current_value { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        int identified { get; set; }

        string improvement_comment { get; set; }

        string location_comment { get; set; }

        string market_comment { get; set; }

        string property_comment { get; set; }

        int require_senior_approval { get; set; }

        decimal retention_amount { get; set; }

        int street_address_confirmed { get; set; }

        int val_allocation_id { get; set; }

        int val_capture_id { get; set; }

        int val_comment_improvements_type_id { get; set; }

        int val_comment_location_type_id { get; set; }

        int val_comment_market_type_id { get; set; }

        int val_comment_property_type_id { get; set; }

        //IAdCheckCondition[] Val_Conditions { get; set;}
        //IAdCheckCottageBuilding Val_CottageBuilding { get; set;}
        //IAdCheckImprovements Val_Improvements { get; set;}
        //IAdCheckInsurance Val_Insurance { get; set;}
        //IAdCheckMainBuilding Val_MainBuilding { get; set;}
        //IAdCheckOtherImprovement[] Val_OtherImprovements { get; set;}
        //IAdCheckOutBuilding Val_OutBuilding { get; set;}
        int val_retention_reason_type_id { get; set; }

        int val_sector_type_id { get; set; }

        int val_SMP_type_id { get; set; }

        int val_valuation_id { get; set; }
    }

    public interface IAdCheckCondition
    {
        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        int usr_id { get; set; }

        int val_capture_conditions_id { get; set; }

        int val_capture_id { get; set; }

        int val_conditions_type_id { get; set; }
    }

    public interface IAdCheckCottageBuilding
    {
        int bathroom_count { get; set; }

        int bedroom_count { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        int kitchen_count { get; set; }

        int livingroom_count { get; set; }

        int other_count { get; set; }

        string other_description { get; set; }

        int usr_id { get; set; }

        int val_capture_cottagebuilding_count_id { get; set; }

        int val_capture_id { get; set; }
    }

    public interface IAdCheckImprovements
    {
        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        decimal other_improvements_value { get; set; }

        decimal swimmingpool_value { get; set; }

        decimal tenniscourt_value { get; set; }

        decimal total_improvements_value { get; set; }

        int usr_id { get; set; }

        int val_capture_id { get; set; }

        int val_capture_improvements_id { get; set; }

        int val_swimmingpool_type_id { get; set; }

        int val_tenniscourt_type_id { get; set; }
    }

    public interface IAdCheckInsurance
    {
        decimal cottage_rate { get; set; }

        int cottage_roof_type_id { get; set; }

        decimal cottage_square_meterage { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        decimal main_rate { get; set; }

        int main_roof_type_id { get; set; }

        decimal main_square_meterage { get; set; }

        decimal out_rate { get; set; }

        int out_roof_type_id { get; set; }

        decimal out_square_meterage { get; set; }

        int Percentage { get; set; }

        int usr_id { get; set; }

        int val_capture_id { get; set; }

        int val_capture_insurance_id { get; set; }
    }

    public interface IAdCheckMainBuilding
    {
        int bathroom_count { get; set; }

        int bedroom_count { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        int diningroom_count { get; set; }

        int entrance_count { get; set; }

        int familyroom_count { get; set; }

        int kitchen_count { get; set; }

        int laundry_count { get; set; }

        int lounge_count { get; set; }

        int other_count { get; set; }

        string other_description { get; set; }

        int pantry_count { get; set; }

        int study_count { get; set; }

        int usr_id { get; set; }

        int val_capture_id { get; set; }

        int val_capture_mainbuilding_count_id { get; set; }

        int wc_count { get; set; }
    }

    public interface IAdCheckOtherImprovement
    {
        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        string description { get; set; }

        decimal rate { get; set; }

        decimal square_meterage { get; set; }

        int usr_id { get; set; }

        int val_capture_improvement_item_id { get; set; }

        int val_capture_improvements_id { get; set; }

        int val_other_improvement_roof_type_id { get; set; }

        int val_roof_type_id { get; set; }
    }

    public interface IAdCheckOutBuilding
    {
        int bathroom_count { get; set; }

        int bedroom_count { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

        int deleted { get; set; }

        int garage_count { get; set; }

        int kitchen_count { get; set; }

        int laundry_count { get; set; }

        int other_count { get; set; }

        string other_description { get; set; }

        int storeroom_count { get; set; }

        int usr_id { get; set; }

        int val_capture_id { get; set; }

        int val_capture_outbuilding_count_id { get; set; }

        int wc_count { get; set; }

        int workshop_count { get; set; }
    }
}