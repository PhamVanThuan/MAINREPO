using System;
using System.Collections.Generic;
using System.Text;

namespace AspectAttribute
{
    public class PerfObj
    {
        List<string> _InnerMethod = new List<string>();
        string _MethodName = string.Empty;
        string _ClassName = string.Empty;
        long TicksStart;
        int _HashCode;
        double _TotalSecondsTaken = 0;

        public string MethodName { get { return _MethodName; } }

        public string ClassName { get { return _ClassName; } }

        public int HashCode { get { return _HashCode; } }

        public PerfObj(string s, int i, string c)
        {
            this._HashCode = i;
            this._MethodName = s;
            this._ClassName = c;
        }

        public bool IsInitialMethod(string s, int i)
        {
            if ((MethodName == s) && (HashCode == i))
                return true;
            return false;
        }

        public void Start()
        {
            TicksStart = DateTime.Now.Ticks;
        }

        public void AddMethodCall(string Method, double Sec)
        {
            if ((Method != ".ctor"))
            {
                _InnerMethod.Add(string.Format("{0}[{1}]", Method, Sec));
            }
        }

        public void Complete()
        {
            long TotalTicks = DateTime.Now.Ticks - TicksStart;
            TimeSpan ts = new TimeSpan(TotalTicks);
            _TotalSecondsTaken = ts.TotalSeconds;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(InnerMethod);
            sb.AppendFormat("{1}[{0}]", _TotalSecondsTaken, MethodName);
            return sb.ToString();
        }

        public string InnerMethod
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in _InnerMethod)
                {
                    sb.AppendFormat("{0},", s);
                }
                return sb.ToString();
            }
        }

        public double TotalSeconds
        {
            get
            {
                return _TotalSecondsTaken;
            }
        }
    }
}