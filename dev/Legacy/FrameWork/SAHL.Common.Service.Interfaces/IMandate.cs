using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.Service.Interfaces
{
    public interface IMandate
    {
        bool StartMandate(IAllocationMandate Mandate);

        bool ExecuteMandate(params object[] args);

        void CompleteMandate();
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class MandateAssemblyTag : System.Attribute
    {
        public MandateAssemblyTag()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MandateInfo : System.Attribute
    {
    }

    public interface IMandateService
    {
        IList<IADUser> ExecuteMandateSet(string MandateSet, params object[] parameters);

        bool ExecuteMandate(string Mandate, params object[] parameters);
    }
}