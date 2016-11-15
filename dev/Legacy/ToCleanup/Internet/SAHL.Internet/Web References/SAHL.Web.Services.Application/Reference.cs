﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace SAHL.Internet.SAHL.Web.Services.Application {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ApplicationSoap", Namespace="http://webservices.sahomeloans.com/")]
    public partial class Application : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CreateLeadOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Application() {
            this.Url = global::SAHL.Internet.Properties.Settings.Default.SAHL_Internet_SAHL_Web_Services_Application_Application;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CreateLeadCompletedEventHandler CreateLeadCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/CreateLead", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CreateLead(PreProspect prospect) {
            object[] results = this.Invoke("CreateLead", new object[] {
                        prospect});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CreateLeadAsync(PreProspect prospect) {
            this.CreateLeadAsync(prospect, null);
        }
        
        /// <remarks/>
        public void CreateLeadAsync(PreProspect prospect, object userState) {
            if ((this.CreateLeadOperationCompleted == null)) {
                this.CreateLeadOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateLeadOperationCompleted);
            }
            this.InvokeAsync("CreateLead", new object[] {
                        prospect}, this.CreateLeadOperationCompleted, userState);
        }
        
        private void OnCreateLeadOperationCompleted(object arg) {
            if ((this.CreateLeadCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateLeadCompleted(this, new CreateLeadCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://webservices.sahomeloans.com/")]
    public partial class PreProspect {
        
        private int applicationKeyField;
        
        private bool offerSubmittedField;
        
        private int marketrateKeyField;
        
        private int creditMatrixKeyField;
        
        private int employmentTypeField;
        
        private int productKeyField;
        
        private double linkRateField;
        
        private double interestRateField;
        
        private double maximumInstallmentField;
        
        private int purposeNumberField;
        
        private int maturityMonthsField;
        
        private int incomeTypeField;
        
        private int numberOfApplicantsField;
        
        private int categoryKeyField;
        
        private string advertisingCampaignIDField;
        
        private string userURLField;
        
        private string referringServerURLField;
        
        private string userAddressField;
        
        private string preProspectFirstNamesField;
        
        private string preProspectSurnameField;
        
        private string preProspectIDNumberField;
        
        private string preProspectHomePhoneCodeField;
        
        private string preProspectHomePhoneNumberField;
        
        private string preProspectEmailAddressField;
        
        private int varifixMarketRateKeyField;
        
        private int marketRateTypeNumberField;
        
        private string referenceNumberField;
        
        private int rateConfigKeyField;
        
        private int mortgageLoanPurposeField;
        
        private bool interestOnlyField;
        
        private int productField;
        
        private double initiationFeeField;
        
        private double registrationFeeField;
        
        private bool capitaliseFeesField;
        
        private double cancellationFeeField;
        
        private double interimInterestField;
        
        private double valuationFeeField;
        
        private double transferFeeField;
        
        private double depositField;
        
        private double currentLoanField;
        
        private double totalFeeField;
        
        private double householdIncomeField;
        
        private double lTVField;
        
        private double pTIField;
        
        private double activeMarketRateField;
        
        private double instalmentTotalField;
        
        private double estimatedPropertyValueField;
        
        private int marginKeyField;
        
        private int termField;
        
        private double cashOutField;
        
        private double instalmentFixField;
        
        private double fixPercentField;
        
        private double purchasePriceField;
        
        private double totalPriceField;
        
        private double loanAmountRequiredField;
        
        private bool fixLoanField;
        
        private double electedFixedPercentageField;
        
        private double electedVariablePercentageField;
        
        private double electedFixedRateField;
        
        /// <remarks/>
        public int ApplicationKey {
            get {
                return this.applicationKeyField;
            }
            set {
                this.applicationKeyField = value;
            }
        }
        
        /// <remarks/>
        public bool OfferSubmitted {
            get {
                return this.offerSubmittedField;
            }
            set {
                this.offerSubmittedField = value;
            }
        }
        
        /// <remarks/>
        public int marketrateKey {
            get {
                return this.marketrateKeyField;
            }
            set {
                this.marketrateKeyField = value;
            }
        }
        
        /// <remarks/>
        public int CreditMatrixKey {
            get {
                return this.creditMatrixKeyField;
            }
            set {
                this.creditMatrixKeyField = value;
            }
        }
        
        /// <remarks/>
        public int EmploymentType {
            get {
                return this.employmentTypeField;
            }
            set {
                this.employmentTypeField = value;
            }
        }
        
        /// <remarks/>
        public int ProductKey {
            get {
                return this.productKeyField;
            }
            set {
                this.productKeyField = value;
            }
        }
        
        /// <remarks/>
        public double LinkRate {
            get {
                return this.linkRateField;
            }
            set {
                this.linkRateField = value;
            }
        }
        
        /// <remarks/>
        public double InterestRate {
            get {
                return this.interestRateField;
            }
            set {
                this.interestRateField = value;
            }
        }
        
        /// <remarks/>
        public double MaximumInstallment {
            get {
                return this.maximumInstallmentField;
            }
            set {
                this.maximumInstallmentField = value;
            }
        }
        
        /// <remarks/>
        public int PurposeNumber {
            get {
                return this.purposeNumberField;
            }
            set {
                this.purposeNumberField = value;
            }
        }
        
        /// <remarks/>
        public int MaturityMonths {
            get {
                return this.maturityMonthsField;
            }
            set {
                this.maturityMonthsField = value;
            }
        }
        
        /// <remarks/>
        public int IncomeType {
            get {
                return this.incomeTypeField;
            }
            set {
                this.incomeTypeField = value;
            }
        }
        
        /// <remarks/>
        public int NumberOfApplicants {
            get {
                return this.numberOfApplicantsField;
            }
            set {
                this.numberOfApplicantsField = value;
            }
        }
        
        /// <remarks/>
        public int CategoryKey {
            get {
                return this.categoryKeyField;
            }
            set {
                this.categoryKeyField = value;
            }
        }
        
        /// <remarks/>
        public string AdvertisingCampaignID {
            get {
                return this.advertisingCampaignIDField;
            }
            set {
                this.advertisingCampaignIDField = value;
            }
        }
        
        /// <remarks/>
        public string UserURL {
            get {
                return this.userURLField;
            }
            set {
                this.userURLField = value;
            }
        }
        
        /// <remarks/>
        public string ReferringServerURL {
            get {
                return this.referringServerURLField;
            }
            set {
                this.referringServerURLField = value;
            }
        }
        
        /// <remarks/>
        public string UserAddress {
            get {
                return this.userAddressField;
            }
            set {
                this.userAddressField = value;
            }
        }
        
        /// <remarks/>
        public string PreProspectFirstNames {
            get {
                return this.preProspectFirstNamesField;
            }
            set {
                this.preProspectFirstNamesField = value;
            }
        }
        
        /// <remarks/>
        public string PreProspectSurname {
            get {
                return this.preProspectSurnameField;
            }
            set {
                this.preProspectSurnameField = value;
            }
        }
        
        /// <remarks/>
        public string PreProspectIDNumber {
            get {
                return this.preProspectIDNumberField;
            }
            set {
                this.preProspectIDNumberField = value;
            }
        }
        
        /// <remarks/>
        public string PreProspectHomePhoneCode {
            get {
                return this.preProspectHomePhoneCodeField;
            }
            set {
                this.preProspectHomePhoneCodeField = value;
            }
        }
        
        /// <remarks/>
        public string PreProspectHomePhoneNumber {
            get {
                return this.preProspectHomePhoneNumberField;
            }
            set {
                this.preProspectHomePhoneNumberField = value;
            }
        }
        
        /// <remarks/>
        public string PreProspectEmailAddress {
            get {
                return this.preProspectEmailAddressField;
            }
            set {
                this.preProspectEmailAddressField = value;
            }
        }
        
        /// <remarks/>
        public int VarifixMarketRateKey {
            get {
                return this.varifixMarketRateKeyField;
            }
            set {
                this.varifixMarketRateKeyField = value;
            }
        }
        
        /// <remarks/>
        public int MarketRateTypeNumber {
            get {
                return this.marketRateTypeNumberField;
            }
            set {
                this.marketRateTypeNumberField = value;
            }
        }
        
        /// <remarks/>
        public string ReferenceNumber {
            get {
                return this.referenceNumberField;
            }
            set {
                this.referenceNumberField = value;
            }
        }
        
        /// <remarks/>
        public int rateConfigKey {
            get {
                return this.rateConfigKeyField;
            }
            set {
                this.rateConfigKeyField = value;
            }
        }
        
        /// <remarks/>
        public int MortgageLoanPurpose {
            get {
                return this.mortgageLoanPurposeField;
            }
            set {
                this.mortgageLoanPurposeField = value;
            }
        }
        
        /// <remarks/>
        public bool InterestOnly {
            get {
                return this.interestOnlyField;
            }
            set {
                this.interestOnlyField = value;
            }
        }
        
        /// <remarks/>
        public int Product {
            get {
                return this.productField;
            }
            set {
                this.productField = value;
            }
        }
        
        /// <remarks/>
        public double InitiationFee {
            get {
                return this.initiationFeeField;
            }
            set {
                this.initiationFeeField = value;
            }
        }
        
        /// <remarks/>
        public double RegistrationFee {
            get {
                return this.registrationFeeField;
            }
            set {
                this.registrationFeeField = value;
            }
        }
        
        /// <remarks/>
        public bool CapitaliseFees {
            get {
                return this.capitaliseFeesField;
            }
            set {
                this.capitaliseFeesField = value;
            }
        }
        
        /// <remarks/>
        public double CancellationFee {
            get {
                return this.cancellationFeeField;
            }
            set {
                this.cancellationFeeField = value;
            }
        }
        
        /// <remarks/>
        public double InterimInterest {
            get {
                return this.interimInterestField;
            }
            set {
                this.interimInterestField = value;
            }
        }
        
        /// <remarks/>
        public double ValuationFee {
            get {
                return this.valuationFeeField;
            }
            set {
                this.valuationFeeField = value;
            }
        }
        
        /// <remarks/>
        public double TransferFee {
            get {
                return this.transferFeeField;
            }
            set {
                this.transferFeeField = value;
            }
        }
        
        /// <remarks/>
        public double Deposit {
            get {
                return this.depositField;
            }
            set {
                this.depositField = value;
            }
        }
        
        /// <remarks/>
        public double CurrentLoan {
            get {
                return this.currentLoanField;
            }
            set {
                this.currentLoanField = value;
            }
        }
        
        /// <remarks/>
        public double TotalFee {
            get {
                return this.totalFeeField;
            }
            set {
                this.totalFeeField = value;
            }
        }
        
        /// <remarks/>
        public double HouseholdIncome {
            get {
                return this.householdIncomeField;
            }
            set {
                this.householdIncomeField = value;
            }
        }
        
        /// <remarks/>
        public double LTV {
            get {
                return this.lTVField;
            }
            set {
                this.lTVField = value;
            }
        }
        
        /// <remarks/>
        public double PTI {
            get {
                return this.pTIField;
            }
            set {
                this.pTIField = value;
            }
        }
        
        /// <remarks/>
        public double ActiveMarketRate {
            get {
                return this.activeMarketRateField;
            }
            set {
                this.activeMarketRateField = value;
            }
        }
        
        /// <remarks/>
        public double InstalmentTotal {
            get {
                return this.instalmentTotalField;
            }
            set {
                this.instalmentTotalField = value;
            }
        }
        
        /// <remarks/>
        public double EstimatedPropertyValue {
            get {
                return this.estimatedPropertyValueField;
            }
            set {
                this.estimatedPropertyValueField = value;
            }
        }
        
        /// <remarks/>
        public int MarginKey {
            get {
                return this.marginKeyField;
            }
            set {
                this.marginKeyField = value;
            }
        }
        
        /// <remarks/>
        public int Term {
            get {
                return this.termField;
            }
            set {
                this.termField = value;
            }
        }
        
        /// <remarks/>
        public double CashOut {
            get {
                return this.cashOutField;
            }
            set {
                this.cashOutField = value;
            }
        }
        
        /// <remarks/>
        public double InstalmentFix {
            get {
                return this.instalmentFixField;
            }
            set {
                this.instalmentFixField = value;
            }
        }
        
        /// <remarks/>
        public double FixPercent {
            get {
                return this.fixPercentField;
            }
            set {
                this.fixPercentField = value;
            }
        }
        
        /// <remarks/>
        public double PurchasePrice {
            get {
                return this.purchasePriceField;
            }
            set {
                this.purchasePriceField = value;
            }
        }
        
        /// <remarks/>
        public double TotalPrice {
            get {
                return this.totalPriceField;
            }
            set {
                this.totalPriceField = value;
            }
        }
        
        /// <remarks/>
        public double LoanAmountRequired {
            get {
                return this.loanAmountRequiredField;
            }
            set {
                this.loanAmountRequiredField = value;
            }
        }
        
        /// <remarks/>
        public bool FixLoan {
            get {
                return this.fixLoanField;
            }
            set {
                this.fixLoanField = value;
            }
        }
        
        /// <remarks/>
        public double ElectedFixedPercentage {
            get {
                return this.electedFixedPercentageField;
            }
            set {
                this.electedFixedPercentageField = value;
            }
        }
        
        /// <remarks/>
        public double ElectedVariablePercentage {
            get {
                return this.electedVariablePercentageField;
            }
            set {
                this.electedVariablePercentageField = value;
            }
        }
        
        /// <remarks/>
        public double ElectedFixedRate {
            get {
                return this.electedFixedRateField;
            }
            set {
                this.electedFixedRateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CreateLeadCompletedEventHandler(object sender, CreateLeadCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateLeadCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateLeadCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591