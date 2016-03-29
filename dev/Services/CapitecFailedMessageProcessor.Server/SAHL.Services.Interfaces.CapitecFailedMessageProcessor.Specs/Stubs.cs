using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor.Specs
{
	internal static class Stubs
	{
		public static SwitchLoanApplication SwitchLoanApplicationRequest()
		{
			return new SwitchLoanApplication(1, 1, DateTime.Now, new SwitchLoanDetails(1, 1, 1, 1, 1, 1, true, 240),
			new Applicant[] { Applicant() }, 1,
			new ConsultantDetails("consultant", "some branch"), null);
		}
		public static NewPurchaseApplication NewPurchaseApplicationRequest()
		{
            return new NewPurchaseApplication(1, 1, DateTime.Now, new NewPurchaseLoanDetails(1, 1, 1, 1, false, 240),
			new Applicant[] { Applicant() }, 1,
			new ConsultantDetails("consultant", "some branch"), null);

		}
		public static Applicant Applicant()
		{
			return new Applicant(new ApplicantInformation("8105295193082", "joe", "doe", 1, "0837400571", "10827312", "11239021", "email@address.com", DateTime.Now, "Mr", true),
				new ApplicantResidentialAddress("1", "1", "building", "222", "street name", "suburb", "province", "city", "4011", 1),
				new ApplicantEmploymentDetails(1, new SalariedDetails(30000)),
				new ApplicantDeclarations(true, true, true, true),
				new ApplicantITC(DateTime.Now, string.Empty, string.Empty));
		}
	}
}
