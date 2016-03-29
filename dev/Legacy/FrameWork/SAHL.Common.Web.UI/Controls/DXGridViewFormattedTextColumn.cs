using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Web.ASPxGridView;

namespace SAHL.Common.Web.UI.Controls
{
    public class DXGridViewFormattedTextColumn : GridViewDataTextColumn
    {

        private GridFormatType _format = GridFormatType.GridString;
        private string _formatString;

        /// <summary>
        /// Gets/sets a default format to be applied to the column, for which a default format string 
        /// will be used.  The string used for formatting can be overridden using <see cref="FormatString"/>.  
        /// This defaults to <see cref="GridFormatType.GridString"/>.
        /// </summary>
        public GridFormatType Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Gets/sets a string for formatting the column.  If not explicitly set, this will return 
        /// a default string for the <see cref="GridFormatType"/>.
        /// </summary>
        public string FormatString
        {
            get
            {
                if (String.IsNullOrEmpty(_formatString))
                {
                    switch (Format)
                    {
                        case GridFormatType.GridCurrency:
                            return SAHL.Common.Constants.CurrencyFormat;
                        case GridFormatType.GridDate:
                            return SAHL.Common.Constants.DateFormat;
                        case GridFormatType.GridDateTime:
                            return SAHL.Common.Constants.DateTimeFormat;
                        case GridFormatType.GridNumber:
                            return SAHL.Common.Constants.NumberFormat;
                        case GridFormatType.GridRate:
                            return SAHL.Common.Constants.RateFormat;
                        case GridFormatType.GridRate3Decimal:
                            return SAHL.Common.Constants.RateFormat3Decimal;
                        default:
                            return String.Empty;
                    }
                }
                return _formatString;
            }
            set { _formatString = value; }
        }



    }

}
