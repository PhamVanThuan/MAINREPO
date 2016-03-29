using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a AdCheck Physical Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "4", Lazy = true)]
    public class ValuationDiscriminatedAdCheckPhysical_DAO : Valuation_DAO
    {
        public override ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                if (base.ValuationDataProviderDataService == null)
                    base.ValuationDataProviderDataService = ValuationDataProviderDataService_DAO.Find(4);

                return base.ValuationDataProviderDataService;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckPhysical_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckPhysical_DAO>.Find(id).As<ValuationDiscriminatedAdCheckPhysical_DAO>();
        }

        public new static ValuationDiscriminatedAdCheckPhysical_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckPhysical_DAO>.Find(id).As<ValuationDiscriminatedAdCheckPhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckPhysical_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckPhysical_DAO>.FindFirst().As<ValuationDiscriminatedAdCheckPhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckPhysical_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckPhysical_DAO>.FindOne().As<ValuationDiscriminatedAdCheckPhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckPhysical_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckPhysical_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}