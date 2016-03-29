using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a Lightstone Automated Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    [GenericTest(TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "3", Lazy = true)]
    public class ValuationDiscriminatedLightstoneAVM_DAO : Valuation_DAO
    {
        public override ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                if (base.ValuationDataProviderDataService == null)
                    base.ValuationDataProviderDataService = ValuationDataProviderDataService_DAO.Find(3);

                return base.ValuationDataProviderDataService;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightstoneAVM_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedLightstoneAVM_DAO>.Find(id).As<ValuationDiscriminatedLightstoneAVM_DAO>();
        }

        public new static ValuationDiscriminatedLightstoneAVM_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedLightstoneAVM_DAO>.Find(id).As<ValuationDiscriminatedLightstoneAVM_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightstoneAVM_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedLightstoneAVM_DAO>.FindFirst().As<ValuationDiscriminatedLightstoneAVM_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightstoneAVM_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedLightstoneAVM_DAO>.FindOne().As<ValuationDiscriminatedLightstoneAVM_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedLightstoneAVM_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedLightstoneAVM_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}