using System;

//using SAHL.Common.UI;
using System.Data;

namespace SAHL.Common.Service.Interfaces
{
    public interface IAdCheckService
    {
        void Initialise(int GenericKey, int GenericKeyTypeKey);

        DataSet ValidateAddress(int ReferenceNumber, string SellerID, string UnitNo, string ComplexName, string StreetNo, string StreetName, string Suburb, string Town, string Province, string ErfNumber, string ErfPortionNumber);

        int RequestValuation(
            int OfferKey,
            int ValuatorID, //companyid
            AdCheckInterfaces.val_priority Priority,
            AdCheckInterfaces.val_request_reason_type RequestReason,
            DateTime RequestedPerformDate,
            decimal PurchasePrice,
            DateTime PurchaseDate,
            decimal LoanAmount,
            decimal Balance,
            string Contact1Name,
            string Contact1Home,
            string Contact1Work,
            string Contact1Cell,
            string Contact2Name,
            string Contact2Phone,
            string Instructions,
            int SuburbID,
            int PropertyID,
            AdCheckInterfaces.val_area_type AreaType,
            AdCheckInterfaces.val_property_use_type PropertyUseType,
            AdCheckInterfaces.val_property_type PropertyType,
            AdCheckInterfaces.val_erf_type ErfType,
            string ContactAccessDetails,
            string OfPortion,
            string LegalDescription,
            string LandSize);

        string WithdrawRequest(int ReferenceNumber);

        DataTable GetCompletedValuations();

        DataSet RetrieveValuationDetails(int ValuationID);

        DataSet RetrieveValuationCaptureDetails(int ValuationID);

        DataSet RetrieveAllValuationDetails(int ValuationID);
    }

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
        string Address { get; }

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

        string Region { get; }

        DateTime requested_perform_date { get; set; }

        Guid rowguid { get; }

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

        DateTime capture_start { get; set; }

        DateTime capture_end { get; set; }

        decimal retention_amount { get; set; }

        decimal current_value { get; set; }

        decimal completed_value { get; set; }

        int require_senior_approval { get; set; }

        string additional_comments { get; set; }

        DateTime date_added { get; set; }

        DateTime date_modified { get; set; }

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

        IAdCheckCondition[] Val_Conditions { get; set; }

        IAdCheckCottageBuilding Val_CottageBuilding { get; set; }

        IAdCheckImprovements Val_Improvements { get; set; }

        IAdCheckInsurance Val_Insurance { get; set; }

        IAdCheckMainBuilding Val_MainBuilding { get; set; }

        IAdCheckOtherImprovement[] Val_OtherImprovements { get; set; }

        IAdCheckOutBuilding Val_OutBuilding { get; set; }

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