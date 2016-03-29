using SAHL.Core.Data;
using SAHL.DomainProcessManager.Models;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements
{
    public class DoesOpenAppExistForComcorpPropertyAndClientStatement : ISqlStatement<int>
    {
        public string ClientIdNumber { get; protected set; }

        private ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail;

        public string ComplexName { get { return comcorpApplicationPropertyDetail.ComplexName; } }

        public string StreetNo { get { return comcorpApplicationPropertyDetail.StreetNo; } }

        public string StreetName { get { return comcorpApplicationPropertyDetail.StreetName; } }

        public string Suburb { get { return comcorpApplicationPropertyDetail.Suburb; } }

        public string City { get { return comcorpApplicationPropertyDetail.City; } }

        public string Province { get { return comcorpApplicationPropertyDetail.Province; } }

        public string PostalCode { get { return comcorpApplicationPropertyDetail.PostalCode; } }

        public DoesOpenAppExistForComcorpPropertyAndClientStatement(string clientIdNumber, ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail)
        {
            this.ClientIdNumber = clientIdNumber;
            this.comcorpApplicationPropertyDetail = comcorpApplicationPropertyDetail;
        }

        public string GetStatement()
        {
            return @"SELECT COUNT(1)
                    FROM [2AM].dbo.[LegalEntity] le
                        JOIN [2AM].dbo.[OfferRole] ofr ON le.LegalEntityKey = ofr.LegalEntityKey
                            and ofr.generalStatusKey = 1
                        JOIN [2AM].dbo.[Offer] o ON ofr.OfferKey = o.OfferKey
                        JOIN [2AM].dbo.[ComcorpOfferPropertyDetails] prop ON o.OfferKey = prop.OfferKey
                        LEFT JOIN [2AM].dbo.[StageTransitionComposite] stc on ofr.offerKey = stc.GenericKey
                            and stc.StageDefinitionStageDefinitionGroupKey in (110,111)
                        WHERE le.IDNumber = @ClientIdNumber
                        AND o.OfferStatusKey IN (1,4,5)
                        AND stc.GenericKey IS NULL
                        AND prop.ComplexName = @ComplexName
                        AND prop.StreetNo = @StreetNo
                        AND prop.StreetName = @StreetName
                        AND prop.Suburb = @Suburb
                        AND prop.City = @City
                        AND prop.Province = @Province
                        AND prop.PostalCode = @PostalCode";
        }
    }
}