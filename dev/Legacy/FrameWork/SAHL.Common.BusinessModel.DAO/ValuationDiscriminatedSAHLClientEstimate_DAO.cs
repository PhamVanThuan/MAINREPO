using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a Client Estimate Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "2", Lazy = true)]
    public class ValuationDiscriminatedSAHLClientEstimate_DAO : Valuation_DAO
    {
        public override ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                if (base.ValuationDataProviderDataService == null)
                    base.ValuationDataProviderDataService = ValuationDataProviderDataService_DAO.Find(2);

                return base.ValuationDataProviderDataService;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLClientEstimate_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLClientEstimate_DAO>.Find(id).As<ValuationDiscriminatedSAHLClientEstimate_DAO>();
        }

        public new static ValuationDiscriminatedSAHLClientEstimate_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLClientEstimate_DAO>.Find(id).As<ValuationDiscriminatedSAHLClientEstimate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLClientEstimate_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLClientEstimate_DAO>.FindFirst().As<ValuationDiscriminatedSAHLClientEstimate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLClientEstimate_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLClientEstimate_DAO>.FindOne().As<ValuationDiscriminatedSAHLClientEstimate_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLClientEstimate_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLClientEstimate_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}