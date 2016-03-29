using System;
using System.Collections.Generic;
using System.Text;

namespace DomainService
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        string _Name = string.Empty;
        int _Port = 0;
        Type _I;
        Type _C;
        public ServiceAttribute(string Name, int Port, Type Interface, Type Concrete)
        {
            _Name = Name;
            _Port = Convert.ToInt32(Port);
            _I = Interface;
            _C = Concrete;
        }

        public string GetURL(string IP)
        {
            return string.Format("tcp://{0}:{1}/{2}", IP, _Port, _Name);
        }

        public Type Interface { get { return _I; } }
        public Type Concrete { get { return _C; } }

        public int Port
        {
            get { return _Port; }
        }

        public string Name
        {
            get { return _Name; }
        }
    }
}
