﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoanServicingTests {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class LoanServicingTests : global::System.Configuration.ApplicationSettingsBase {
        
        private static LoanServicingTests defaultInstance = ((LoanServicingTests)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new LoanServicingTests())));
        
        public static LoanServicingTests Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ShowMaximized")]
        public string BrowserVisability {
            get {
                return ((string)(this["BrowserVisability"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("sahls203b")]
        public string SAHLDataBaseServer {
            get {
                return ((string)(this["SAHLDataBaseServer"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1")]
        public int WaitTimeOut {
            get {
                return ((int)(this["WaitTimeOut"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("sahls215b")]
        public string SQLReportServer {
            get {
                return ((string)(this["SQLReportServer"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://sahls216b/Halo/")]
        public string HaloWebServiceURL {
            get {
                return ((string)(this["HaloWebServiceURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://sahls216b:81/Valuation.svc")]
        public string EzValWebserviceUrl {
            get {
                return ((string)(this["EzValWebserviceUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://testwebservices.sahomeloans.com:8083/Valuation.svc")]
        public string EzValPublicWebserviceUrl {
            get {
                return ((string)(this["EzValPublicWebserviceUrl"]));
            }
        }
    }
}
