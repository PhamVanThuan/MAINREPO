using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class DoesCityIdExistQuery : ISqlStatement<CityDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesCityIdExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return "SELECT Id,SAHLCityKey,CityName,ProvinceId FROM [Capitec].[geo].[City] (NOLOCK) WHERE Id = @Id";
        }
    }
}