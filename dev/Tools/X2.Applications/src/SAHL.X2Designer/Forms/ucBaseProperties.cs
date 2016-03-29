using System;
using System.Windows.Forms;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public delegate void TextChanged(string Text);

    public partial class ucBaseProperties : UserControl
    {
        public event TextChanged OnTextChanged;

        public ucBaseProperties(BaseItem item)
        {
            InitializeComponent();
            //txtName.Text = item.Name;
        }

        protected virtual void Construct()
        {
            ListViewGroup grp = new ListViewGroup("Common");
            ListViewItem lvi = new ListViewItem();
            ListViewItem.ListViewSubItem svi = new ListViewItem.ListViewSubItem();
            svi.Text = Name;
            lvi.SubItems.Add(svi);
            grp.Items.Add(lvi);
            li.Groups.Add(grp);
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            //if (null != OnTextChanged)
            //    OnTextChanged(txtName.Text);
        }

        private void ucBaseProperties_Resize(object sender, EventArgs e)
        {
        }
    }
}