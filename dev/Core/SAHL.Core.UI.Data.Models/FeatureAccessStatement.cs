using SAHL.Core.Data;
using System.Collections.Generic;

namespace SAHL.Core.UI.Data.Models
{
    public class FeatureAccessStatement : ISqlStatement<FeatureAccess>
    {
        public FeatureAccessStatement(IEnumerable<string> groups)
        {
            this.Groups = groups;
        }

        public IEnumerable<string> Groups { get; protected set; }

        public string GetStatement()
        {
            return @"SELECT distinct ShortName as FeatureName "
                    + "FROM FeatureGroup fg "
                    + "JOIN Feature f ON f.FeatureKey = fg.FeatureKey "
                    + "where ADUserGroup in @Groups";
        }
    }
}