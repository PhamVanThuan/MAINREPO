using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Web.Views.Common
{
    public partial class OrganisationalStructure : SAHLCommonBaseView, IOrganisationalStructure
    {
        protected void Page_Load(object sender, EventArgs e)
        {

//            IOrganisationStructureRepository orgRep = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
//            IEventList<IOrganisationalStructure> list = orgRep.GetTopLevelOrganisationStructureForOriginationSource((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);            
            //foreach (IOrganisationalStructure n in list)
            //{
            //    TreeNode node= new TreeNode();
            //    node.Text = n.ViewName;
            //    treeOS.Nodes.Add(node);
            //}


        }
    }
}
