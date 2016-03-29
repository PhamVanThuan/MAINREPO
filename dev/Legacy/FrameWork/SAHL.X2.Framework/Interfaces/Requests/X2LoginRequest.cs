using System;
using SAHL.X2.Common;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2LoginRequest : X2RequestBase
    {
        X2FieldInputList m_FieldInputList;

        public X2LoginRequest(IX2MessageCollection Messages)
            : base(false, null)
        {
            m_RequestType = RequestType.LoginRequest;
        }

        public X2FieldInputList FieldInputList
        {
            get
            {
                if (m_FieldInputList == null)
                    m_FieldInputList = new X2FieldInputList();
                return m_FieldInputList;
            }
        }
    }
}