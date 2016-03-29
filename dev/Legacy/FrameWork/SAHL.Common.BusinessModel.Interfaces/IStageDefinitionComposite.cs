using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IStageDefinitionComposite : IEntityValidation, IStageDefinition
    {
        /// <summary>
        ///
        /// </summary>
        System.String CompositeTypeName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IStageDefinition> ChildStageDefinitions
        {
            get;
        }
    }
}