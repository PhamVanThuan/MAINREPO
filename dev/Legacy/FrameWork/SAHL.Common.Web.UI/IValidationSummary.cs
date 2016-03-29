using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Web.UI
{
    /// <summary>
    /// Interface used for handling validation summary.
    /// </summary>
    public interface IValidationSummary
    {
        /// <summary>
        /// Gets the validation messages added to the validation summary.
        /// </summary>
        IDomainMessageCollection DomainMessages { get; }

        /// <summary>
        /// Gets/sets the error message summary displayed on the control.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Gets/sets the text to display in the header of the validation summary.
        /// </summary>
        string HeaderText { get; set; }

        /// <summary>
        /// Gets/sets whether warnings should be ignored.  
        /// </summary>
        bool IgnoreWarnings { get; set; }

        /// <summary>
        /// Used to determine if there are remaining errors/warnings in the <see cref="DomainMessages"/> 
        /// collection.  If <see cref="IgnoreWarnings"/> is true and only errors remain, this should 
        /// return tru.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets/sets the warning summary message to display on the control.
        /// </summary>
        string WarningMessage { get; set; }


    }
}
