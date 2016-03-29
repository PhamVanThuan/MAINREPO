using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a AdCheck Desktop Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "5", Lazy = true)]
    public class ValuationDiscriminatedAdCheckDesktop_DAO : Valuation_DAO
    {
        public override ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                if (base.ValuationDataProviderDataService == null)
                    base.ValuationDataProviderDataService = ValuationDataProviderDataService_DAO.Find(5);

                return base.ValuationDataProviderDataService;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckDesktop_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckDesktop_DAO>.Find(id).As<ValuationDiscriminatedAdCheckDesktop_DAO>();
        }

        public new static ValuationDiscriminatedAdCheckDesktop_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckDesktop_DAO>.Find(id).As<ValuationDiscriminatedAdCheckDesktop_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckDesktop_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckDesktop_DAO>.FindFirst().As<ValuationDiscriminatedAdCheckDesktop_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckDesktop_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckDesktop_DAO>.FindOne().As<ValuationDiscriminatedAdCheckDesktop_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedAdCheckDesktop_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedAdCheckDesktop_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}