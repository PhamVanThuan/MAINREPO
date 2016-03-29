using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ResponseException
    {
        public string Value;
        public X2ResponseException Exception;

        public X2ResponseException(string p_Value)
        {
            Value = p_Value;
        }

        public X2ResponseException(string p_Value, X2ResponseException p_Exception)
        {
            Value = p_Value;
            Exception = p_Exception;
        }
    }
}