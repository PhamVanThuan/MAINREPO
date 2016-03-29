using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public class BindableGenericKeyType
    {
        internal int _Key;
        public int Key { get { return _Key; } }
        internal string _Desc;
        public string Desc { get { return _Desc; } }
        public BindableGenericKeyType(IGenericKeyType gkt)
        {
            _Key = gkt.Key;
            _Desc = gkt.Description;
        }
        public BindableGenericKeyType() { }
    }
    public class BindableUIStatement
    {
        internal int _key;
        public int Key { get { return _key; } }
        internal string _ApplicationName;
        public string ApplicationName { get { return _ApplicationName; } }
        internal string _StatementName;
        public string StatementName { get { return _StatementName; } }
        internal DateTime _ModifyDate;
        public DateTime ModifyDate { get { return _ModifyDate; } }
        internal int _Version;
        public int Version { get { return _Version; } }
        internal string _ModifyUser;
        public string ModifyUser { get { return _ModifyUser; } }
        internal string _Statement;
        public string Statement { get { return _Statement; } }
        internal int _type;
        public int StatementType { get { return _type; } }

        public BindableUIStatement() { }
        public BindableUIStatement(IUIStatement s)
        {
            this._key = s.Key;
            this._ApplicationName = s.ApplicationName;
            this._StatementName = s.StatementName;
            this._ModifyDate = s.ModifyDate;
            this._Version = s.Version;
            this._ModifyUser = s.ModifyUser;
            this._Statement = s.Statement;
            this._type = s.uiStatementType.Key;
        }

    }

    public interface IViewCBOMenu : IViewBase
    {
        void BindCBOList(List<IBindableTreeItem> CBONodes, int TopLevelKey);
        bool ShowAllCBO { set; }
        void BindCBO(ICBOMenu CBO);
        event EventHandler OnNextClick;
        event EventHandler OnTreeSelected;
        void BindGenericKeyType(List<BindableGenericKeyType> Bind);
        string Desc { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string URL { get; }
        char NodeType { get; }
        int Sequence { get; }
        string MenuIcon { get; }
        int GenericKeyTYpe { get; }
        bool HasOriginationSource { get; }
        bool IsRemovable { get; }
        bool IncludeParentHeaderIcons { get; }
        string ExpandLevel { get; }
        bool VisibleMaint { set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Statements"></param>
        /// /// <param name="StatementName"></param>
        void BindUIStatement(List<BindableUIStatement> Statements, string StatementName);
        /// <summary>
        /// 
        /// </summary>
        string UIStatementName { get; }

    }
}
