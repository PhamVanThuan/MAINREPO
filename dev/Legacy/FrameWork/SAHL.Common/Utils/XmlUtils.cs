using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SAHL.Common.Utils
{
    /// <summary>
    /// This class contains static functions for use when working with XML.
    /// </summary>
    public class XmlUtils
    {
        /// <summary>
        /// Method to validate XML against an XSD schema.
        /// </summary>
        /// <param name="xml">The XML to be validated.</param>
        /// <param name="schema">The schema to be used to validate the XML.</param>
        /// <remarks>This method has no error handling - if validation fails an exception is thrown.</remarks>
        public static void ValidateXml(string xml, string schema)
        {
            XmlReader xReaderSchema = null;
            XmlReader xReaderXml = null;

            try
            {
                // create the XmlSchemaSet class.
                XmlSchemaSet schemaSet = new XmlSchemaSet();

                // add the schema to the schema set
                xReaderSchema = XmlReader.Create(new StringReader(schema));
                schemaSet.Add(null, xReaderSchema);

                // Set the validation settings.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;

                // use an XmlReader to parse and validate the file
                xReaderXml = XmlReader.Create(new StringReader(xml), settings);
                while (xReaderXml.Read()) ;
            }
            finally
            {
                if (xReaderSchema != null) xReaderSchema.Close();
                if (xReaderXml != null) xReaderXml.Close();
            }
        }
    }
}