using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class ApplicantDeclarations
    {

        public ApplicantDeclarations(Guid incomeContributor, Guid allowCreditBureauCheck, Guid hasCapitecTransactionAccount, Guid marriedInCommunityOfProperty)
        {
            this.IncomeContributor = incomeContributor;
            this.AllowCreditBureauCheck = allowCreditBureauCheck;
            this.HasCapitecTransactionAccount = hasCapitecTransactionAccount;
            this.MarriedInCommunityOfProperty = marriedInCommunityOfProperty;
            this.DeclarationsDate = DateTime.Now;
        }

        public DateTime DeclarationsDate { get; set; }

        [Required(ErrorMessage = "Please indicate if the client is an income contributor")]
        public Guid IncomeContributor { get; protected set; }

        [Required(ErrorMessage = "Please indicate if the client will allow a credit bureau check to be conducted on them")]
        public Guid AllowCreditBureauCheck { get; protected set; }

        [Required(ErrorMessage = "Please indicate if the client has a Capitec transactional account")]
        public Guid HasCapitecTransactionAccount { get; protected set; }

        [Required(ErrorMessage = "Please indicate if the client is married in community of property")]
        public Guid MarriedInCommunityOfProperty { get; protected set; }
    }
}
