using System;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The lurker attribute is used to specify classes and properties in the DAO assembly that should not be exposed in the bussiness model implementation, nor
    /// it's interface.  These classes and properties will be present to be used in the business model, but not exposed. They will therfore lurk.  The lurkee parameter can be set
    /// to specify the person responsible for marking something as a lurker.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    internal class LurkerAttribute : Attribute
    {
        protected string _lurkee;

        public string Lurkee
        {
            get
            {
                return _lurkee;
            }
            set
            {
                _lurkee = value;
            }
        }
    }
}