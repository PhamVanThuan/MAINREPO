using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Managers.Employment.Statements
{
    public class GetEmployerStatement : ISqlStatement<EmployerDataModel>
    {
        private EmployerModel EmployerModel;
        public int EmployerBusinessTypeKey { get; protected set; }
        public string EmployerName { get; protected set; }
        public int EmploymentSector { get; protected set; }

        public GetEmployerStatement(EmployerModel employerModel)
        {
            this.EmployerModel = employerModel;
            this.EmployerBusinessTypeKey = (int)employerModel.EmployerBusinessType;
            this.EmployerName = employerModel.EmployerName;
            this.EmploymentSector = (int)employerModel.EmploymentSector;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                                [EmployerKey]
                                  ,[Name]
                                  ,[TelephoneNumber]
                                  ,[TelephoneCode]
                                  ,[ContactPerson]
                                  ,[ContactEmail]
                                  ,[AccountantName]
                                  ,[AccountantContactPerson]
                                  ,[AccountantTelephoneCode]
                                  ,[AccountantTelephoneNumber]
                                  ,[AccountantEmail]
                                  ,[EmployerBusinessTypeKey]
                                  ,[UserID]
                                  ,[ChangeDate]
                                  ,[EmploymentSectorKey]
                            FROM
                                Employer
                            WHERE
                                [Name] = @EmployerName";
            return query;
        }
    }
}
