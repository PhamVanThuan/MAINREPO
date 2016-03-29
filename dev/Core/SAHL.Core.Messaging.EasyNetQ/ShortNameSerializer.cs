using EasyNetQ;
using SAHL.Core.Strings;
using System;
using System.Text.RegularExpressions;

namespace SAHL.Core.Messaging.EasyNetQ
{
    public class ShortNameSerializer : ITypeNameSerializer
    {
        //this is to remove the versioning of the type when subscribing
        //[\s]{0,} == space
        //Version=\d*\.\d*\.\d*\.\d* == the version of the item.
        Regex regex = new Regex(@"(,[\s]{0,}Version=\d*\.\d*\.\d*\.\d*[\s]{0,},[\s]{0,}Culture=neutral,[\s]{0,}PublicKeyToken=null)");

        public Type DeSerialize(string typeName)
        {
            var nameParts = typeName.Split(':');
            var type = Type.GetType(string.Format("{0}, {1}", nameParts[0].Lengthen(), nameParts[1].Lengthen()));
            
            return type;
        }

        public string Serialize(Type type)
        {
            var fullTypeName = regex.Replace(type.FullName, "");
            var typeName = fullTypeName.Shorten() + ":" + type.Assembly.GetName().Name.Shorten();
            if (typeName.Length > 255)
            {
                throw new SystemException(string.Format("The serialized name of type '{0}' exceeds the AMQP maximum short string lengh of 255 characters.", type.Name));
            }
            return typeName;
        }
    }
}