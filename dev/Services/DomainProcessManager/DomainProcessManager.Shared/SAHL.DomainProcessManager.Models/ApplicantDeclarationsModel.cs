using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantDeclarationsModel : IDataModel
    {
        public ApplicantDeclarationsModel(OfferDeclarationQuestionEnum declarationQuestion, OfferDeclarationAnswer declarationAnswer, DateTime? declarationDate)
        {
            this.DeclarationQuestion = declarationQuestion;
            this.DeclarationAnswer   = declarationAnswer;
            this.DeclarationDate     = declarationDate;
        }

        [DataMember]
        public OfferDeclarationQuestionEnum DeclarationQuestion { get; set; }

        [DataMember]
        public OfferDeclarationAnswer DeclarationAnswer { get; set; }

        [DataMember]
        public DateTime? DeclarationDate { get; set; }
    }
}
