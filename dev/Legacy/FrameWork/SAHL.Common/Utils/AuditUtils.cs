using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Castle.ActiveRecord;
using System.Collections;
using System.IO;
using Castle.ActiveRecord.Framework.Internal;

namespace SAHL.Common.Utils
{
    /// <summary>
    /// Class containing utility functions for SAHL auditing.
    /// </summary>
    //public class AuditUtils
    //{
    //    private AuditUtils()
    //    {
    //    }

    //    /// <summary>
    //    /// Converts a DAO dictionary adapter into an XML string (all properties are represented in the 
    //    /// XML).
    //    /// </summary>
    //    /// <param name="adapter">The entity as a dictionary object, containing all values including collections.</param>
    //    /// <returns>An xml string representing all entity values.</returns>
    //    /// <remarks>The XML returned is guaranteed to be valid XML, as a XmlTextWriter object is used to build the string.</remarks>
    //    public static string ConvertDaoAdapterToXml(IDictionary adapter)
    //    {
    //        MemoryStream ms = new MemoryStream(1024);
    //        XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);

    //        foreach (object key in adapter.Keys)
    //        {
    //            if (adapter[key] != null)
    //            {
    //                GetPropertyString(adapter[key], key, writer);
    //            }

    //        }
    //        writer.Flush();

    //        ms.Seek(0, SeekOrigin.Begin);
    //        StreamReader reader = new StreamReader(ms);
    //        string xml = reader.ReadToEnd();

    //        // clean up as much as we can - this gets called a lot so don't take any chances
    //        writer.Close();
    //        reader.Dispose();
    //        reader = null;
    //        writer = null;
    //        ms = null;

    //        return xml;

    //    }

    //    /// <summary>
    //    /// Writes a property of a DAO entity to <c>writer</c> as an XML element.
    //    /// </summary>
    //    /// <param name="dataValue"></param>
    //    /// <param name="key"></param>
    //    /// <param name="writer"></param>
    //    private static void GetPropertyString(object dataValue, object key, XmlTextWriter writer)
    //    {
    //        Type val = dataValue.GetType();
    //        if (val.IsValueType || val.Name == "String")
    //        {
    //            writer.WriteElementString(key.ToString(), dataValue.ToString());
    //        }
    //        else
    //        {
    //            if (val.IsSubclassOf(typeof(Castle.ActiveRecord.ActiveRecordBase)))
    //            {
    //                ActiveRecordModel model = ActiveRecordModel.GetModel(val);

    //                PrimaryKeyModel pk = model.PrimaryKey;
    //                if (model.IsDiscriminatorSubClass || model.IsJoinedSubClass)
    //                    pk = model.Parent.PrimaryKey;

    //                object keyValue = pk.Property.GetValue(dataValue, null);
    //                writer.WriteElementString(key.ToString(), keyValue.ToString());
    //            }
    //            else
    //                if (val.GetInterface(typeof(IEnumerable).ToString()) != null)
    //                {
    //                    IEnumerable en = dataValue as IEnumerable;
    //                    writer.WriteStartElement(key.ToString()); // + Items
    //                    foreach (object o in en)
    //                    {
    //                        GetPropertyString(o, "Item", writer);
    //                    }
    //                    writer.WriteEndElement(); // - Items
    //                }
    //        }
    //    }
    //}
}
