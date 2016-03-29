using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SAHL.Common.BusinessModel.DataTransferObjects;

namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    /// <summary>
    /// This class is used in the SPVService.GetValidSPV() method.
    /// The method takes an XML string that is built up by serializing this class to XML.
    /// </summary>
    [Serializable]
    public class Row : IRow
    {
        [XmlElement]
        public decimal LTV { get; set; }

        [XmlElement]
        public int HasBeenInCompany2 { get; set; }

        [XmlElement]
        public int FLAllowed { get; set; }

        [XmlElement]
        public int TermChangeAllowed { get; set; }

		[XmlElement]
		public string OfferAttributes { get; set; }

        [XmlElement]
        public decimal LoanAmount { get; set; }

        [XmlElement]
        public int IsGEPF { get; set; }
    }
}