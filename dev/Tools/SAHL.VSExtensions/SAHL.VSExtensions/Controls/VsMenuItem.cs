using SAHL.VSExtensions.Interfaces.Configuration;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SAHomeloans.SAHL_VSExtensions.Controls
{
    [TemplatePart(Name = "btn_Name", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "ctrl_Content", Type = typeof(ItemsPresenter))]
    public class VsMenuItem : ItemsControl
    {
        private ToggleButton _button;
        private ItemsPresenter _ctrl_Content;

        public static readonly RoutedEvent ItemClickEvent;
        public static readonly RoutedEvent GroupClickEvent;

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded",
            typeof(Boolean),
            typeof(VsMenuItem),
            new PropertyMetadata(false));

        public bool IsExpanded
        {
            get
            {
                return (bool)GetValue(IsExpandedProperty);
            }
            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(String),
            typeof(VsMenuItem),
            new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        [Category("Behavior")]
        public event RoutedEventHandler ItemClick
        {
            add
            {
                base.AddHandler(VsMenuItem.ItemClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(VsMenuItem.ItemClickEvent, value);
            }
        }

        [Category("Behavior")]
        public event RoutedEventHandler GroupClick
        {
            add
            {
                base.AddHandler(VsMenuItem.GroupClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(VsMenuItem.GroupClickEvent, value);
            }
        }

        public IMenuItem MenuItem
        {
            get;
            set;
        }

        public static readonly DependencyProperty IsGroupProperty = DependencyProperty.Register("IsGroup",
            typeof(Boolean),
            typeof(VsMenuItem),
            new PropertyMetadata(false));

        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        static VsMenuItem()
        {
            VsMenuItem.ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VsMenuItem));
            VsMenuItem.GroupClickEvent = EventManager.RegisterRoutedEvent("GroupClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VsMenuItem));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VsMenuItem), new FrameworkPropertyMetadata(typeof(VsMenuItem)));
        }

        public override void OnApplyTemplate()
        {
            IsGroup = this.Items.Count > 0;
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                ToggleButton button = this.Template.FindName("btn_Name", this) as ToggleButton;
                if (_button != button)
                {
                    if (_button != null)
                    {
                        _button.Click -= Item_Clicked;
                    }
                    _button = button;
                    if (_button != null)
                    {
                        _button.Click += Item_Clicked;
                    }
                }
                _ctrl_Content = this.Template.FindName("ctrl_Content", this) as ItemsPresenter;
            }
        }

        public VsMenuItem this[int index]
        {
            get
            {
                return (VsMenuItem)this.Items[index];
            }
        }

        public void Item_Clicked(object sender, EventArgs e)
        {
            if (this.Items.Count > 0)
            {
                RaiseEvent(new RoutedEventArgs(VsMenuItem.GroupClickEvent, this));
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(VsMenuItem.ItemClickEvent, this));
            }
        }
    }
}