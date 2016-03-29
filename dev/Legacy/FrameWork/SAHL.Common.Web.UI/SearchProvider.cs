using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SAHL.Common.Web.UI
{
    public enum SearchParameterDisplayType
    {
        Label,
        DropList
    }

    public interface ISearchParameter
    {
        string Name{get;}
        string DisplayName { get;}
        SearchParameterDisplayType DisplayType { get;}
        string DataType { get;}
    }

    public interface ISearchProvider
    {
        List<ISearchParameter> Parameters {get;}
        int StatementDefinitionKey { get;}

        void OnPostBack(Dictionary<string, string> PostBackVars);
        DataTable Search(Dictionary<string, object> Parameters);
    }
}
