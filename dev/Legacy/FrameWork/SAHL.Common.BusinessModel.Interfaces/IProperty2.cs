namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IProperty
    {
        /// <summary>
        ///
        /// </summary>
        IPropertyAccessDetails PropertyAccessDetails
        {
            get;
            set;
        }

        /// <summary>
        ///Checks if this evaluation is the latest complete valuation
        /// </summary>
        IValuation LatestCompleteValuation { get; }

        bool PropertyAlreadyExistsOnOpenAccount(string desc1, string desc2, string desc3);

        bool CanPerformPropertyAdCheckValuation();

        bool CanPerformPropertyLightStoneValuation();
    }
}