﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18047
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Batch.Service.PersonalLoanService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PersonalLoanService.IPersonalLoan")]
    public interface IPersonalLoan {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPersonalLoan/CreatePersonalLoanLeadFromIdNumber", ReplyAction="http://tempuri.org/IPersonalLoan/CreatePersonalLoanLeadFromIdNumberResponse")]
        bool CreatePersonalLoanLeadFromIdNumber(string idNumber, int messageId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPersonalLoan/CreatePersonalLoanLeadFromIdNumber", ReplyAction="http://tempuri.org/IPersonalLoan/CreatePersonalLoanLeadFromIdNumberResponse")]
        System.Threading.Tasks.Task<bool> CreatePersonalLoanLeadFromIdNumberAsync(string idNumber, int messageId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPersonalLoanChannel : SAHL.Batch.Service.PersonalLoanService.IPersonalLoan, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PersonalLoanClient : System.ServiceModel.ClientBase<SAHL.Batch.Service.PersonalLoanService.IPersonalLoan>, SAHL.Batch.Service.PersonalLoanService.IPersonalLoan {
        
        public PersonalLoanClient() {
        }
        
        public PersonalLoanClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PersonalLoanClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PersonalLoanClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PersonalLoanClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool CreatePersonalLoanLeadFromIdNumber(string idNumber, int messageId) {
            return base.Channel.CreatePersonalLoanLeadFromIdNumber(idNumber, messageId);
        }
        
        public System.Threading.Tasks.Task<bool> CreatePersonalLoanLeadFromIdNumberAsync(string idNumber, int messageId) {
            return base.Channel.CreatePersonalLoanLeadFromIdNumberAsync(idNumber, messageId);
        }
    }
}
