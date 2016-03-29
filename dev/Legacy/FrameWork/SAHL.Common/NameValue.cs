using System;

namespace SAHL.Common
{
    /// <summary>
    /// A simple comparable namevalue class
    /// </summary>
    public class NameValue : IComparable
    {
        private string m_Name;
        private object m_Value;

        /// <summary>
        /// Constructor with default values
        /// </summary>
        public NameValue()
        {
            m_Name = "";
            m_Value = 0;
        }

        /// <summary>
        /// Constructor with initial values
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public NameValue(string Name, object Value)
        {
            m_Name = Name;
            m_Value = Value;
        }

        /// <summary>
        /// Name propery - string
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is NameValue)
            {
                NameValue nv = (NameValue)obj;

                return m_Name.CompareTo(nv.m_Name);
            }

            if (obj is string)
            {
                return m_Name.CompareTo((string)obj);
            }

            throw new ArgumentException("object is not a correct type");
        }

        #endregion IComparable Members
    }
}