using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a SAHL Manual Valuation.
    /// </summary>
    /// <seealso cref="ValuationDataProviderDataService_DAO"/>
    /// <seealso cref="ValuationOutbuilding_DAO"/>
    /// <seealso cref="ValuationMainBuilding_DAO"/>
    /// <seealso cref="ValuationCombinedThatch_DAO"/>
    /// <seealso cref="ValuationImprovement_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "1", Lazy = true)]
    public class ValuationDiscriminatedSAHLManual_DAO : Valuation_DAO
    {
        public override ValuationDataProviderDataService_DAO ValuationDataProviderDataService
        {
            get
            {
                if (base.ValuationDataProviderDataService == null)
                    base.ValuationDataProviderDataService = ValuationDataProviderDataService_DAO.Find(1);

                return base.ValuationDataProviderDataService;
            }
        }

        //private ValuationClassification_DAO _valuationClassification;
        //private double? _valuationEscalationPercentage;
        private ValuationMainBuilding_DAO _valuationMainBuilding;
        private IList<ValuationOutbuilding_DAO> _valuationOutBuildings;
        private ValuationCombinedThatch_DAO _valuationCombinedThatch;
        private IList<ValuationImprovement_DAO> _valuationImprovements;
        private ValuationCottage_DAO _valuationCottage;

        //[BelongsTo("ValuationClassificationKey", NotNull = false)]
        //public virtual ValuationClassification_DAO ValuationClassification
        //{
        //    get
        //    {
        //        return _valuationClassification;
        //    }
        //    set
        //    {
        //        _valuationClassification = value;
        //    }
        //}

        //[Property("ValuationEscalationPercentage", NotNull = false)]
        //public virtual double? ValuationEscalationPercentage
        //{
        //    get { return _valuationEscalationPercentage; }
        //    set { _valuationEscalationPercentage = value; }
        //}

        /// <summary>
        /// A SAHL Manual Valuation can only have one Main Building.
        /// </summary>
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ValuationMainBuilding_DAO ValuationMainBuilding
        {
            get { return _valuationMainBuilding; }
            set { _valuationMainBuilding = value; }
        }

        /// <summary>
        /// A SAHL Manual Valuation can have many outbuildings.
        /// </summary>
        [HasMany(typeof(ValuationOutbuilding_DAO), ColumnKey = "ValuationKey", Table = "ValuationOutbuilding", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ValuationOutbuilding_DAO> ValuationOutBuildings
        {
            get { return _valuationOutBuildings; }
            set { _valuationOutBuildings = value; }
        }

        /// <summary>
        /// A SAHL Manual Valuation can only have one Total Combined Thatch Value.
        /// </summary>
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ValuationCombinedThatch_DAO ValuationCombinedThatch
        {
            get { return _valuationCombinedThatch; }
            set { _valuationCombinedThatch = value; }
        }

        /// <summary>
        /// A SAHL Manual Valuation can have many improvements associated to it. e.g. Tennis Court, Walls or a Pool.
        /// </summary>
        [HasMany(typeof(ValuationImprovement_DAO), ColumnKey = "ValuationKey", Table = "ValuationImprovement", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ValuationImprovement_DAO> ValuationImprovements
        {
            get { return _valuationImprovements; }
            set { _valuationImprovements = value; }
        }

        /// <summary>
        /// A SAHL Manual Valuation can only have one Cottage.
        /// </summary>
        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ValuationCottage_DAO ValuationCottage
        {
            get { return _valuationCottage; }
            set { _valuationCottage = value; }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLManual_DAO Find(int id)
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLManual_DAO>.Find(id).As<ValuationDiscriminatedSAHLManual_DAO>();
        }

        public new static ValuationDiscriminatedSAHLManual_DAO Find(object id)
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLManual_DAO>.Find(id).As<ValuationDiscriminatedSAHLManual_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLManual_DAO FindFirst()
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLManual_DAO>.FindFirst().As<ValuationDiscriminatedSAHLManual_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLManual_DAO FindOne()
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLManual_DAO>.FindOne().As<ValuationDiscriminatedSAHLManual_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ValuationDiscriminatedSAHLManual_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<ValuationDiscriminatedSAHLManual_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}