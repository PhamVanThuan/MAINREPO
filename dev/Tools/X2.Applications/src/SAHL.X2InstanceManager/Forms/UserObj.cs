using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SAHL.X2InstanceManager.Forms
{
    public class UserObj
    {
        public string ADUserName;
        public string ORT;
        public int ORTKey;
        public int LEKEy;
        public int BLAKey;
        public int ADUserKey;
        public UserObj(string ADUserName, string ORT, int LEKey, int ORTKey, int blaKey, int ADUserKey)
        {
            this.ADUserName = ADUserName;
            this.ORT = ORT;
            this.LEKEy = LEKey;
            this.ORTKey = ORTKey;
            this.BLAKey = blaKey;
            this.ADUserKey = ADUserKey;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ORT))
                return string.Format("{0} - {1}", ADUserName, ORT);
            else
                return ADUserName;
        }
    }
}
