using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.ThirdParty;
using SAHL.Services.Query.Models.Treasury;

namespace SAHL.Services.Query.Models.Account
{
  public  class AccountDataModel : IQueryDataModel
    {
      public AccountDataModel()
        {
            SetupRelationships();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        public int Id { get; set; }
        public int? AccountStatusKey { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal? FixedPayment { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? ParentAccountKey { get; set; }
        public int? SPVKey { get; set; }
      
        private void SetupRelationships()
        {
            
            Relationships = new List<IRelationshipDefinition>();
           
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "SPV",
                DataModelType = typeof(SPVDataModel),
                RelationshipType = RelationshipType.OneToOne,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "SPVKey", RelatedKey = "SPVKey", Value = ""}
                    }
            });


        }
    

    }
}
