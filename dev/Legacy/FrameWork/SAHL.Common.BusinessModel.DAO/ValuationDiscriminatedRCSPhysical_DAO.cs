using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents an RCS Physical Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "6", Lazy = true)]
    public class ValuationDiscriminatedRCSPhysical_DAO : Valuation_DAO
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
        public static ValuationDiscriminatedRCSPhysical_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedRCSPhysical_DAO>.Find(id).As<ValuationDiscriminatedRCSPhysical_DAO>();
        }

        public new static ValuationDiscriminatedRCSPhysical_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedRCSPhysical_DAO>.Find(id).As<ValuationDiscriminatedRCSPhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedRCSPhysical_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedRCSPhysical_DAO>.FindFirst().As<ValuationDiscriminatedRCSPhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedRCSPhysical_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedRCSPhysical_DAO>.FindOne().As<ValuationDiscriminatedRCSPhysical_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static new ValuationDiscriminatedRCSPhysical_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedRCSPhysical_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}