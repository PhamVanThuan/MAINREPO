using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IStageActivity : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.Int32 StageDefinitionKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 ID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IActivity Activity
        {
            get;
            set;
        }
    }
}