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
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IViewFeature : IViewBase
    {
        /// <summary>
        /// Instructs the view to bind the treeview to a list of features.
        /// </summary>
        /// <param name="Features"></param>
        void BindFeatureTree(List<IBindableTreeItem> Features);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        void BindFeature(BindableFeature feature);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Desc"></param>
        void BindParent(int Key, string Desc);
        /// <summary>
        /// 
        /// </summary>
        bool VisibleMaint { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleButtons { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleFeatureList { set;}
        /// <summary>
        /// 
        /// </summary>
        string LongName { get; }
        /// <summary>
        /// 
        /// </summary>
        string ShortName { get; }
        /// <summary>
        /// 
        /// </summary>
        int Key { get; }

        /// <summary>
        /// 
        /// </summary>
        int ParentKey { get; }
        /// <summary>
        /// 
        /// </summary>
        int Sequence { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FeatureList"></param>
        void BindFeatureList(IEventList<IFeature> FeatureList);
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitClick;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnTreeSelected;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnFeatureSelectedItemChanged;
    }

    
    public class BindableFeature : IComparer<BindableFeature>, IBindableTreeItem
    {
        internal int _Key;
        public int Key { get { return _Key; } }
        public string Desc { get { return _ShortName; } }
        internal string _ShortName;
        public string ShortName { get { return _ShortName; } }
        internal string _LongName;
        public string LongName { get { return _LongName; } }
        internal bool _HasAccess;
        public bool HasAccess { get { return _HasAccess; } }
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Exposed for binding.")]
        internal List<IBindableTreeItem> _Children = new List<IBindableTreeItem>();
        public List<IBindableTreeItem> Children { get { return _Children; } }
        internal int _Sequence;
        public int Sequence { get { return _Sequence; } }
        internal int _ParentKey = -1;
        public int ParentKey { get { return _ParentKey; } }
        internal string _ParentShort;
        public string ParentShort { get { return _ParentShort; } }
        internal BindableFeature() { }
        internal BindableFeature(SAHL.Common.BusinessModel.Interfaces.IFeature f, bool LoadLittleBrats)
        {
            Populate(f, LoadLittleBrats);
        }
        internal void Populate(IFeature f, bool LoadLittleBrats)
        {
            try
            {
                this._Key = f.Key;
                this._ShortName = f.ShortName;
                this._LongName = f.LongName;
                this._HasAccess = f.HasAccess;
                this._Sequence = f.Sequence;
                if (LoadLittleBrats)
                {
                    if (f.ChildFeatures.Count > 0)
                    {
                        foreach (SAHL.Common.BusinessModel.Interfaces.IFeature child in f.ChildFeatures)
                        {
                            _Children.Add(new BindableFeature(child, LoadLittleBrats));
                        }
                    }
                }
                if (null != f.ParentFeature)
                {
                    _ParentKey = f.ParentFeature.Key;
                    _ParentShort = f.ParentFeature.ShortName;
                }
            }
            catch (Exception)
            {
                // string s = ex.ToString();
                throw;
            }
        }

        //int IComparer.Compare(Object x, Object y)
        //{
        //    string a = ((BindableFeature)x).ShortName;
        //    string b = ((BindableFeature)y).ShortName;
        //    return string.Compare(a, b);
        //}

        #region IComparer<BindableFeature> Members

        public int Compare(BindableFeature x, BindableFeature y)
        {
            return string.Compare(x.ShortName, y.ShortName); 
        }

        #endregion
    }
}
