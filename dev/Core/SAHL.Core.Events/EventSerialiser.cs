using MoreLinq;
using SAHL.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SAHL.Core.Events
{
    public class EventSerialiser : IEventSerialiser
    {
        public const string W3CDateTimeFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";

        public string Serialise(IEvent eventToSerialise)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms, Encoding.UTF8, 1024, true))
                {
                    using (XmlWriter xw = new XmlTextWriter(sw))
                    {
                        xw.WriteStartElement("Event");
                        xw.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

                        WriteElement(xw, eventToSerialise);

                        xw.WriteEndElement();

                        xw.Flush();

                        ms.Seek(0, SeekOrigin.Begin);
                        using (var sr = new StreamReader(ms))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void WriteElement(XmlWriter xw, object input)
        {
            Type evType = input.GetType();
            foreach (var property in evType.GetProperties())
            {
                var value = property.GetValue(input);
                if (value is IEnumerable && (property.PropertyType.IsGenericType || property.PropertyType.IsArray))
                {
                    xw.WriteStartElement(property.Name);
                    foreach (var item in value as IEnumerable)
                    {
                        WriteElementItem(xw, item, "item");
                    }
                    xw.WriteEndElement();
                }
                else
                {
                    WriteElementItem(xw, value, property.Name);
                }
            }
        }

        private void WriteElementItem(XmlWriter xw, object value, string name)
        {
            
            if (IsPrimitive(value))
            {
                xw.WriteElementString(name, (value ?? string.Empty).ToString());
            }
            else if (IsDateTime(value))
            {
                xw.WriteElementString(name, ((DateTime)value).ToString(W3CDateTimeFormatString));
            }
            else
            {
                xw.WriteStartElement(name);
                WriteElement(xw, value);
                xw.WriteEndElement();
            }
        }

        private bool IsDateTime(object item)
        {
            return item != null &&
                item.GetType() == typeof(DateTime);
        }

        private bool IsPrimitive(object item)
        {
            return item == null ||
                item.GetType().IsPrimitive ||
                item.GetType() == typeof(Decimal) ||
                item.GetType() == typeof(String) ||
                
                item.GetType().IsEnum ||
                item.GetType() == typeof(Guid);
        }

        public T Deserialise<T>(string eventXmlData) where T : IEvent
        {
            return (T)this.Deserialise(typeof(T), eventXmlData);
        }

        public IEvent Deserialise(Type eventType, string eventXmlData)
        {
            return (IEvent)DeserialiseObject(eventType, eventXmlData);
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            return type.GetConstructors().MaxBy(x => x.GetParameters().Length);
        }

        public object DeserialiseObject(Type eventType, string eventXmlData)
        {
            XDocument xDoc = XDocument.Parse(eventXmlData);

            ConstructorInfo constructor = GetConstructor(eventType);

            Dictionary<string, object> deserialisedConstructorParams = new Dictionary<string, object>();

            IDictionary elements = GetElements(xDoc);
            string[] exclusions = { "Name", "ClassName" };
            foreach (ParameterInfo param in constructor.GetParameters())
            {
                string paramName = param.Name.ToLower();

                if (!(exclusions.Contains(paramName, StringComparer.OrdinalIgnoreCase)))
                {
                    var paramValue = (elements[paramName] as XElement);
                    object value = null;

                    value = TypeInstantiator.CreateFromType(param.ParameterType, paramValue, (x, y) =>
                    {
                        return this.DeserialiseObject(x, y);
                    });

                    deserialisedConstructorParams.Add(paramName, value);
                }
            }

            var originalConstructorParamCount = constructor.GetParameters().Count(x => !exclusions.Any(y => y.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)));
            if (originalConstructorParamCount != deserialisedConstructorParams.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format("Not enough properties to fulfil constructor of: {0}", eventType.Name));
            }

            return constructor.Invoke(deserialisedConstructorParams.Select(x => x.Value).ToArray());
        }

        private IDictionary GetElements(XDocument xDoc)
        {
            IDictionary xmlProps = new Dictionary<string, XElement>();
            foreach (XElement node in xDoc.Root.Nodes())
            {
                string propName = node.Name.LocalName;
                xmlProps.Add(propName.ToLower(), node);
            }
            return xmlProps;
        }

        public IServiceRequestMetadata DeserialiseMetadata(string eventXmlData)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            XDocument xDoc = XDocument.Parse(eventXmlData);
            foreach (XElement node in xDoc.Root.Nodes())
            {
                string key = node.Descendants("Key").SingleOrDefault().Value;
                string value = node.Descendants("Value").SingleOrDefault().Value;
                metadata.Add(key, value);
            }

            return new ServiceRequestMetadata(metadata);
        }

        public string SerialiseEventMetadata(IServiceRequestMetadata metadata)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms, Encoding.UTF8, 1024, true))
                {
                    using (XmlWriter xw = new XmlTextWriter(sw))
                    {
                        xw.WriteStartElement("ServiceRequestMetadata");
                        xw.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

                        foreach (var keyValue in metadata)
                        {
                            xw.WriteStartElement("item");
                            xw.WriteElementString("Key", keyValue.Key);
                            xw.WriteElementString("Value", keyValue.Value);
                            xw.WriteEndElement();
                        }

                        xw.WriteEndElement();

                        xw.Flush();

                        ms.Seek(0, SeekOrigin.Begin);
                        using (var sr = new StreamReader(ms))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public IServiceRequestMetadata DeserialiseEventMetadata(string metadataXmlData)
        {
            return this.DeserialiseMetadata(metadataXmlData);
        }
    }
}