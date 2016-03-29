using System;

namespace SAHL.Common.BusinessModel.DAO.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    internal class ConstructorInjectorAttribute : Attribute
    {
        protected string _functionName;

        /// <summary>
        /// The function to call during construction
        /// </summary>
        public string FunctionName
        {
            get
            {
                return _functionName;
            }
            set
            {
                _functionName = value;
            }
        }
    }
}