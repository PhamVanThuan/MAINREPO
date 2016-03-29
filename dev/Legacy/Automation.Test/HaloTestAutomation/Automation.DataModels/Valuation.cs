using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class Valuation : IComparable<Valuation>, IDataModel
    {
        public int OfferKey { get; set; }

        public int AccountKey { get; set; }

        public int ValuationKey { get; set; }

        public int ValuatorKey { get; set; }

        public DateTime? ValuationDate { get; set; }

        public double ValuationAmount { get; set; }

        public double ValuationHOCValue { get; set; }

        public double ValuationMunicipal { get; set; }

        public string ValuationUserID { get; set; }

        public int PropertyKey { get; set; }

        public double HOCThatchAmount { get; set; }

        public double HOCConventionalAmount { get; set; }

        public double HOCShingleAmount { get; set; }

        public DateTime ChangeDate { get; set; }

        public int? ValuationClassificationKey { get; set; }

        public double ValuationEscalationPercentage { get; set; }

        public ValuationStatusEnum ValuationStatusKey { get; set; }

        public string Data { get; set; }

        public int ValuationDataProviderDataServiceKey { get; set; }

        public bool IsActive { get; set; }

        public HOCRoofEnum HOCRoofKey { get; set; }

        public string HOCRoofDescription { get; set; }

        public string ValuationClassificationDescription { get; set; }

        #region Other

        public ValuationCottage ValuationCottage { get; set; }

        public ValuationCombinedThatch ValuationCombinedThatch { get; set; }

        public ValuationMainBuilding ValuationMainBuilding { get; set; }

        public ValuationOutbuilding ValuationOutbuilding { get; set; }

        public ValuationImprovement ValuationImprovement { get; set; }

        public Valuer Valuator { get; set; }

        #endregion Other

        public int CompareTo(Valuation other)
        {
            if (!this.IsActive.Equals(other.IsActive))
                return 0;
            return 1;
        }
    }
}