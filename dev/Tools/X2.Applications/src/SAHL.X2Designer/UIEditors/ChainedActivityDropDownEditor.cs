using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.UiEditors
{
    public class LinkedActivityDropDownEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        public static bool UserHasClicked = false;
        IWindowsFormsEditorService edSvc;
        public static UserActivity mActivity;

        public LinkedActivityDropDownEditor()
        {
            mListBox.BorderStyle = BorderStyle.None;
            mListBox.Click += new EventHandler(mListBox_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            UserHasClicked = false;
            mListBox.Items.Clear();
            mListBox.Items.Add(" ");
            BaseState mBaseState = null;

            if (mActivity == null)
            {
                return null;
            }
            foreach (CustomLink l in mActivity.Links)
            {
                if (l.ToNode is BaseState)
                {
                    mBaseState = l.ToNode as BaseState;
                    break;
                }
            }
            if (mBaseState != null)
            {
                foreach (CustomLink l in mBaseState.Links)
                {
                    if (l.ToNode.GetType().BaseType == typeof(BaseActivity) && l.ToNode != mActivity)
                    {
                        BaseActivity ba = l.ToNode as BaseActivity;
                        mListBox.Items.Add(ba.Name);
                    }
                }
            }

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities[x].Name == mListBox.SelectedItem.ToString())
                        {
                            return MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities[x];
                        }
                    }
                }
                return null;
            }
            return value;
        }

        private void mListBox_Click(object sender, EventArgs e)
        {
            UserHasClicked = true;
            edSvc.CloseDropDown();
        }
    }

    public class LinkedActivityConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(UserActivity))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is UserActivity)
            {
                UserActivity mItem = value as UserActivity;
                return mItem.Name.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}