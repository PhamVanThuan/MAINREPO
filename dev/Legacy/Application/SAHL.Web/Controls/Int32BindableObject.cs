using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SAHL.Web.Controls
{
    public class Int32BindableObject
    {
        private int _key;
        private object _object;

        public Int32BindableObject(int key, object val)
        {
            _key = key;
            _object = val;
        }

        public int Key
        {
            get
            {
                return _key;
            }
        }

        public object Object
        {
            get
            {
                return _object;
            }
        }
    }
}
