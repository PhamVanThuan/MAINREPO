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

namespace SAHL.Internet.SAHL.Web.Services.SendMail {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="SendMailSoap", Namespace="http://webservices.sahomeloans.com/")]
    public partial class SendMail : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendMailorFaxOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendTextEmailInternalOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailInternalOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailWithAttachmentInternalOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailWithAttachmentsExternalOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendHTMLEmailWithAttachmentsExternalOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendFaxOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendFaxMultipleDocsOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendSMSOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SendMail() {
            this.Url = global::SAHL.Internet.Properties.Settings.Default.SAHL_Internet_localhost_SendMail;
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
        public event SendMailorFaxCompletedEventHandler SendMailorFaxCompleted;
        
        /// <remarks/>
        public event SendTextEmailInternalCompletedEventHandler SendTextEmailInternalCompleted;
        
        /// <remarks/>
        public event SendEmailInternalCompletedEventHandler SendEmailInternalCompleted;
        
        /// <remarks/>
        public event SendEmailWithAttachmentInternalCompletedEventHandler SendEmailWithAttachmentInternalCompleted;
        
        /// <remarks/>
        public event SendEmailWithAttachmentsExternalCompletedEventHandler SendEmailWithAttachmentsExternalCompleted;
        
        /// <remarks/>
        public event SendHTMLEmailWithAttachmentsExternalCompletedEventHandler SendHTMLEmailWithAttachmentsExternalCompleted;
        
        /// <remarks/>
        public event SendFaxCompletedEventHandler SendFaxCompleted;
        
        /// <remarks/>
        public event SendFaxMultipleDocsCompletedEventHandler SendFaxMultipleDocsCompleted;
        
        /// <remarks/>
        public event SendSMSCompletedEventHandler SendSMSCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendMailorFax", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendMailorFax() {
            object[] results = this.Invoke("SendMailorFax", new object[0]);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendMailorFaxAsync() {
            this.SendMailorFaxAsync(null);
        }
        
        /// <remarks/>
        public void SendMailorFaxAsync(object userState) {
            if ((this.SendMailorFaxOperationCompleted == null)) {
                this.SendMailorFaxOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendMailorFaxOperationCompleted);
            }
            this.InvokeAsync("SendMailorFax", new object[0], this.SendMailorFaxOperationCompleted, userState);
        }
        
        private void OnSendMailorFaxOperationCompleted(object arg) {
            if ((this.SendMailorFaxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendMailorFaxCompleted(this, new SendMailorFaxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendTextEmailInternal", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendTextEmailInternal(string from, string to, string cc, string bcc, string subject, string body) {
            object[] results = this.Invoke("SendTextEmailInternal", new object[] {
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendTextEmailInternalAsync(string from, string to, string cc, string bcc, string subject, string body) {
            this.SendTextEmailInternalAsync(from, to, cc, bcc, subject, body, null);
        }
        
        /// <remarks/>
        public void SendTextEmailInternalAsync(string from, string to, string cc, string bcc, string subject, string body, object userState) {
            if ((this.SendTextEmailInternalOperationCompleted == null)) {
                this.SendTextEmailInternalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendTextEmailInternalOperationCompleted);
            }
            this.InvokeAsync("SendTextEmailInternal", new object[] {
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body}, this.SendTextEmailInternalOperationCompleted, userState);
        }
        
        private void OnSendTextEmailInternalOperationCompleted(object arg) {
            if ((this.SendTextEmailInternalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendTextEmailInternalCompleted(this, new SendTextEmailInternalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendEmailInternal", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML) {
            object[] results = this.Invoke("SendEmailInternal", new object[] {
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        isBodyHTML});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailInternalAsync(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML) {
            this.SendEmailInternalAsync(from, to, cc, bcc, subject, body, isBodyHTML, null);
        }
        
        /// <remarks/>
        public void SendEmailInternalAsync(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, object userState) {
            if ((this.SendEmailInternalOperationCompleted == null)) {
                this.SendEmailInternalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailInternalOperationCompleted);
            }
            this.InvokeAsync("SendEmailInternal", new object[] {
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        isBodyHTML}, this.SendEmailInternalOperationCompleted, userState);
        }
        
        private void OnSendEmailInternalOperationCompleted(object arg) {
            if ((this.SendEmailInternalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailInternalCompleted(this, new SendEmailInternalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendEmailWithAttachmentInternal", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendEmailWithAttachmentInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, string attachment) {
            object[] results = this.Invoke("SendEmailWithAttachmentInternal", new object[] {
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        isBodyHTML,
                        attachment});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailWithAttachmentInternalAsync(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, string attachment) {
            this.SendEmailWithAttachmentInternalAsync(from, to, cc, bcc, subject, body, isBodyHTML, attachment, null);
        }
        
        /// <remarks/>
        public void SendEmailWithAttachmentInternalAsync(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, string attachment, object userState) {
            if ((this.SendEmailWithAttachmentInternalOperationCompleted == null)) {
                this.SendEmailWithAttachmentInternalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailWithAttachmentInternalOperationCompleted);
            }
            this.InvokeAsync("SendEmailWithAttachmentInternal", new object[] {
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        isBodyHTML,
                        attachment}, this.SendEmailWithAttachmentInternalOperationCompleted, userState);
        }
        
        private void OnSendEmailWithAttachmentInternalOperationCompleted(object arg) {
            if ((this.SendEmailWithAttachmentInternalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailWithAttachmentInternalCompleted(this, new SendEmailWithAttachmentInternalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendEmailWithAttachmentsExternal", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendEmailWithAttachmentsExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3) {
            object[] results = this.Invoke("SendEmailWithAttachmentsExternal", new object[] {
                        genericKey,
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        attachment1,
                        attachment2,
                        attachment3});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailWithAttachmentsExternalAsync(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3) {
            this.SendEmailWithAttachmentsExternalAsync(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3, null);
        }
        
        /// <remarks/>
        public void SendEmailWithAttachmentsExternalAsync(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, object userState) {
            if ((this.SendEmailWithAttachmentsExternalOperationCompleted == null)) {
                this.SendEmailWithAttachmentsExternalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailWithAttachmentsExternalOperationCompleted);
            }
            this.InvokeAsync("SendEmailWithAttachmentsExternal", new object[] {
                        genericKey,
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        attachment1,
                        attachment2,
                        attachment3}, this.SendEmailWithAttachmentsExternalOperationCompleted, userState);
        }
        
        private void OnSendEmailWithAttachmentsExternalOperationCompleted(object arg) {
            if ((this.SendEmailWithAttachmentsExternalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailWithAttachmentsExternalCompleted(this, new SendEmailWithAttachmentsExternalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendHTMLEmailWithAttachmentsExternal", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendHTMLEmailWithAttachmentsExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, int contentType) {
            object[] results = this.Invoke("SendHTMLEmailWithAttachmentsExternal", new object[] {
                        genericKey,
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        attachment1,
                        attachment2,
                        attachment3,
                        contentType});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendHTMLEmailWithAttachmentsExternalAsync(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, int contentType) {
            this.SendHTMLEmailWithAttachmentsExternalAsync(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3, contentType, null);
        }
        
        /// <remarks/>
        public void SendHTMLEmailWithAttachmentsExternalAsync(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, int contentType, object userState) {
            if ((this.SendHTMLEmailWithAttachmentsExternalOperationCompleted == null)) {
                this.SendHTMLEmailWithAttachmentsExternalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendHTMLEmailWithAttachmentsExternalOperationCompleted);
            }
            this.InvokeAsync("SendHTMLEmailWithAttachmentsExternal", new object[] {
                        genericKey,
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        attachment1,
                        attachment2,
                        attachment3,
                        contentType}, this.SendHTMLEmailWithAttachmentsExternalOperationCompleted, userState);
        }
        
        private void OnSendHTMLEmailWithAttachmentsExternalOperationCompleted(object arg) {
            if ((this.SendHTMLEmailWithAttachmentsExternalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendHTMLEmailWithAttachmentsExternalCompleted(this, new SendHTMLEmailWithAttachmentsExternalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendFax", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendFax(int genericKey, string from, string faxNumber, string faxAttachment) {
            object[] results = this.Invoke("SendFax", new object[] {
                        genericKey,
                        from,
                        faxNumber,
                        faxAttachment});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendFaxAsync(int genericKey, string from, string faxNumber, string faxAttachment) {
            this.SendFaxAsync(genericKey, from, faxNumber, faxAttachment, null);
        }
        
        /// <remarks/>
        public void SendFaxAsync(int genericKey, string from, string faxNumber, string faxAttachment, object userState) {
            if ((this.SendFaxOperationCompleted == null)) {
                this.SendFaxOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendFaxOperationCompleted);
            }
            this.InvokeAsync("SendFax", new object[] {
                        genericKey,
                        from,
                        faxNumber,
                        faxAttachment}, this.SendFaxOperationCompleted, userState);
        }
        
        private void OnSendFaxOperationCompleted(object arg) {
            if ((this.SendFaxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendFaxCompleted(this, new SendFaxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendFaxMultipleDocs", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendFaxMultipleDocs(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3) {
            object[] results = this.Invoke("SendFaxMultipleDocs", new object[] {
                        genericKey,
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        attachment1,
                        attachment2,
                        attachment3});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendFaxMultipleDocsAsync(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3) {
            this.SendFaxMultipleDocsAsync(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3, null);
        }
        
        /// <remarks/>
        public void SendFaxMultipleDocsAsync(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, object userState) {
            if ((this.SendFaxMultipleDocsOperationCompleted == null)) {
                this.SendFaxMultipleDocsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendFaxMultipleDocsOperationCompleted);
            }
            this.InvokeAsync("SendFaxMultipleDocs", new object[] {
                        genericKey,
                        from,
                        to,
                        cc,
                        bcc,
                        subject,
                        body,
                        attachment1,
                        attachment2,
                        attachment3}, this.SendFaxMultipleDocsOperationCompleted, userState);
        }
        
        private void OnSendFaxMultipleDocsOperationCompleted(object arg) {
            if ((this.SendFaxMultipleDocsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendFaxMultipleDocsCompleted(this, new SendFaxMultipleDocsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.sahomeloans.com/SendSMS", RequestNamespace="http://webservices.sahomeloans.com/", ResponseNamespace="http://webservices.sahomeloans.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendSMS(int genericKey, string message, string cellphoneNumber) {
            object[] results = this.Invoke("SendSMS", new object[] {
                        genericKey,
                        message,
                        cellphoneNumber});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendSMSAsync(int genericKey, string message, string cellphoneNumber) {
            this.SendSMSAsync(genericKey, message, cellphoneNumber, null);
        }
        
        /// <remarks/>
        public void SendSMSAsync(int genericKey, string message, string cellphoneNumber, object userState) {
            if ((this.SendSMSOperationCompleted == null)) {
                this.SendSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendSMSOperationCompleted);
            }
            this.InvokeAsync("SendSMS", new object[] {
                        genericKey,
                        message,
                        cellphoneNumber}, this.SendSMSOperationCompleted, userState);
        }
        
        private void OnSendSMSOperationCompleted(object arg) {
            if ((this.SendSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendSMSCompleted(this, new SendSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendMailorFaxCompletedEventHandler(object sender, SendMailorFaxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendMailorFaxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendMailorFaxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendTextEmailInternalCompletedEventHandler(object sender, SendTextEmailInternalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendTextEmailInternalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendTextEmailInternalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendEmailInternalCompletedEventHandler(object sender, SendEmailInternalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailInternalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailInternalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendEmailWithAttachmentInternalCompletedEventHandler(object sender, SendEmailWithAttachmentInternalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailWithAttachmentInternalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailWithAttachmentInternalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendEmailWithAttachmentsExternalCompletedEventHandler(object sender, SendEmailWithAttachmentsExternalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailWithAttachmentsExternalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailWithAttachmentsExternalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendHTMLEmailWithAttachmentsExternalCompletedEventHandler(object sender, SendHTMLEmailWithAttachmentsExternalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendHTMLEmailWithAttachmentsExternalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendHTMLEmailWithAttachmentsExternalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendFaxCompletedEventHandler(object sender, SendFaxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendFaxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendFaxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendFaxMultipleDocsCompletedEventHandler(object sender, SendFaxMultipleDocsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendFaxMultipleDocsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendFaxMultipleDocsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendSMSCompletedEventHandler(object sender, SendSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591