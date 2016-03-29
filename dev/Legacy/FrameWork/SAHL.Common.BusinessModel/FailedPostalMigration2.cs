using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class FailedPostalMigration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO>, IFailedPostalMigration
	{
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnFailedLegalEntityAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnFailedLegalEntityAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnFailedLegalEntityAddresses_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnFailedLegalEntityAddresses_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// Gets a formatted description of the address.
        /// </summary>
        /// <param name="delimiter">The delimiter to use to split up the address components.</param>
        /// <returns>The address as a readable string.</returns>
        /// <seealso cref="GetFormattedDescription(AddressDelimiters)"/>
        public virtual string GetFormattedDescription(AddressDelimiters delimiter)
        {
            List<string> lines = new List<string>();
            lines.Add(this.ClientBoxNumber);
            lines.Add(this.ClientBoxNumber2);
            // lines.Add(this.ClientNumber);
            lines.Add(this.ClientPostalCode);
            lines.Add(this.ClientPostOffice);
            lines.Add(this.NewAdd3);
            lines.Add(this.NewCity);
            lines.Add(this.NewProvince);
            lines.Add(this.NewCountry);

            StringBuilder sb = new StringBuilder();

            string delim = "";
            // Set up the Delimiter
            if (delimiter == AddressDelimiters.CarriageReturn)
                delim = Environment.NewLine;
            else if (delimiter == AddressDelimiters.Comma)
                delim = ", ";
            else if (delimiter == AddressDelimiters.HtmlLineBreak)
                delim = "<br />";
            else
                delim = " ";

            // Build the Formatted Address string
            foreach (string line in lines)
            {
                if (line != null && line.Trim().Length > 0)
                {
                    if (sb.Length > 0) sb.Append(delim);
                    sb.Append(line.Trim());
                }
            }

            // Format the address into sentence case (ie: each new work starts with a capital)
            return new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(sb.ToString().ToLower());

        }
	}
}


