using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a Lightstone Physical Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "7", Lazy = true)]
    public class ValuationDiscriminatedLightStonePhysical_DAO : Valuation_DAO
    {
        public override ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                if (base.ValuationDataProviderDataService == null)
                    base.ValuationDataProviderDataService = ValuationDataProviderDataService_DAO.Find(7);

                return base.ValuationDataProviderDataService;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightStonePhysical_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedLightStonePhysical_DAO>.Find(id).As<ValuationDiscriminatedLightStonePhysical_DAO>();
        }

        public static ValuationDiscriminatedLightStonePhysical_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedLightStonePhysical_DAO>.Find(id).As<ValuationDiscriminatedLightStonePhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightStonePhysical_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedLightStonePhysical_DAO>.FindFirst().As<ValuationDiscriminatedLightStonePhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightStonePhysical_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedLightStonePhysical_DAO>.FindOne().As<ValuationDiscriminatedLightStonePhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightStonePhysical_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedLightStonePhysical_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}