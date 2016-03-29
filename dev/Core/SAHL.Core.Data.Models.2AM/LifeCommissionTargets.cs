using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeCommissionTargetsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifeCommissionTargetsDataModel(string consultant, int? effectiveYear, int? effectiveMonth, int? targetPolicies, int? minPoliciesToQualify)
        {
            this.Consultant = consultant;
            this.EffectiveYear = effectiveYear;
            this.EffectiveMonth = effectiveMonth;
            this.TargetPolicies = targetPolicies;
            this.MinPoliciesToQualify = minPoliciesToQualify;
		
        }
		[JsonConstructor]
        public LifeCommissionTargetsDataModel(int targetKey, string consultant, int? effectiveYear, int? effectiveMonth, int? targetPolicies, int? minPoliciesToQualify)
        {
            this.TargetKey = targetKey;
            this.Consultant = consultant;
            this.EffectiveYear = effectiveYear;
            this.EffectiveMonth = effectiveMonth;
            this.TargetPolicies = targetPolicies;
            this.MinPoliciesToQualify = minPoliciesToQualify;
		
        }		

        public int TargetKey { get; set; }

        public string Consultant { get; set; }

        public int? EffectiveYear { get; set; }

        public int? EffectiveMonth { get; set; }

        public int? TargetPolicies { get; set; }

        public int? MinPoliciesToQualify { get; set; }

        public void SetKey(int key)
        {
            this.TargetKey =  key;
        }
    }
}