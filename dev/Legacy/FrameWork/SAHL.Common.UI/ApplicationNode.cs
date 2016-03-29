using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.UI.Walkthroughs;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Common.UI
{
    public class ApplicationNode : CBOMenuNode, IWalkthroughProvider
    {
        IList<IWalkthroughItem> _items = new List<IWalkthroughItem>();

        public ApplicationNode(ICBOMenu CBOMenu, int GenericKey, CBOMenuNode Parent, string Description, string LongDescription)
            : base(CBOMenu, GenericKey, Parent, Description, LongDescription)
        {
            _items.Add(new WalkthroughItem("Calculator", "Calculator", "Origination_Calculator", "CALC", "CALC_HOT", "CALC_DIS", true, true));
            _items.Add(new WalkthroughItem("Client Details", "Client Details", "Applicants", "CLIENT", "CLIENT_HOT", "CLIENT_DIS", true, true));
            _items.Add(new WalkthroughItem("Property Details", "Property Details", "Properties", "PROP", "PROP_HOT", "PROP_DIS", true, true));
            _items.Add(new WalkthroughItem("Loan Details", "Loan Details", "Origination_Calculator", "LOAN", "LOAN_HOT", "LOAN_DIS", true, true));
            _items.Add(new WalkthroughItem("Save", "Save Application", "", "SAVE", "SAVE_HOT", "SAVE_DIS", true, true));

            //base._cboUniqueKey = "AN_" + GenericKey.ToString();
        }

        public ApplicationNode(Dictionary<string, object> NodeData)
            : base(NodeData)
        {
            _items.Add(new WalkthroughItem("Calculator", "Calculator", "Origination_Calculator", "CALC", "CALC_HOT", "CALC_DIS", true, true));
            _items.Add(new WalkthroughItem("Client Details", "Client Details", "Applicants", "CLIENT", "CLIENT_HOT", "CLIENT_DIS", true, true));
            _items.Add(new WalkthroughItem("Property Details", "Property Details", "Properties", "PROP", "PROP_HOT", "PROP_DIS", true, true));
            _items.Add(new WalkthroughItem("Loan Details", "Loan Details", "Origination_Calculator", "LOAN", "LOAN_HOT", "LOAN_DIS", true, true));
            _items.Add(new WalkthroughItem("Save", "Save Application", "", "SAVE", "SAVE_HOT", "SAVE_DIS", true, true));

            //base._cboUniqueKey = "AN_" + GenericKey.ToString();
        }

        #region IWalkthroughProvider Members

        public bool ApplyToChildren
        {
            get { return true; }
        }

        public bool ApplyToParent
        {
            get { return true; }
        }

        public string ItemClicked(IWalkthroughItem Item)
        {
            return "";
        }

        public IList<IWalkthroughItem> Items
        {
            get { return _items; }
        }

        #endregion
    }
}
