﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Web.Public.AttorneyService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceMessage", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class ServiceMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SAHL.Web.Public.AttorneyService.Message[] ServiceMessagesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SAHL.Web.Public.AttorneyService.Message[] ServiceMessages {
            get {
                return this.ServiceMessagesField;
            }
            set {
                if ((object.ReferenceEquals(this.ServiceMessagesField, value) != true)) {
                    this.ServiceMessagesField = value;
                    this.RaisePropertyChanged("ServiceMessages");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SAHL.Web.Public.AttorneyService.ServiceMessageType MessageTypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SAHL.Web.Public.AttorneyService.ServiceMessageType MessageType {
            get {
                return this.MessageTypeField;
            }
            set {
                if ((this.MessageTypeField.Equals(value) != true)) {
                    this.MessageTypeField = value;
                    this.RaisePropertyChanged("MessageType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceMessageType", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    public enum ServiceMessageType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Info = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Success = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Warning = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Error = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ReportParameter", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class ReportParameter : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ParameterTypeKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReportParameterNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SAHL.Web.Public.AttorneyService.ReportParameter[] ReportParamsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ParameterTypeKey {
            get {
                return this.ParameterTypeKeyField;
            }
            set {
                if ((this.ParameterTypeKeyField.Equals(value) != true)) {
                    this.ParameterTypeKeyField = value;
                    this.RaisePropertyChanged("ParameterTypeKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReportParameterName {
            get {
                return this.ReportParameterNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ReportParameterNameField, value) != true)) {
                    this.ReportParameterNameField = value;
                    this.RaisePropertyChanged("ReportParameterName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SAHL.Web.Public.AttorneyService.ReportParameter[] ReportParams {
            get {
                return this.ReportParamsField;
            }
            set {
                if ((object.ReferenceEquals(this.ReportParamsField, value) != true)) {
                    this.ReportParamsField = value;
                    this.RaisePropertyChanged("ReportParams");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="NoteDetail", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class NoteDetail : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime InsertedDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int KeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LegalEntityDisplayNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LegalEntityKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NoteTextField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TagField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WorkflowStateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime InsertedDate {
            get {
                return this.InsertedDateField;
            }
            set {
                if ((this.InsertedDateField.Equals(value) != true)) {
                    this.InsertedDateField = value;
                    this.RaisePropertyChanged("InsertedDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Key {
            get {
                return this.KeyField;
            }
            set {
                if ((this.KeyField.Equals(value) != true)) {
                    this.KeyField = value;
                    this.RaisePropertyChanged("Key");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LegalEntityDisplayName {
            get {
                return this.LegalEntityDisplayNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LegalEntityDisplayNameField, value) != true)) {
                    this.LegalEntityDisplayNameField = value;
                    this.RaisePropertyChanged("LegalEntityDisplayName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LegalEntityKey {
            get {
                return this.LegalEntityKeyField;
            }
            set {
                if ((this.LegalEntityKeyField.Equals(value) != true)) {
                    this.LegalEntityKeyField = value;
                    this.RaisePropertyChanged("LegalEntityKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NoteText {
            get {
                return this.NoteTextField;
            }
            set {
                if ((object.ReferenceEquals(this.NoteTextField, value) != true)) {
                    this.NoteTextField = value;
                    this.RaisePropertyChanged("NoteText");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Tag {
            get {
                return this.TagField;
            }
            set {
                if ((object.ReferenceEquals(this.TagField, value) != true)) {
                    this.TagField = value;
                    this.RaisePropertyChanged("Tag");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WorkflowState {
            get {
                return this.WorkflowStateField;
            }
            set {
                if ((object.ReferenceEquals(this.WorkflowStateField, value) != true)) {
                    this.WorkflowStateField = value;
                    this.RaisePropertyChanged("WorkflowState");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DebtCounselling", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class DebtCounselling : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AccountKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DebtCounsellingKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> DiaryDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SAHL.Web.Public.AttorneyService.LegalEntity[] LegalEntitiesOnAccountField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AccountKey {
            get {
                return this.AccountKeyField;
            }
            set {
                if ((this.AccountKeyField.Equals(value) != true)) {
                    this.AccountKeyField = value;
                    this.RaisePropertyChanged("AccountKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DebtCounsellingKey {
            get {
                return this.DebtCounsellingKeyField;
            }
            set {
                if ((this.DebtCounsellingKeyField.Equals(value) != true)) {
                    this.DebtCounsellingKeyField = value;
                    this.RaisePropertyChanged("DebtCounsellingKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> DiaryDate {
            get {
                return this.DiaryDateField;
            }
            set {
                if ((this.DiaryDateField.Equals(value) != true)) {
                    this.DiaryDateField = value;
                    this.RaisePropertyChanged("DiaryDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SAHL.Web.Public.AttorneyService.LegalEntity[] LegalEntitiesOnAccount {
            get {
                return this.LegalEntitiesOnAccountField;
            }
            set {
                if ((object.ReferenceEquals(this.LegalEntitiesOnAccountField, value) != true)) {
                    this.LegalEntitiesOnAccountField = value;
                    this.RaisePropertyChanged("LegalEntitiesOnAccount");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LegalEntity", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class LegalEntity : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DisplayNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDNumberField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DisplayName {
            get {
                return this.DisplayNameField;
            }
            set {
                if ((object.ReferenceEquals(this.DisplayNameField, value) != true)) {
                    this.DisplayNameField = value;
                    this.RaisePropertyChanged("DisplayName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IDNumber {
            get {
                return this.IDNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.IDNumberField, value) != true)) {
                    this.IDNumberField = value;
                    this.RaisePropertyChanged("IDNumber");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Proposal", Namespace="http://schemas.datacontract.org/2004/07/SAHL.Web.Services.Internal.DataModel")]
    [System.SerializableAttribute()]
    public partial class Proposal : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> AcceptedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreateDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DebtCounsellingKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> HOCInclusiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LegalEntityDisplayNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> LifeInclusiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ProposalKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProposalStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProposalTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> ReviewDateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> Accepted {
            get {
                return this.AcceptedField;
            }
            set {
                if ((this.AcceptedField.Equals(value) != true)) {
                    this.AcceptedField = value;
                    this.RaisePropertyChanged("Accepted");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreateDate {
            get {
                return this.CreateDateField;
            }
            set {
                if ((this.CreateDateField.Equals(value) != true)) {
                    this.CreateDateField = value;
                    this.RaisePropertyChanged("CreateDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DebtCounsellingKey {
            get {
                return this.DebtCounsellingKeyField;
            }
            set {
                if ((this.DebtCounsellingKeyField.Equals(value) != true)) {
                    this.DebtCounsellingKeyField = value;
                    this.RaisePropertyChanged("DebtCounsellingKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> HOCInclusive {
            get {
                return this.HOCInclusiveField;
            }
            set {
                if ((this.HOCInclusiveField.Equals(value) != true)) {
                    this.HOCInclusiveField = value;
                    this.RaisePropertyChanged("HOCInclusive");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LegalEntityDisplayName {
            get {
                return this.LegalEntityDisplayNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LegalEntityDisplayNameField, value) != true)) {
                    this.LegalEntityDisplayNameField = value;
                    this.RaisePropertyChanged("LegalEntityDisplayName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> LifeInclusive {
            get {
                return this.LifeInclusiveField;
            }
            set {
                if ((this.LifeInclusiveField.Equals(value) != true)) {
                    this.LifeInclusiveField = value;
                    this.RaisePropertyChanged("LifeInclusive");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ProposalKey {
            get {
                return this.ProposalKeyField;
            }
            set {
                if ((this.ProposalKeyField.Equals(value) != true)) {
                    this.ProposalKeyField = value;
                    this.RaisePropertyChanged("ProposalKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProposalStatus {
            get {
                return this.ProposalStatusField;
            }
            set {
                if ((object.ReferenceEquals(this.ProposalStatusField, value) != true)) {
                    this.ProposalStatusField = value;
                    this.RaisePropertyChanged("ProposalStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProposalType {
            get {
                return this.ProposalTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ProposalTypeField, value) != true)) {
                    this.ProposalTypeField = value;
                    this.RaisePropertyChanged("ProposalType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> ReviewDate {
            get {
                return this.ReviewDateField;
            }
            set {
                if ((this.ReviewDateField.Equals(value) != true)) {
                    this.ReviewDateField = value;
                    this.RaisePropertyChanged("ReviewDate");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AttorneyService.IAttorney")]
    public interface IAttorney {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/GetServiceMessage", ReplyAction="http://tempuri.org/IAttorney/GetServiceMessageResponse")]
        SAHL.Web.Public.AttorneyService.ServiceMessage GetServiceMessage(string legalEntityKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/Login", ReplyAction="http://tempuri.org/IAttorney/LoginResponse")]
        bool Login(out string leKey, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/RegisterUser", ReplyAction="http://tempuri.org/IAttorney/RegisterUserResponse")]
        bool RegisterUser(string emailAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/RenderSQLReport", ReplyAction="http://tempuri.org/IAttorney/RenderSQLReportResponse")]
        byte[] RenderSQLReport(int reportkey, System.Collections.Generic.Dictionary<string, string> sqlReportParameters, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/GetReportParametersByStatementKey", ReplyAction="http://tempuri.org/IAttorney/GetReportParametersByStatementKeyResponse")]
        SAHL.Web.Public.AttorneyService.ReportParameter[] GetReportParametersByStatementKey(int ReportStatementKey, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/ForgottenPassword", ReplyAction="http://tempuri.org/IAttorney/ForgottenPasswordResponse")]
        bool ForgottenPassword(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/GetNotesByDebtCounselling", ReplyAction="http://tempuri.org/IAttorney/GetNotesByDebtCounsellingResponse")]
        SAHL.Web.Public.AttorneyService.NoteDetail[] GetNotesByDebtCounselling(int debtCounsellingKey, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/GetDebtCounsellingByKey", ReplyAction="http://tempuri.org/IAttorney/GetDebtCounsellingByKeyResponse")]
        SAHL.Web.Public.AttorneyService.DebtCounselling GetDebtCounsellingByKey(int debtCounsellingKey, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/SearchForCases", ReplyAction="http://tempuri.org/IAttorney/SearchForCasesResponse")]
        SAHL.Web.Public.AttorneyService.DebtCounselling[] SearchForCases(int legalEntityKey, int accountNumber, string idNumber, string legalEntityName, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/SaveNoteDetail", ReplyAction="http://tempuri.org/IAttorney/SaveNoteDetailResponse")]
        bool SaveNoteDetail(SAHL.Web.Public.AttorneyService.NoteDetail noteDetail, int legalEntityKey, int debtCounsellingKey, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAttorney/GetProposals", ReplyAction="http://tempuri.org/IAttorney/GetProposalsResponse")]
        SAHL.Web.Public.AttorneyService.Proposal[] GetProposals(int debtCounsellingKey, string username, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAttorneyChannel : SAHL.Web.Public.AttorneyService.IAttorney, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AttorneyClient : System.ServiceModel.ClientBase<SAHL.Web.Public.AttorneyService.IAttorney>, SAHL.Web.Public.AttorneyService.IAttorney {
        
        public AttorneyClient() {
        }
        
        public AttorneyClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AttorneyClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AttorneyClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AttorneyClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SAHL.Web.Public.AttorneyService.ServiceMessage GetServiceMessage(string legalEntityKey) {
            return base.Channel.GetServiceMessage(legalEntityKey);
        }
        
        public bool Login(out string leKey, string username, string password) {
            return base.Channel.Login(out leKey, username, password);
        }
        
        public bool RegisterUser(string emailAddress) {
            return base.Channel.RegisterUser(emailAddress);
        }
        
        public byte[] RenderSQLReport(int reportkey, System.Collections.Generic.Dictionary<string, string> sqlReportParameters, string username, string password) {
            return base.Channel.RenderSQLReport(reportkey, sqlReportParameters, username, password);
        }
        
        public SAHL.Web.Public.AttorneyService.ReportParameter[] GetReportParametersByStatementKey(int ReportStatementKey, string username, string password) {
            return base.Channel.GetReportParametersByStatementKey(ReportStatementKey, username, password);
        }
        
        public bool ForgottenPassword(string userName) {
            return base.Channel.ForgottenPassword(userName);
        }
        
        public SAHL.Web.Public.AttorneyService.NoteDetail[] GetNotesByDebtCounselling(int debtCounsellingKey, string username, string password) {
            return base.Channel.GetNotesByDebtCounselling(debtCounsellingKey, username, password);
        }
        
        public SAHL.Web.Public.AttorneyService.DebtCounselling GetDebtCounsellingByKey(int debtCounsellingKey, string username, string password) {
            return base.Channel.GetDebtCounsellingByKey(debtCounsellingKey, username, password);
        }
        
        public SAHL.Web.Public.AttorneyService.DebtCounselling[] SearchForCases(int legalEntityKey, int accountNumber, string idNumber, string legalEntityName, string username, string password) {
            return base.Channel.SearchForCases(legalEntityKey, accountNumber, idNumber, legalEntityName, username, password);
        }
        
        public bool SaveNoteDetail(SAHL.Web.Public.AttorneyService.NoteDetail noteDetail, int legalEntityKey, int debtCounsellingKey, string username, string password) {
            return base.Channel.SaveNoteDetail(noteDetail, legalEntityKey, debtCounsellingKey, username, password);
        }
        
        public SAHL.Web.Public.AttorneyService.Proposal[] GetProposals(int debtCounsellingKey, string username, string password) {
            return base.Channel.GetProposals(debtCounsellingKey, username, password);
        }
    }
}