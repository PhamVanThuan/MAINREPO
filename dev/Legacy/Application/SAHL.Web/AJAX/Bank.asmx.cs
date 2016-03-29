using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for Bank
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Bank : System.Web.Services.WebService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="BankAccountKey"></param>
        /// <returns></returns>
        [WebMethod]        
        public SAHLAutoCompleteItem[] GetBranches(string Prefix, string BankAccountKey)
        {
            int key = 0;
            if (Int32.TryParse(BankAccountKey, out key))
            {
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                IReadOnlyEventList<IACBBranch> branches = BAR.GetACBBranchesByPrefix(int.Parse(BankAccountKey),Prefix, 15);

                SAHLAutoCompleteItem[] items = new SAHLAutoCompleteItem[branches.Count];

                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new SAHLAutoCompleteItem(branches[i].Key.ToString(), branches[i].Key + " - " + branches[i].ACBBranchDescription);
                }

                return items;
            }
            else
            {
                return new SAHLAutoCompleteItem[0];
            }
        }
    }
}
