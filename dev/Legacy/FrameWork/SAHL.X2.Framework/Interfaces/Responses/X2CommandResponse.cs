using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2CommandResponse : X2ResponseBase
    {
        public string xmlResponse = "<xml/>";

        public X2CommandResponse(string xml)
            : base(xml)
        {
        }
    }
}