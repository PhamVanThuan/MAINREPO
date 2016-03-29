using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration.Interfaces
{
    /// <summary>
    /// Attorney Contact Interface
    /// </summary>
    public interface IAttorneyContact : IViewBase
    {
        string AttorneyName { set; }
        #region Legal Entity Code
        /// <summary>
        /// Legal Entity Add
        /// </summary>
        event EventHandler<LegalEntityEventArgs> LegalEntityAdd;

        /// <summary>
        /// Bind External Role Types
        /// </summary>
        /// <param name="externalRoleTypes"></param>
        void BindExternalRoleTypes(IDictionary<int, string> externalRoleTypes);

        /// <summary>
        /// Clear Legal Entity Fields
        /// </summary>
        void ClearLegalEntityFields();


        #endregion

        #region Grid

        /// <summary>
        /// When the value of a check box has changed
        /// </summary>
        event KeyChangedEventHandler OnCheckedChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        void SetUplitigationAttorneyGrid(DataTable dt);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        void BindSetUplitigationAttorneyGridPostRowUpdate(DataTable dt);

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnAddToCBO;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnSelectionChanged;

        #endregion

        /// <summary>
        /// Get/Set whether the view is readonly
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        bool ReadOnly { get; set; }

        /// <summary>
        /// What roles can be updated
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Not a read only property.")]
        Dictionary<int, string> ExternalRoleTypeKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnDone;
    }

    /// <summary>
    /// Legal Entity Event Args
    /// </summary>
    public class LegalEntityEventArgs : EventArgs
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCode { get; set; }
        public string FaxNumber { get; set; }
        public string FaxCode { get; set; }
        public string EmailAddress { get; set; }
        public int RoleTypeKey { get; set; }
    }
}
