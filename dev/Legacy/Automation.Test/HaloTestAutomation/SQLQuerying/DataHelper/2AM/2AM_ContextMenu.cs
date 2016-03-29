using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public IEnumerable<Automation.DataModels.ContextMenu> GetContextMenuItemsByCBOKey(int cboKey)
        {
            var contextMenu =
                dataContext.Query<Automation.DataModels.ContextMenu>
                (string.Format(@"select cm.description as ParentNode, cm.url as ParentURL, isnull(children.description,'') as ChildNode,
                                    isnull(children.url,'') as ChildURL
                                    from [2am].dbo.[ContextMenu] cm
                                    left join [2am].dbo.[ContextMenu] children on cm.[ContextKey] = children.[ParentKey]
                                    where cm.corebusinessobjectkey= {0} ", cboKey));
            return contextMenu;
        }
    }
}