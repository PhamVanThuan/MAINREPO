using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SAHL.WCF.Validation.Engine
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class ParameterValidatorAttribute : Attribute, IOperationBehavior, IContractBehavior
    {
        public ParameterValidatorAttribute()
        {
            ThrowErrorOnFirstError = false;
            ThrowErrorAfterValidation = true;
        }

        public bool ThrowErrorOnFirstError { get; set; }
        public bool ThrowErrorAfterValidation { get; set; }

        #region -- IContractBehavior Members --
        public void Validate(OperationDescription description)
        {
            //not required in our implentation for Comcorp.   
        }

        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
            //not required in our implentation for Comcorp.
        }

        public void ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
        {
            //not required in our implentation for Comcorp.
        }

        public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
        {
            dispatch.ParameterInspectors.Add(new ParameterValidatorBehavior(ThrowErrorOnFirstError, ThrowErrorAfterValidation));
        }
        #endregion

        #region -- IContractBehavior Members --
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            //not required in our implentation for Comcorp.
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //not required in our implentation for Comcorp.
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //not required in our implentation for Comcorp.
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            foreach (DispatchOperation op in dispatchRuntime.Operations)
            {
                op.ParameterInspectors.Add(new ParameterValidatorBehavior(ThrowErrorOnFirstError, ThrowErrorAfterValidation));
            }
        }
        #endregion
    }
}