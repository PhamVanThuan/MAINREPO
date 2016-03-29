using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Data;

namespace SAHL.Web.Views.Migrate.Interfaces
{
	public interface IProposal : IViewBase
    {
        int AccountKey { get; set; }
		bool HOCInclusive { get; set; }
		bool LifeInclusive { get; set; }

		decimal HOCInstalment { get; set ;}
		decimal LifeInstalment { get; set; }

		decimal TotalInstalmentAmount { get; set; }
		IDictionary<int, string> ProposalStatusses { get; set; }
		IDictionary<int, string> InclusiveChoices { get; set; }
		IDictionary<int, string> YesNoChoices { get; set; }
		IDictionary<int, string> MarketRates { get; set; }
		DataTable ApprovalUsers { get; set; }

		void BindProposal(IMigrationDebtCounsellingProposal proposal);
		void GetProposalFromView(IMigrationDebtCounsellingProposal proposal);
		void GetProposalItemFromView(IMigrationDebtCounsellingProposalItem proposalItem);

		void ResetInputFields();

		bool ValidateProposal();
		bool ValidateProposalItem();
        bool SaveProposal { get; set; }

		event EventHandler<EventArgs> BackClick;
		event EventHandler<EventArgs> AddClick;
		event EventHandler<KeyChangedEventArgs> RemoveClick;
		event EventHandler<EventArgs> FinishClick;

		event EventHandler<EventArgs> HOCOrLifeChanged;
    }
}
