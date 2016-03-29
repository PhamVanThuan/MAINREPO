using System.Windows;

namespace SAHomeloans.SAHL_VSExtensions.Controls
{
    public class VsMenu : VsMenuItem
    {
        static VsMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VsMenu), new FrameworkPropertyMetadata(typeof(VsMenu)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new VsMenuItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is VsMenuItem;
        }

        public VsMenuItem GetSelectedMenuItem()
        {
            //this will change later
            foreach (VsMenuItem group in this.Items)
            {
                foreach (VsMenuItem item in group.Items)
                {
                    if (item.IsExpanded)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}