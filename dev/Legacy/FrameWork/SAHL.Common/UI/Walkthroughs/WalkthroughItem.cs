using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.UI.Walkthroughs
{
    public class WalkthroughItem : IWalkthroughItem
    {
        string _caption = "";
        string _title = "";
        string _url = "";
        string _image = "";
        string _hotImage = "";
        string _disabledImage = "";
        bool _visible;
        bool _enabled;

        public WalkthroughItem(string Caption, string Title, string Url, string Image, string HotImage, string DisabledImage, bool Visible, bool Enabled)
        {
            _caption = Caption;
            _title = Title;
            _image = Image;
            _hotImage = HotImage;
            _disabledImage = DisabledImage;
            _visible = Visible;
            _enabled = Enabled;
        }

        #region IWalkthroughItem Members

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public string HotImage
        {
            get
            {
                return _hotImage;
            }
            set
            {
                _hotImage = value;
            }
        }

        public string DisabledImage
        {
            get
            {
                return _disabledImage;
            }
            set
            {
                _disabledImage = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        #endregion
    }
}
