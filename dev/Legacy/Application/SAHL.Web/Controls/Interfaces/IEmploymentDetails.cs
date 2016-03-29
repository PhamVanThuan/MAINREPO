using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Controls.Interfaces
{
    /// <summary>
    /// Interface for the <see cref="EmploymentDetails"/> control so we can easily mock the control in testing.
    /// </summary>
    public interface IEmploymentDetails
    {
        /// <summary>
        /// Gets/sets the name of the person in PreCredit who confirmed the employment details.
        /// </summary>
        string ConfirmedBy { get; set; }

        /// <summary>
        /// Gets/sets the date that the income was confirmed.
        /// </summary>
        DateTime? ConfirmedDate { get; set; }

        /// <summary>
        /// Gets/sets the confirmed income amount displayed.
        /// </summary>
        double? ConfirmedBasicIncome { get; set; }

        /// <summary>
        /// Gets/sets whether the Confirmed Income field is enabled.
        /// </summary>
        bool ConfirmedBasicIncomeEnabled { get; set; }

        /// <summary>
        /// Gets/sets whether the Confirmed Income field is raed only.  If true, a label will be displayed instead of a text box.
        /// </summary>
        bool ConfirmedBasicIncomeReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the contact person displayed.
        /// </summary>
        //string ContactPerson { get; set; }

        /// <summary>
        /// Gets/sets whether the Contact Person field can be edited.
        /// </summary>
        //bool ContactPersonReadOnly { get; set; }


        /// <summary>
        /// Gets/sets whether the Contact Code field can be edited.
        /// </summary>
        //string ContactPhoneCode { get; set; }

        /// <summary>
        /// Gets/sets whether the Contact Number value.
        /// </summary>
        //string ContactPhoneNumber { get; set; }

        /// <summary>
        /// Gets/sets whether the Contact Number field can be edited.
        /// </summary>
        //bool ContactPhoneNumberReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the value in the Department field.
        /// </summary>
        //string Department { get; set; }

        /// <summary>
        /// Gets/sets whether the Department field can be edited.
        /// </summary>
        //bool DepartmentReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the employment status key selected on the control.
        /// </summary>
        int? EmploymentStatusKey { get; }

        /// <summary>
        /// Gets/sets the employment status.
        /// </summary>
        IEmploymentStatus EmploymentStatus { get; set; }

        /// <summary>
        /// Gets/sets whether the Status field can be edited.
        /// </summary>
        bool EmploymentStatusReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the employment type key selected on the control.
        /// </summary>
        int? EmploymentTypeKey { get; }

        /// <summary>
        /// Gets/sets the employment type.
        /// </summary>
        IEmploymentType EmploymentType { get; set; }

        /// <summary>
        /// Gets/sets whether the Employment Type field can be edited.
        /// </summary>
        bool EmploymentTypeReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the value in the End Date field.
        /// </summary>
        DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets/sets whether the End Date field can be edited.
        /// </summary>
        bool EndDateReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the legal entity applicable to the control.  This affects the remuneration types displayed 
        /// in the Remuneration Type drop down box.  Leaving this as null will result in all possible 
        /// remuneration types being shown.
        /// </summary>
        ILegalEntity LegalEntity { get; set; }

        /// <summary>
        /// Gets/sets the value in the Monthly Income field.
        /// </summary>
        double? BasicIncome { get; set; }

        /// <summary>
        /// Gets/sets whether the Monthly Income field is enabled.
        /// </summary>
        bool BasicIncomeEnabled { get; set; }

        /// <summary>
        /// Gets/sets whether the Monthly Income field is read only.  If true, a label will be displayed instead of a textbox.
        /// </summary>
        bool BasicIncomeReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the remuneration type.
        /// </summary>
        IRemunerationType RemunerationType { get; set; }

        /// <summary>
        /// Gets/sets whether the Remuneration Type field can be edited.
        /// </summary>
        bool RemunerationTypeReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the value in the Start Date field.
        /// </summary>
        DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets/sets whether the Start Date field can be edited.
        /// </summary>
        bool StartDateReadOnly { get; set; }

        /// <summary>
        /// Gets/sets the visibility of the control.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool? ConfirmedEmployment { get;set;}

        /// <summary>
        /// 
        /// </summary>
        bool? ConfirmedIncome { get;set;}

        /// <summary>
        /// 
        /// </summary>
        bool ConfirmedEmploymentReadOnly { get;set;}

        /// <summary>
        /// 
        /// </summary>
        bool ConfirmedIncomeReadOnly { get;set;}

        /*
        event KeyChangedEventHandler ConfirmedIncomeSelectedIndexChanged;
        event KeyChangedEventHandler ConfirmedEmploymentSelectedIndexChanged;
        event KeyChangedEventHandler EmploymentTypeSelectedIndexChanged;
        event KeyChangedEventHandler RemunerationTypeSelectedIndexChanged;
        */
        void BindDropDownListControls();

        //EventHandler ConfirmedIncomeSelectedIndexChanged { set;}
        //EventHandler ConfirmedEmploymentSelectedIndexChanged { set;}
        //EventHandler EmploymentTypeSelectedIndexChanged { set;}
        //EventHandler RemunerationTypeSelectedIndexChanged { set;}
    }
}
