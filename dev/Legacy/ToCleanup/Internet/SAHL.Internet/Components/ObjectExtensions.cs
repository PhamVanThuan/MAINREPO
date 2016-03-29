using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace SAHL.Internet.Components
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
    }
}
